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
        public const int ChlogVersion = 12;
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
                "- Web api. You can enable it in settings as well as find the link to the guide.\n" +
                "   It allows you to control Splatoon externally from Triggernometry, Cactbot\n" +
                "   and any other program that supports sending HTTP requests.\n" +
                "   Please note that web API is in beta and requires a lot of testing ideally.\n" +
                "- Minor fixes and gui improvements.");
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
