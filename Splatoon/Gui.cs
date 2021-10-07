using Dalamud.Game.ClientState.Conditions;

namespace Splatoon
{
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
                        ImGui.Begin("Splatoon ring", ImGuiWindowFlags.NoInputs | ImGuiWindowFlags.NoNav | ImGuiWindowFlags.NoTitleBar
                            | ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoBackground | ImGuiWindowFlags.AlwaysUseWindowPadding);
                        ImGui.SetWindowPos(new Vector2(0, 0));
                        ImGui.SetWindowSize(ImGui.GetIO().DisplaySize);
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
            var pointA = new Vector3(e.ax, e.ay, e.az);
            var pointB = new Vector3(e.bx, e.by, e.bz);
            var resultA = Svc.GameGui.WorldToScreen(new Vector3(e.ax, e.az, e.ay), out Vector2 posA);
            if (!resultA)
            {
                var posA2 = GetLineClosestToVisiblePoint(pointA,
                (pointB - pointA) / p.CurrentLineSegments, 0, p.CurrentLineSegments);
                if (posA2 == null)
                {
                    if (p.Profiler.Enabled) p.Profiler.GuiLines.StopTick();
                    return;
                }
                else
                {
                    posA = posA2.Value;
                }
            }
            var resultB = Svc.GameGui.WorldToScreen(new Vector3(e.bx, e.bz, e.by), out Vector2 posB);
            if (!resultB)
            {
                var posB2 = GetLineClosestToVisiblePoint(pointB,
                (pointA - pointB) / p.CurrentLineSegments, 0, p.CurrentLineSegments);
                if (posB2 == null)
                {
                    if (p.Profiler.Enabled) p.Profiler.GuiLines.StopTick();
                    return;
                }
                else
                {
                    posB = posB2.Value;
                }
            }

            ImGui.GetWindowDrawList().PathLineTo(new Vector2(posA.X, posA.Y));
            ImGui.GetWindowDrawList().PathLineTo(new Vector2(posB.X, posB.Y));
            ImGui.GetWindowDrawList().PathStroke(e.color, ImDrawFlags.None, e.thickness);
            if (p.Profiler.Enabled) p.Profiler.GuiLines.StopTick();
        }

        Vector2? GetLineClosestToVisiblePoint(Vector3 currentPos, Vector3 delta, int curSegment, int numSegments)
        {
            if (curSegment > numSegments) return null;
            var nextPos = currentPos + delta;
            if(Svc.GameGui.WorldToScreen(new Vector3(nextPos.X, nextPos.Z, nextPos.Y), out Vector2 pos))
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
            if (Svc.GameGui.WorldToScreen(
                            new Vector3(e.x, e.z, e.y),
                            out Vector2 pos))
            {
                var size = ImGui.CalcTextSize(e.text);
                size = new Vector2(size.X + 10f, size.Y + 10f);
                ImGui.SetNextWindowPos(new Vector2(pos.X - size.X / 2, pos.Y - size.Y / 2));
                ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, new Vector2(5, 5));
                ImGui.PushStyleVar(ImGuiStyleVar.ChildRounding, 10f);
                ImGui.PushStyleColor(ImGuiCol.ChildBg, ImGui.ColorConvertU32ToFloat4(e.bgcolor));
                ImGui.BeginChild("##child" + e.text + ++uid, size, false,
                    ImGuiWindowFlags.NoInputs | ImGuiWindowFlags.NoNav
                    | ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.AlwaysUseWindowPadding);
                ImGui.TextColored(ImGui.ColorConvertU32ToFloat4(e.fgcolor), e.text);
                ImGui.EndChild();
                ImGui.PopStyleColor();
                ImGui.PopStyleVar(2);
            }
        }

        public void DrawRingWorld(DisplayObjectCircle e)
        {
            int seg = p.Config.segments / 2;
            Svc.GameGui.WorldToScreen(new Vector3(
                e.x + (e.radius * (float)Math.Sin(p.CamAngleX)),
                e.z, 
                e.y + (e.radius * (float)Math.Cos(p.CamAngleX))
                ), out Vector2 refpos);
            var visible = false;
            Vector2?[] elements = new Vector2?[p.Config.segments];
            for (int i = 0; i < p.Config.segments; i++)
            {
                visible = Svc.GameGui.WorldToScreen(
                    new Vector3(e.x + (e.radius * (float)Math.Sin((Math.PI / seg) * i)),
                    e.z,
                    e.y + (e.radius * (float)Math.Cos((Math.PI / seg) * i))
                    ),
                    out Vector2 pos) 
                    || visible;
                if (pos.Y > refpos.Y) elements[i] = new Vector2(pos.X, pos.Y);
            }
            if (visible)
            {
                foreach(var pos in elements)
                {
                    if (pos == null) continue;
                    ImGui.GetWindowDrawList().PathLineTo(pos.Value);
                }
                ImGui.GetWindowDrawList().PathStroke(e.color, ImDrawFlags.Closed, e.thickness);
            }
        }

        public void DrawPoint(DisplayObjectDot e)
        {
            if(Svc.GameGui.WorldToScreen(new Vector3(e.x, e.z, e.y), out Vector2 pos)) 
                ImGui.GetWindowDrawList().AddCircleFilled(
                new Vector2(pos.X, pos.Y),
                e.thickness,
                ImGui.GetColorU32(e.color),
                100);
        }
    }
}
