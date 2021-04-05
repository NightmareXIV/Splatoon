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
                foreach (var a in p._pi.ClientState.Actors)
                {
                    if (!(a is BattleNpc)) continue;
                    try
                    {
                        ImGui.Text(a.Name);
                        ImGui.SameLine();
                        ImGui.Text("");
                    }
                    catch (Exception e)
                    {
                        ImGui.Text(e.Message);
                    }
                }
            }
        }
    }
}
