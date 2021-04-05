using Dalamud.Game.ClientState.Actors.Types.NonPlayer;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Num = System.Numerics;

namespace Splatoon
{
    class Gui : IDisposable
    {
        readonly Splatoon p;
        public Gui(Splatoon p)
        {
            this.p = p;
            p._pi.UiBuilder.OnBuildUi += Draw;
        }

        public void Dispose()
        {
            p._pi.UiBuilder.OnBuildUi -= Draw;
        }

        void Draw()
        {
            int uid = 0;
            if (p._pi.ClientState == null || p._pi.ClientState.LocalPlayer == null) return;
            ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, new Num.Vector2(0, 0));
            ImGui.Begin("Splatoon ring", ImGuiWindowFlags.NoInputs | ImGuiWindowFlags.NoNav | ImGuiWindowFlags.NoTitleBar
                | ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoBackground);
            ImGui.SetWindowPos(new Num.Vector2(0, 0));
            ImGui.SetWindowSize(ImGui.GetIO().DisplaySize);
            foreach(var i in p.Config.Layouts.Values)
            {
                if (!i.Enabled) continue;
                if (i.ZoneLock != 0 && i.ZoneLock != p._pi.ClientState.TerritoryType) continue;
                foreach(var e in i.Elements.Values.ToArray())
                {
                    if (!e.Enabled) continue;
                    if(e.type == 0)
                    {
                        if (e.thicc > 0)
                        {
                            if (e.radius > 0)
                            {
                                DrawRingWorld(e.refX + e.offX, e.refY + e.offY, e.refZ + e.offZ, e.radius, 100, e.thicc, e.color);
                            }
                            else
                            {
                                DrawPoint(e.refX + e.offX, e.refY + e.offY, e.refZ + e.offZ, e.thicc, e.color);
                            }
                        }
                        if (e.overlayText.Length > 0)
                        {
                            if(p._pi.Framework.Gui.WorldToScreen(
                                new SharpDX.Vector3(e.refX + e.offX, e.refZ + e.offZ, e.refY + e.offY), 
                                out SharpDX.Vector2 pos))
                            {
                                var size = ImGui.CalcTextSize(e.overlayText);
                                size = new Num.Vector2(size.X + 10f, size.Y + 10f);
                                ImGui.SetNextWindowPos(new Num.Vector2(pos.X-size.X/2, pos.Y-size.Y/2-e.overlayVOffset));
                                ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, new Num.Vector2(5, 5));
                                ImGui.PushStyleVar(ImGuiStyleVar.ChildRounding, 10f);
                                ImGui.PushStyleColor(ImGuiCol.ChildBg, ImGui.ColorConvertU32ToFloat4(e.overlayBGColor));
                                ImGui.BeginChild("##child" + e.overlayText + ++uid, size, false, 
                                    ImGuiWindowFlags.NoInputs | ImGuiWindowFlags.NoNav 
                                    | ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.AlwaysUseWindowPadding);
                                ImGui.TextColored(ImGui.ColorConvertU32ToFloat4(e.overlayTextColor), e.overlayText);
                                ImGui.EndChild();
                                ImGui.PopStyleColor();
                                ImGui.PopStyleVar(2);
                            }
                        }
                    }
                }
            }
            ImGui.End();
            ImGui.PopStyleVar();
        }

        public void DrawRingWorld(float x, float y, float z, float radius, int num_segments, float thicc, uint colour)
        {
            int seg = num_segments / 2;

            for (int i = 0; i <= num_segments; i++)
            {
                p._pi.Framework.Gui.WorldToScreen(new SharpDX.Vector3(x + (radius * (float)Math.Sin((Math.PI / seg) * i)), z, y + (radius * (float)Math.Cos((Math.PI / seg) * i))), out SharpDX.Vector2 pos);
                ImGui.GetWindowDrawList().PathLineTo(new System.Numerics.Vector2(pos.X, pos.Y));
            }
            ImGui.GetWindowDrawList().PathStroke(colour, true, thicc);
        }

        public void DrawPoint(float x, float y, float z, float thicc, uint col)
        {
            p._pi.Framework.Gui.WorldToScreen(new SharpDX.Vector3(x, z, y), out SharpDX.Vector2 pos);
            ImGui.GetWindowDrawList().AddCircleFilled(
                new Num.Vector2(pos.X, pos.Y),
                thicc,
                ImGui.GetColorU32(col),
                100);
        }
    }
}
