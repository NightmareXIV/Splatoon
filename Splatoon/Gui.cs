using Dalamud.Game.ClientState.Conditions;
using ECommons.MathHelpers;
using Splatoon.External;

namespace Splatoon;

unsafe class Gui : IDisposable
{
    readonly Splatoon p;
    int uid = 0;
    public Gui(Splatoon p)
    {
        this.p = p;
        Svc.PluginInterface.UiBuilder.Draw += Draw;
    }

    public void Dispose()
    {
        Svc.PluginInterface.UiBuilder.Draw -= Draw;
    }

    [HandleProcessCorruptedStateExceptions]
    void Draw()
    {
        if (p.Profiler.Enabled) p.Profiler.Gui.StartTick();
        try
        {
            if (!Svc.Condition[ConditionFlag.OccupiedInCutSceneEvent]
                && !Svc.Condition[ConditionFlag.WatchingCutscene78])
            {
                uid = 0;
                if (p.Config.segments > 1000 || p.Config.segments < 4)
                {
                    p.Config.segments = 100;
                    p.Log("Your smoothness setting was unsafe. It was reset to 100.");
                }
                if (p.Config.lineSegments > 50 || p.Config.lineSegments < 4)
                {
                    p.Config.lineSegments = 20;
                    p.Log("Your line segment setting was unsafe. It was reset to 20.");
                }
                try
                {
                    ImGuiHelpers.ForceNextWindowMainViewport();
                    ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, Vector2.Zero);
                    ImGuiHelpers.SetNextWindowPosRelativeMainViewport(Vector2.Zero);
                    ImGui.SetNextWindowSize(ImGuiHelpers.MainViewport.Size);
                    ImGui.Begin("Splatoon ring", ImGuiWindowFlags.NoInputs | ImGuiWindowFlags.NoNav | ImGuiWindowFlags.NoTitleBar
                        | ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoBackground | ImGuiWindowFlags.AlwaysUseWindowPadding);
                    foreach (var element in p.displayObjects)
                    {
                        if (element is DisplayObjectCircle elementCircle)
                        {
                            DrawRingWorld(elementCircle);
                        }
                        else if (element is DisplayObjectDot elementDot)
                        {
                            DrawPoint(elementDot);
                        }
                        else if (element is DisplayObjectText elementText)
                        {
                            DrawTextWorld(elementText);
                        }
                        else if (element is DisplayObjectLine elementLine)
                        {
                            DrawLineWorld(elementLine);
                        }
                        else if (element is DisplayObjectRect elementRect)
                        {
                            DrawRectWorld(elementRect);
                        }
                        else if (element is DisplayObjectCone elementCone)
                        {
                            var brush = new Brush()
                            {
                                Color = elementCone.e.color.ToVector4(),
                                Fill = elementCone.e.Filled ? elementCone.e.color.ToVector4() : Vector4.Zero,
                                Thickness = elementCone.e.thicc
                            };
                            Canvas.ConeXZ(elementCone.v, elementCone.radius,
                                (elementCone.angle.RadToDeg() - elementCone.e.coneAngleMin.Float()).DegreesToRadians(),
                                (elementCone.angle.RadToDeg() - elementCone.e.coneAngleMax.Float()).DegreesToRadians(), brush);
                        }
                        /*else if(element is DisplayObjectPolygon elementPolygon)
                        {
                            DrawPolygon(elementPolygon);
                        }*/
                    }
                    ImGui.End();
                    ImGui.PopStyleVar();
                }
                catch (Exception e)
                {
                    p.Log("Splatoon exception: please report it to developer", true);
                    p.Log(e.Message, true);
                    p.Log(e.StackTrace, true);
                }
            }
        }
        catch(Exception e)
        {
            p.Log("Caught exception: " + e.Message, true);
            p.Log(e.StackTrace, true);
        }
        if (p.Profiler.Enabled) p.Profiler.Gui.StopTick();
    }

    void DrawLineWorld(DisplayObjectLine e)
    {
        if (p.Profiler.Enabled) p.Profiler.GuiLines.StartTick();
        var result = GetAdjustedLine(new Vector3(e.ax, e.ay, e.az), new Vector3(e.bx, e.by, e.bz));
        if (result.posA == null) return;
        ImGui.GetWindowDrawList().PathLineTo(new Vector2(result.posA.Value.X, result.posA.Value.Y));
        ImGui.GetWindowDrawList().PathLineTo(new Vector2(result.posB.Value.X, result.posB.Value.Y));
        ImGui.GetWindowDrawList().PathStroke(e.color, ImDrawFlags.None, e.thickness);
        if (p.Profiler.Enabled) p.Profiler.GuiLines.StopTick();
    }

    (Vector2? posA, Vector2? posB) GetAdjustedLine(Vector3 pointA, Vector3 pointB)
    {
        var resultA = p.MemoryManager.WorldToScreen(new Vector3(pointA.X, pointA.Z, pointA.Y), out Vector2 posA);
        if (!resultA && !p.DisableLineFix)
        {
            var posA2 = GetLineClosestToVisiblePoint(pointA,
            (pointB - pointA) / p.CurrentLineSegments, 0, p.CurrentLineSegments);
            if (posA2 == null)
            {
                if (p.Profiler.Enabled) p.Profiler.GuiLines.StopTick();
                return (null, null);
            }
            else
            {
                posA = posA2.Value;
            }
        }
        var resultB = p.MemoryManager.WorldToScreen(new Vector3(pointB.X, pointB.Z, pointB.Y), out Vector2 posB);
        if (!resultB && !p.DisableLineFix)
        {
            var posB2 = GetLineClosestToVisiblePoint(pointB,
            (pointA - pointB) / p.CurrentLineSegments, 0, p.CurrentLineSegments);
            if (posB2 == null)
            {
                if (p.Profiler.Enabled) p.Profiler.GuiLines.StopTick();
                return (null, null);
            }
            else
            {
                posB = posB2.Value;
            }
        }
        return (posA, posB);
    }

    void DrawRectWorld(DisplayObjectRect e) //oof
    {
        if (p.Profiler.Enabled) p.Profiler.GuiLines.StartTick();
        var result1 = GetAdjustedLine(new Vector3(e.l1.ax, e.l1.ay, e.l1.az), new Vector3(e.l1.bx, e.l1.by, e.l1.bz));
        if (result1.posA == null) goto Alternative;
        var result2 = GetAdjustedLine(new Vector3(e.l2.ax, e.l2.ay, e.l2.az), new Vector3(e.l2.bx, e.l2.by, e.l2.bz));
        if (result2.posA == null) goto Alternative;
        goto Build;
        Alternative:
        result1 = GetAdjustedLine(new Vector3(e.l1.ax, e.l1.ay, e.l1.az), new Vector3(e.l2.ax, e.l2.ay, e.l2.az));
        if (result1.posA == null) goto Quit;
        result2 = GetAdjustedLine(new Vector3(e.l1.bx, e.l1.by, e.l1.bz), new Vector3(e.l2.bx, e.l2.by, e.l2.bz));
        if (result2.posA == null) goto Quit;
        Build:
        ImGui.GetWindowDrawList().AddQuadFilled(
            new Vector2(result1.posA.Value.X, result1.posA.Value.Y),
            new Vector2(result1.posB.Value.X, result1.posB.Value.Y),
            new Vector2(result2.posB.Value.X, result2.posB.Value.Y),
            new Vector2(result2.posA.Value.X, result2.posA.Value.Y), e.l1.color
            );
        Quit:
        if (p.Profiler.Enabled) p.Profiler.GuiLines.StopTick();
    }

    Vector2? GetLineClosestToVisiblePoint(Vector3 currentPos, Vector3 delta, int curSegment, int numSegments)
    {
        if (curSegment > numSegments) return null;
        var nextPos = currentPos + delta;
        if(p.MemoryManager.WorldToScreen(new Vector3(nextPos.X, nextPos.Z, nextPos.Y), out Vector2 pos))
        {
            var preciseVector = GetLineClosestToVisiblePoint(currentPos, (nextPos - currentPos) / p.Config.lineSegments, 0, p.Config.lineSegments);
            return preciseVector.HasValue?preciseVector.Value:pos;
        }
        else
        {
            return GetLineClosestToVisiblePoint(nextPos, delta, ++curSegment, numSegments);
        }
    }

    public void DrawTextWorld(DisplayObjectText e)
    {
        if (p.MemoryManager.WorldToScreen(
                        new Vector3(e.x, e.z, e.y),
                        out Vector2 pos))
        {
            DrawText(e, pos);
        }
    }

    public void DrawText(DisplayObjectText e, Vector2 pos)
    {
        var scaled = e.fscale != 1f;
        var size = scaled? ImGui.CalcTextSize(e.text)*e.fscale: ImGui.CalcTextSize(e.text);
        size = new Vector2(size.X + 10f, size.Y + 10f);
        ImGui.SetNextWindowPos(new Vector2(pos.X - size.X / 2, pos.Y - size.Y / 2));
        ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, new Vector2(5, 5));
        ImGui.PushStyleVar(ImGuiStyleVar.ChildRounding, 10f);
        ImGui.PushStyleColor(ImGuiCol.ChildBg, ImGui.ColorConvertU32ToFloat4(e.bgcolor));
        ImGui.BeginChild("##child" + e.text + ++uid, size, false,
            ImGuiWindowFlags.NoInputs | ImGuiWindowFlags.NoNav
            | ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.AlwaysUseWindowPadding);
        ImGui.PushStyleColor(ImGuiCol.Text, e.fgcolor);
        if (scaled) ImGui.SetWindowFontScale(e.fscale);
        ImGui.TextUnformatted(e.text);
        if (scaled) ImGui.SetWindowFontScale(1f);
        ImGui.PopStyleColor();
        ImGui.EndChild();
        ImGui.PopStyleColor();
        ImGui.PopStyleVar(2);
    }

    public void DrawRingWorld(DisplayObjectCircle e)
    {
        int seg = p.Config.segments / 2;
        p.MemoryManager.WorldToScreen(new Vector3(
            e.x + (e.radius * (float)Math.Sin(p.CamAngleX)),
            e.z, 
            e.y + (e.radius * (float)Math.Cos(p.CamAngleX))
            ), out Vector2 refpos);
        var visible = false;
        Vector2?[] elements = new Vector2?[p.Config.segments];
        for (int i = 0; i < p.Config.segments; i++)
        {
            visible = p.MemoryManager.WorldToScreen(
                new Vector3(e.x + (e.radius * (float)Math.Sin((Math.PI / seg) * i)),
                e.z,
                e.y + (e.radius * (float)Math.Cos((Math.PI / seg) * i))
                ),
                out Vector2 pos) 
                || visible;
            if (pos.Y > refpos.Y || p.MemoryManager.ErrorCode != 0) elements[i] = new Vector2(pos.X, pos.Y);
        }
        if (visible)
        {
            foreach(var pos in elements)
            {
                if (pos == null) continue;
                ImGui.GetWindowDrawList().PathLineTo(pos.Value);
            }
            
            if (e.filled)
            {
                ImGui.GetWindowDrawList().PathFillConvex(e.color);
            }
            else
            {
                ImGui.GetWindowDrawList().PathStroke(e.color, ImDrawFlags.Closed, e.thickness);
            }
        }
    }

    public void DrawPoint(DisplayObjectDot e)
    {
        if(p.MemoryManager.WorldToScreen(new Vector3(e.x, e.z, e.y), out Vector2 pos)) 
            ImGui.GetWindowDrawList().AddCircleFilled(
            new Vector2(pos.X, pos.Y),
            e.thickness,
            ImGui.GetColorU32(e.color),
            100);
    }

    /*void DrawPolygon(DisplayObjectPolygon p)
    {
        var i = 0;
        var objects = new List<Action>();
        var coords = GetPolygon(p.e.Polygon.Select((x) =>
        {
            p.MemoryManager.WorldToScreen(new Vector3(x.X, x.Z, x.Y), out Vector2 pos);
            return pos;
        }).ToList());
        var medium = new Vector2(coords.Average(x => x.v2.X), coords.Average(x => x.v2.Y));
        DrawText(new DisplayObjectText(0, 0, 0, $"{medium.X}, {medium.Y}", 0xff000000, 0xff0000ff), medium);
        foreach (var c in coords)
        {
            ImGui.GetWindowDrawList().PathLineTo(c.v2);
            var txt = i.ToString();
            objects.Add(() => DrawText(new DisplayObjectText(0,0,0, $"{txt}: {c.v2.X}, {c.v2.Y}, {c.angle}", 0xff000000, 0xffffffff), c.v2));
            i++;
        }
        //ImGui.GetWindowDrawList().PathLineTo(coords.First().v2);

        if (p.e.Filled)
        {
            ImGui.GetWindowDrawList().PathFillConvex(p.e.color);
        }
        else
        {
            ImGui.GetWindowDrawList().PathStroke(p.e.color, ImDrawFlags.Closed, p.e.thicc);
        }
        foreach (var o in objects) o();
    }*/
}
