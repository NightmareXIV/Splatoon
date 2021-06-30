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
            p._pi.UiBuilder.OnBuildUi += Draw;
        }

        public void Dispose()
        {
            p._pi.UiBuilder.OnBuildUi -= Draw;
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
                try
                {
                    ImGuiHelpers.ForceNextWindowMainViewport();
                    ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, new Num.Vector2(0, 0));
                    ImGui.Begin("Splatoon ring", ImGuiWindowFlags.NoInputs | ImGuiWindowFlags.NoNav | ImGuiWindowFlags.NoTitleBar
                        | ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoBackground);
                    ImGui.SetWindowPos(new Num.Vector2(0, 0));
                    ImGui.SetWindowSize(ImGui.GetIO().DisplaySize);
                    foreach (var element in p.displayObjects)
                    {
                        if (element is DisplayObjectCircle)
                        {
                            var e = (DisplayObjectCircle)element;
                            DrawRingWorld(e.x, e.y, e.z, e.radius, p.Config.segments, e.thickness, e.color);
                        }
                        else if (element is DisplayObjectDot)
                        {
                            var e = (DisplayObjectDot)element;
                            DrawPoint(e.x, e.y, e.z, e.thickness, e.color);
                        }
                        else if (element is DisplayObjectText)
                        {
                            var e = (DisplayObjectText)element;
                            DrawTextWorld(e.x, e.y, e.z, e.text, e.bgcolor, e.fgcolor);
                        }
                        else if (element is DisplayObjectLine)
                        {
                            var e = (DisplayObjectLine)element;
                            p._pi.Framework.Gui.WorldToScreen(new SharpDX.Vector3(e.ax, e.az, e.ay), out SharpDX.Vector2 pos);
                            ImGui.GetWindowDrawList().PathLineTo(new Num.Vector2(pos.X, pos.Y));
                            p._pi.Framework.Gui.WorldToScreen(new SharpDX.Vector3(e.bx, e.bz, e.by), out SharpDX.Vector2 pos2);
                            ImGui.GetWindowDrawList().PathLineTo(new Num.Vector2(pos2.X, pos2.Y));
                            ImGui.GetWindowDrawList().PathStroke(e.color, ImDrawFlags.None, e.thickness);
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

        public void DrawTextWorld(float x, float y, float z, string text, uint bgcolor, uint fgcolor)
        {
            if (p._pi.Framework.Gui.WorldToScreen(
                            new SharpDX.Vector3(x, z, y),
                            out SharpDX.Vector2 pos))
            {
                var size = ImGui.CalcTextSize(text);
                size = new Num.Vector2(size.X + 10f, size.Y + 10f);
                ImGui.SetNextWindowPos(new Num.Vector2(pos.X - size.X / 2, pos.Y - size.Y / 2));
                ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, new Num.Vector2(5, 5));
                ImGui.PushStyleVar(ImGuiStyleVar.ChildRounding, 10f);
                ImGui.PushStyleColor(ImGuiCol.ChildBg, ImGui.ColorConvertU32ToFloat4(bgcolor));
                ImGui.BeginChild("##child" + text + ++uid, size, false,
                    ImGuiWindowFlags.NoInputs | ImGuiWindowFlags.NoNav
                    | ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.AlwaysUseWindowPadding);
                ImGui.TextColored(ImGui.ColorConvertU32ToFloat4(fgcolor), text);
                ImGui.EndChild();
                ImGui.PopStyleColor();
                ImGui.PopStyleVar(2);
            }
        }

        public void DrawRingWorld(float x, float y, float z, float radius, int num_segments, float thicc, uint colour)
        {
            int seg = num_segments / 2;
            p._pi.Framework.Gui.WorldToScreen(new SharpDX.Vector3(x + (radius * (float)Math.Sin(p.CamAngleX)), z,
            y + (radius * (float)Math.Cos(p.CamAngleX))), out SharpDX.Vector2 refpos); 
            for (int i = 0; i <= num_segments; i++)
            {
                p._pi.Framework.Gui.WorldToScreen(
                    new SharpDX.Vector3(x + (radius * (float)Math.Sin((Math.PI / seg) * i)),
                    z,
                    y + (radius * (float)Math.Cos((Math.PI / seg) * i))),
                    out SharpDX.Vector2 pos);
                if (pos.Y > refpos.Y) ImGui.GetWindowDrawList().PathLineTo(new Num.Vector2(pos.X, pos.Y));
            }
            ImGui.GetWindowDrawList().PathStroke(colour, ImDrawFlags.Closed, thicc);
        }

        public void DrawPoint(float x, float y, float z, float thicc, uint col)
        {
            if(p._pi.Framework.Gui.WorldToScreen(new SharpDX.Vector3(x, z, y), out SharpDX.Vector2 pos)) 
                ImGui.GetWindowDrawList().AddCircleFilled(
                new Num.Vector2(pos.X, pos.Y),
                thicc,
                ImGui.GetColorU32(col),
                100);
        }
    }

    internal class DisplayObjectDot : DisplayObject
    {
        public float x, y, z, thickness;
        public uint color;

        public DisplayObjectDot(float x, float y, float z, float thickness, uint color)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.thickness = thickness;
            this.color = color;
        }
    }

    internal class DisplayObjectCircle : DisplayObject
    {
        public float x, y, z, radius, thickness;
        public uint color;
        public DisplayObjectCircle(float x, float y, float z, float radius, float thickness, uint color)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.radius = radius;
            this.thickness = thickness;
            this.color = color;
        }
    }

    internal class DisplayObjectLine : DisplayObject
    {
        public float ax, ay, az, bx, by, bz, thickness;
        public uint color;

        public DisplayObjectLine(float ax, float ay, float az, float bx, float by, float bz, float thickness, uint color)
        {
            this.ax = ax;
            this.ay = ay;
            this.az = az;
            this.bx = bx;
            this.by = by;
            this.bz = bz;
            this.thickness = thickness;
            this.color = color;
        }
    }

    internal class DisplayObjectText : DisplayObject
    {
        public float x, y, z;
        public string text;
        public uint bgcolor, fgcolor;

        public DisplayObjectText(float x, float y, float z, string text, uint bgcolor, uint fgcolor)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.text = text;
            this.bgcolor = bgcolor;
            this.fgcolor = fgcolor;
        }
    }

    internal interface DisplayObject { }
}
