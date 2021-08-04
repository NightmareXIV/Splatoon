using Dalamud.Game.ClientState.Actors.Types.NonPlayer;
using Dalamud.Interface;
using Dalamud.Plugin;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using Num = System.Numerics;

namespace Splatoon
{
    unsafe class Gui : IDisposable
    {
        readonly Splatoon p;
        int uid = 0;
        public Gui(Splatoon p)
        {
            this.p = p;
            p.pi.UiBuilder.OnBuildUi += Draw;
        }

        public void Dispose()
        {
            p.pi.UiBuilder.OnBuildUi -= Draw;
        }

        [HandleProcessCorruptedStateExceptions]
        void Draw()
        {
            try
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
                    ImGui.SetWindowPos(new Num.Vector2(0, 0));
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
            catch(Exception e)
            {
                p.Log("Caught exception: " + e.Message, true);
                p.Log(e.StackTrace, true);
            }
        }

        void DrawLineWorld(DisplayObjectLine e)
        {
            var pointA = new Vector3(e.ax, e.ay, e.az);
            var pointB = new Vector3(e.bx, e.by, e.bz);
            var resultA = p.pi.Framework.Gui.WorldToScreen(new SharpDX.Vector3(e.ax, e.az, e.ay), out SharpDX.Vector2 posA);
            if (!resultA)
            {
                var posA2 = GetLineClosestToVisiblePoint(pointA,
                (pointB - pointA) / p.CurrentLineSegments, 0, p.CurrentLineSegments);
                if (posA2 == null)
                {
                    return;
                }
                else
                {
                    posA = posA2.Value;
                }
            }
            var resultB = p.pi.Framework.Gui.WorldToScreen(new SharpDX.Vector3(e.bx, e.bz, e.by), out SharpDX.Vector2 posB);
            if (!resultB)
            {
                var posB2 = GetLineClosestToVisiblePoint(pointB,
                (pointA - pointB) / p.CurrentLineSegments, 0, p.CurrentLineSegments);
                if (posB2 == null)
                {
                    return;
                }
                else
                {
                    posB = posB2.Value;
                }
            }

            ImGui.GetWindowDrawList().PathLineTo(new Num.Vector2(posA.X, posA.Y));
            ImGui.GetWindowDrawList().PathLineTo(new Num.Vector2(posB.X, posB.Y));
            ImGui.GetWindowDrawList().PathStroke(e.color, ImDrawFlags.None, e.thickness);
        }

        SharpDX.Vector2? GetLineClosestToVisiblePoint(Vector3 currentPos, Vector3 delta, int curSegment, int numSegments)
        {
            if (curSegment > numSegments) return null;
            var nextPos = currentPos + delta;
            if(p.pi.Framework.Gui.WorldToScreen(new SharpDX.Vector3(nextPos.X, nextPos.Z, nextPos.Y), out SharpDX.Vector2 pos))
            {
                var preciseDelta = (nextPos - currentPos) / p.Config.lineSegments;
                var preciseVector = GetLineClosestToVisiblePoint(currentPos, preciseDelta, 0, p.Config.lineSegments);
                return preciseVector.HasValue?preciseVector.Value:pos;
            }
            else
            {
                return GetLineClosestToVisiblePoint(nextPos, delta, ++curSegment, numSegments);
            }
        }

        public void DrawTextWorld(DisplayObjectText e)
        {
            if (p.pi.Framework.Gui.WorldToScreen(
                            new SharpDX.Vector3(e.x, e.z, e.y),
                            out SharpDX.Vector2 pos))
            {
                var size = ImGui.CalcTextSize(e.text);
                size = new Num.Vector2(size.X + 10f, size.Y + 10f);
                ImGui.SetNextWindowPos(new Num.Vector2(pos.X - size.X / 2, pos.Y - size.Y / 2));
                ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, new Num.Vector2(5, 5));
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
            p.pi.Framework.Gui.WorldToScreen(new SharpDX.Vector3(e.x + (e.radius * (float)Math.Sin(p.CamAngleX)), e.z,
            e.y + (e.radius * (float)Math.Cos(p.CamAngleX))), out SharpDX.Vector2 refpos);
            var visible = false;
            Num.Vector2?[] elements = new Num.Vector2?[p.Config.segments+1];
            for (int i = 0; i <= p.Config.segments; i++)
            {
                visible = p.pi.Framework.Gui.WorldToScreen(
                    new SharpDX.Vector3(e.x + (e.radius * (float)Math.Sin((Math.PI / seg) * i)),
                    e.z,
                    e.y + (e.radius * (float)Math.Cos((Math.PI / seg) * i))),
                    out SharpDX.Vector2 pos) 
                    || visible;
                if (pos.Y > refpos.Y) elements[i] = new Num.Vector2(pos.X, pos.Y);
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
            if(p.pi.Framework.Gui.WorldToScreen(new SharpDX.Vector3(e.x, e.z, e.y), out SharpDX.Vector2 pos)) 
                ImGui.GetWindowDrawList().AddCircleFilled(
                new Num.Vector2(pos.X, pos.Y),
                e.thickness,
                ImGui.GetColorU32(e.color),
                100);
        }
    }
}
