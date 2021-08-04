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
        public const int ChlogVersion = 9;
        readonly Splatoon p;
        bool open = true;
        public ChlogGui(Splatoon p)
        {
            this.p = p;
            p.pi.UiBuilder.OnBuildUi += Draw;
        }

        public void Dispose()
        {
            p.pi.UiBuilder.OnBuildUi -= Draw;
        }

        void Draw()
        {
            if (!open) return;
            if (!p.pi.ClientState.IsLoggedIn) return;
            ImGui.Begin("Splatoon has been updated", ref open, ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.AlwaysAutoResize);
            ImGui.TextUnformatted("Changes in this version:\n" +
                "- Weird lines behavior should be fixed now.\n" +
                "- In addition, an option has been added to allow for further fix of very long lines, \n" +
                "   which is disabled by default because it has side effects.\n" +
                "- Lines fix still needs some testing.");
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
