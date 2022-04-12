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
        public const int ChlogVersion = 34;
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
@"- Support for new display object: rectangle (beta-testing).
   Please note that filled rectangles can behave weirdly or completely disappear
   when being partially outside of the screen.
   Same is for lines with radius.
   This is a bug and it can not be resolved at this time.

- Added support for new object search types: model id, object id and data id, in addition to name. 
- Added object logger which will log all objects that your client have seen to
   help you to figure out model/object/data IDs.
- Reduced dependency on direct memory signatures, allowing plugin to break with less chance on updates.");
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
            p.Config.Save();
            this.Dispose();
        }
    }
}
