using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Splatoon
{
    class ChlogGui
    {
        public const int ChlogVersion = 6;
        readonly Splatoon p;
        bool open = true;
        public ChlogGui(Splatoon p)
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
            if (!open) return;
            if (!p._pi.ClientState.IsLoggedIn) return;
            ImGui.Begin("Splatoon has been updated", ref open, ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.AlwaysAutoResize);
            ImGui.Text("Changes in this version:\n" +
                "Introducing player/NPC searching!\n" +
                "Use the command:\n" +
                "/sf Partial Case-insensitive name\n" +
                "to temporarily highlight NPCs and players\n" +
                "whose name contains specified substring.\n" +
                "Use the command /sf without arguments\n" +
                "to remove highlighting.");
            if(ImGui.Button("Close this window"))
            {
                open = false;
            }
            ImGui.End();
            if (!open) Close();
        }

        void Close()
        {
            p.Config.ChlogReadVer = ChlogVersion;
            p.Config.Save();
            this.Dispose();
        }
    }
}
