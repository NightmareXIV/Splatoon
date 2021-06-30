using Dalamud.Game.ClientState.Actors.Types;
using Dalamud.Game.ClientState.Actors.Types.NonPlayer;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Splatoon
{
    unsafe class DGui : IDisposable
    {
        private Splatoon p;
        public bool Open = false;
        bool autoscrollLog = true;
        float s2wx, s2wy, s2wz, s2wrx, s2wry;
        bool s2wb = false;
        public DGui(Splatoon p)
        {
            this.p = p;
            p._pi.UiBuilder.OnBuildUi += Draw;
        }

        public void Dispose()
        {
            p._pi.UiBuilder.OnBuildUi -= Draw;
        }

        public void Draw()
        {
            if (!Open) return;
            try
            {
                if (ImGui.Begin("Splatoon debug", ref Open))
                {
                    ImGui.Columns(2);
                    ImGui.BeginChild("##splatoondbg1");
                    var t = DateTimeOffset.Now.ToUnixTimeSeconds() - p.CombatStarted;
                    ImGui.Text("CombatStarted = " + t);
                    ImGui.Separator();
                    if (p._pi.ClientState.LocalPlayer != null)
                    {
                        var mypos = p._pi.ClientState.LocalPlayer.Position;
                        ImGui.Text("My pos XYZ: " + mypos.X + ", " + mypos.Y + ", " + mypos.Z);
                        var tar = p._pi.ClientState.Targets.CurrentTarget;
                        if (tar != null)
                        {
                            ImGui.Text("Target pos XYZ: " + tar.Position.X + ", " + tar.Position.Y + ", " + tar.Position.Z);
                        }
                    }
                    ImGui.Separator();
                    ImGui.Text("Query WorldToScreen");
                    ImGui.PushItemWidth(50f);
                    ImGui.DragFloat("##qs2wx", ref s2wx);
                    ImGui.SameLine();
                    ImGui.DragFloat("##qs2wy", ref s2wy);
                    ImGui.SameLine();
                    ImGui.DragFloat("##qs2wz", ref s2wz);
                    ImGui.PopItemWidth();
                    if (ImGui.Button("To my pos"))
                    {
                        var mypos = p._pi.ClientState.LocalPlayer.Position;
                        s2wx = mypos.X;
                        s2wy = mypos.Y;
                        s2wz = mypos.Z;
                    }
                    ImGui.SameLine();
                    if (ImGui.Button("Query"))
                    {
                        s2wb = p._pi.Framework.Gui.WorldToScreen(new SharpDX.Vector3(s2wx, s2wz, s2wy), out SharpDX.Vector2 pos);
                        s2wrx = pos.X;
                        s2wry = pos.Y;
                    }
                    ImGui.TextColored(ImGui.ColorConvertU32ToFloat4(s2wb?Colors.Green:Colors.Red), "X:"+s2wrx+"\nY:"+ s2wry);
                    ImGui.Separator();
                    ImGui.Text("Camera angle X:" + p.CamAngleX);
                    ImGui.Text("Camera angle Y:" + p.CamAngleY);
                    ImGui.Separator();
                    ImGui.Text("Actors:");
                    foreach (var a in p._pi.ClientState.Actors)
                    {
                        try
                        {
                            ImGui.Text(a.Name);
                        }
                        catch (Exception e)
                        {
                            ImGui.Text(e.Message);
                        }
                    }
                    ImGui.EndChild();
                    ImGui.NextColumn();
                    ImGui.Text("Log:");
                    ImGui.SameLine();
                    ImGui.Checkbox("Autoscroll##log", ref autoscrollLog);
                    ImGui.SameLine();
                    if (ImGui.Button("Copy all"))
                    {
                        var s = new StringBuilder();
                        for (int i = 0; i < p.LogStorage.Length; i++)
                        {
                            if (p.LogStorage[i] != null)
                            {
                                s.AppendLine(p.LogStorage[i]);
                            }
                            else
                            {
                                break;
                            }
                        }
                        Clipboard.SetText(s.ToString());
                    }

                    ImGui.Checkbox("Copy in Dalamud.log##log", ref p.Config.dumplog);
                    ImGui.SameLine();
                    ImGui.Checkbox("Verbose##log", ref p.Config.verboselog);
                    ImGui.BeginChild("##splatoondbg2");
                    for (var i = 0; i < p.LogStorage.Length; i++)
                    {
                        if (p.LogStorage[i] != null) ImGui.TextWrapped(p.LogStorage[i]);
                    }
                    if (autoscrollLog) ImGui.SetScrollHereY();
                    ImGui.EndChild();
                    ImGui.Columns(1);
                }
            }
            catch(Exception e)
            {
                p.Log("Minor error: " + e.Message);
            }
        }
    }
}
