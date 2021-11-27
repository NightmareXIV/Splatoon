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
        public const int ChlogVersion = 22;
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
@"This is quite a big update which brings few features.
- Support for filtering highlighted objects only to visible characters, even if they become untargetable.
- Support for setting radus for lines relative to object position only if rotation has been enabled. 
   Two changes above allow to do fun stuff like this: ");
            if(ImGui.SmallButton("Click to open image"))
            {
                ProcessStart("https://github.com/Eternita-S/Splatoon/blob/master/docs/images/mechanic_marker.jpg");
            }
            ImGui.TextUnformatted(
@"   Both are beta features and may be not very stable for now. Please do actively report any bugs and inconsistencies.
- Added an option to keep Splatoon's UI shown even when you hide UI with scroll lock and have Dalamud's setting to hide plugins UI too.
- Small UI adjustments to make window more compact, more of that will be coming as well.
- Code cleanup and optimizations.

A backup of your current configuration will be made upon closing this window.
Additionally, configuration conversion will be performed. 
Should you encounter any problems - you can always restore configuration from automatic backup.");
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
            p.Config.ChlogReadVer = ChlogVersion;
            var i = 0;
            foreach(var l in p.Config.Layouts)
            {
                foreach(var el in l.Value.Elements.Values)
                {
                    if ((el.type == 2 || el.type == 3) && el.radius == 0.35f)
                    {
                        el.radius = 0;
                        i++;
                    }
                }
            }
            Svc.PluginInterface.UiBuilder.AddNotification($"Configuration converted, {i} elements changed", "Splatoon", NotificationType.Info);
            p.Config.Save();
            this.Dispose();
        }
    }
}
