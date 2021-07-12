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
        public const int ChlogVersion = 2;
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
            ImGui.Text("Changes since last few versions:\n" +
                "- Added changelog window. You will see it only once per update.\n" + 
                "- Screen2World feature allows you to place waymarks using your\n" +
                "mouse without having to input coordinates manually\n" +
                "- Removed option to hide markers when camera angle is too low.\n" +
                "Originally it was a dirty hack that was needed to prevent\n" +
                "displaying objects that are behind you.\n" +
                "However, a proper fix has been implemented to resolve that problem.\n" +
                "If you somehow will end up having waymarks displaying when they should not be,\n" +
                "please let me know about it. \n" +
                "- Added contacts into configuration window.\n" +
                "- Plugin is now fully compatible with LivePluginLoader\n" +
                "- Implemented backup system. Every time you close configuration window,\n" +
                "a backup of your configuration will be made in plugin's configuration folder.\n" +
                "For now you will have to restore it manually if anything goes wrong.");
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
