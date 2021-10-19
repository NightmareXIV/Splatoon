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
        public const int ChlogVersion = 16;
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
@"This is mostly a maintenance update directed towards ensuring that the plugin keeps working in 6.0+.
- Failsafe mode has been designed. Should plugin's native memory module fail to initialize due to game's
   update, it will boot in a failsafe mode. While this means that some functions will be disabled,
   it also means that all core functions will remain functional immediately after Dalamud is available for
   new game's version (assuming Dalamud itself doesn't introduces any breaking changes) without needing
   to wait for me or someone else to find new signatures and update the plugin.
   Failsafe mode can be activated from settings manually as well if necessary and will always be automatically
   disabled upon plugin update.
- Additionally, an option to limit display distance per layout was implemented. It has 2 modes:
   - Limit by distance to current target
   - Limit by distance to displayed elements individually
   Please note that lines will not be affected by second mode.
- The plugin is nearing the finish of it's main development. Almost all features that I wanted to have in it are
   already implemented, which means it's a time to do a good testing and find all the bugs before Endwalker.
   Please report any bugs and inconsistencies you will find inside the plugin via github or discord.
- A backup of your current configuration will be created upon closing this window.");
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
