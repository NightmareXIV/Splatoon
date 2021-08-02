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
        public const int ChlogVersion = 8;
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
                "- Object search (/sf) no longer highlights uninteractable objects\n" +
                "- Added an option to only display targetable (interactable) objects when using an option to highlight objects by name\n" +
                "- You may now use * in place of object name to highlight ALL objects at once (including /sf *)");
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
