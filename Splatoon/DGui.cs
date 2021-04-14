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
        float w2sx, w2sy, w2sz, w2srx, w2sry;
        bool w2sb = false;
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
                    ImGui.Checkbox("Cause AV on update", ref p.AccessViolation);
                    ImGui.Checkbox("Cause AV on draw", ref p.DrawingGui.AccessViolation);
                    ImGui.Separator();
                    if (p._pi.ClientState.LocalPlayer != null)
                    {
                        var mypos = p._pi.ClientState.LocalPlayer.Position;
                        ImGui.Text("My pos: " + mypos.X + ", " + mypos.Y + ", " + mypos.Z);
                        var tar = p._pi.ClientState.Targets.CurrentTarget;
                        if (tar != null)
                        {
                            ImGui.Text("Target pos: " + tar.Position.X + ", " + tar.Position.Y + ", " + tar.Position.Z);
                        }
                    }
                    ImGui.Separator();
                    ImGui.Text("Query WorldToScreen");
                    ImGui.PushItemWidth(50f);
                    ImGui.DragFloat("##qw2sx", ref w2sx);
                    ImGui.SameLine();
                    ImGui.DragFloat("##qw2sy", ref w2sy);
                    ImGui.SameLine();
                    ImGui.DragFloat("##qw2sz", ref w2sz);
                    ImGui.PopItemWidth();
                    if (ImGui.Button("To my pos"))
                    {
                        var mypos = p._pi.ClientState.LocalPlayer.Position;
                        w2sx = mypos.X;
                        w2sy = mypos.Y;
                        w2sz = mypos.Z;
                    }
                    ImGui.SameLine();
                    if (ImGui.Button("Query"))
                    {
                        w2sb = p._pi.Framework.Gui.WorldToScreen(new SharpDX.Vector3(w2sx, w2sz, w2sy), out SharpDX.Vector2 pos);
                        w2srx = pos.X;
                        w2sry = pos.Y;
                    }
                    ImGui.TextColored(ImGui.ColorConvertU32ToFloat4(w2sb?Colors.Green:Colors.Red), "X:"+w2srx+"\nY:"+ w2sry);
                    ImGui.Separator();
                    ImGui.Text("Actors:");
                    foreach (var a in p._pi.ClientState.Actors)
                    {
                        if (a is PlayerCharacter || a is BattleNpc)
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
