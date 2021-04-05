using Dalamud.Game.ClientState.Actors.Types;
using Dalamud.Game.ClientState.Actors.Types.NonPlayer;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splatoon
{
    unsafe class DGui : IDisposable
    {
        private Splatoon p;
        public bool Open = false;
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
                ImGui.Text("CStarted: " + t);
                foreach (var a in p._pi.ClientState.Actors)
                {
                    if (a is PlayerCharacter)
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
                ImGui.BeginChild("##splatoondbg2");
                ImGui.EndChild();
                ImGui.Columns(1);
            }
        }
    }
}
