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
        public const int ChlogVersion = 17;
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
@"Added:
- Object type: line relative to object position with an option to account for rotation.
   Example usage: displaying zone for bard's aoe; displaying enemy cone aoe attack area;
      displaying line aoe (e11s)
- Circle relative to object position may now be rotation relative as well.
- Please test plugin well; there is a good chance I've screwed something up while adding all this cursed math.
=========================================
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
