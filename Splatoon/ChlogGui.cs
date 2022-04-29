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
        public const int ChlogVersion = 42;
        readonly Splatoon p;
        bool open = true;
        internal bool openLoggedOut = false;
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
            if (!Svc.ClientState.IsLoggedIn && !openLoggedOut) return;
            ImGui.Begin("Splatoon has been updated", ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.AlwaysAutoResize);
            ImGui.TextUnformatted(
@"- Added phase detection support. For now it is ENGLISH CLIENT ONLY. 
   Unfortunately I can not add support for other languages as I don't have people who would be willing to do any raid with phase transition for testing purposes with me. Sad face.
   People who play other languages can switch phases manually: open up /splatoon and switch phase in top right corner.
   If you have crashed/logged out/reloaded plugin during battle, you will have to select correct phase manually. Splatoon can only detect transition itself.
   If you will send me ""The limit gauge resets!"" message in other languages, I will add phase detection support into other languages as well.
   A layout now can be restricted to be active in certain phase only.

- All times in triggers now support floating point values. Your triggers should be converted automatically into new format but please double-check them.
   A backup of your configuration, as always, will be made upon closing this window to ensure you didn't lost anything in any case.

- Added possibility to delay message trigger by certain amount of time. ");
            if (ImGui.Button("Close this window"))
            {
                open = false;
            }
            ImGui.End();
            if (!open) Close();
        }

        void Close()
        {
            p.Config.Backup(true);
            p.Config.ChlogReadVer = ChlogVersion;
            p.Config.Save();
            this.Dispose();
        }
    }
}
