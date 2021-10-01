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
        public const int ChlogVersion = 15;
        readonly Splatoon p;
        bool open = true;
        bool understood = false;
        public ChlogGui(Splatoon p)
        {
            this.p = p;
            Svc.PluginInterface.UiBuilder.Draw += Draw;
        }

        public void Dispose()
        {
            Svc.PluginInterface.UiBuilder.Draw -= Draw;
        }

        void Draw()
        {
            if (!open) return;
            if (!Svc.ClientState.IsLoggedIn) return;
            ImGui.Begin("Splatoon has been updated", ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.AlwaysAutoResize);
            ImGui.TextUnformatted(
@"Changes in this version:
- Added support for importing waymark presets from Waymark preset plugin library and Paisley Park
- Added tether function to the elements. It will draw line from you to visible marker.
- Added profiling functions
- Added object name cache. Object table lookups by name became about 4 times faster.
- [alpha] Added simple trigger system. Allows you to specify when exactly your layout will appear in battle.
   It will be kept very simple, for more complicated triggers please use Triggernometry/Cactbot + web api.
- Added a button to easily copy visual style between elements.
- A backup of your configuration is now saved any time you open configuration. 
   In addition, your configuration is autosaved every 5-15 seconds to ensure your changes are always safe.
----------------------------------
This is the API 4 version.
There may be major bugs.
Please report any suspicious behavior.
If your existing markers stopped working, DO NOT try fix them yourself.
Instead, report about it to me and I will fix them instead.
A backup of your current configuration will be created when you close this window.");
            if(ImGui.Button("Close this window"))
            {
                open = false;
            }
            ImGui.End();
            if (!open) Close();
        }

        void Close()
        {
            p.Config.Backup();
            p.Config.ChlogReadVer = ChlogVersion;
            p.Config.Save();
            this.Dispose();
        }
    }
}
