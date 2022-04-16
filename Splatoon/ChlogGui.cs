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
        public const int ChlogVersion = 39;
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
@"- Fixed detection of character visibility (hopefully for good this time).
   IMPORTANT NOTE: there may be characters present in the field that are visible, have same name as boss, but have transparent model.
   You can find example in Emanation.
   If you have encountered situation like this, it is very advised to use MODEL ID instead of name.
   With object logger finding out model ID should be very easy.

- Added more information into object logger.

- Reduced amount of unnecessary information being serialized.

- Any manipulations with configuration will be strictly prevented until user closes changelog window now.

- Backup made after significant update will be marked to make it easier to distinguish it.

      ***      Plugin updates will be on hold for 1-2 months after ultimate release      ***
No new major features will be added during that time, only bugfixes.
If you are planning to use Splatoon in ultimate, I strongly suggest you to test all it's functions now
and create bugreport on Github if something is not working or if you think there could be some key feature
added that will significantly help in ultimate progression.

And of course, thanks to everyone who contributed and plans to contribute their presets - your contributions are invaluable.");
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
