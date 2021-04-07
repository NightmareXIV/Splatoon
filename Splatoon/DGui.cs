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
            if(ImGui.Begin("Splatoon debug", ref Open))
            {
                ImGui.Columns(2);
                ImGui.BeginChild("##splatoondbg1");
                var t = DateTimeOffset.Now.ToUnixTimeSeconds() - p.CombatStarted;
                ImGui.Text("CombatStarted = " + t);
                ImGui.Checkbox("Access violation on update", ref p.AccessViolation);
                ImGui.Checkbox("Access violation on draw", ref p.DrawingGui.AccessViolation);
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
    }
}
