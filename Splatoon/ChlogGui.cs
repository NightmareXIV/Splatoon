using Dalamud.Game.ClientState.Objects.Enums;
using Dalamud.Interface.Internal.Notifications;
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
        public const int ChlogVersion = 29;
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
@"A section in plugin's Github repo with some presets is now available. You can share your as well.");
            if (ImGui.Button("Open a page to get and share presets on Github"))
            {
                ProcessStart("https://github.com/Eternita-S/Splatoon/tree/master/Presets");
            }
            ImGui.TextUnformatted(@"You can find some initial presets I uploaded and you also can send your own.
More convenient interface for sharing presets right from the plugin will be added later.
- Additionally, an error has been fixed related to a backup system in case if config for Splatoon does not exists.
-------------------------------------------------------------------------------------------------
Previous update: 
This update removes unreadable symbols from messages before using them as a trigger source.
All your triggers will be updated and unreadable symbols will be removed from them upon closing this window.

A backup of your current configuration will be made upon closing this window.");
            if (ImGui.Button("Close this window"))
            {
                open = false;
            }
            ImGui.End();
            if (!open) Close();
        }

        void Close()
        {
            p.Config.Backup();
            var i = 0;
            foreach (var l in p.Config.Layouts)
            {
                foreach (var t in l.Value.Triggers)
                {
                    t.Match = t.Match.RemoveSymbols(Splatoon.InvalidSymbols);
                    i++;
                }
            }
            Notify($"Processed {i} triggers", NotificationType.Success);
            p.Config.ChlogReadVer = ChlogVersion;
            p.Config.Save();
            this.Dispose();
        }
    }
}
