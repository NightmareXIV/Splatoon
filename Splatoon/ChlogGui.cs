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
@"- Support for radius for lines between 2 fixed position.
   This essentially allows you to draw a rectangle.

- An attempt to increase stability and reliability of lines with radius
   Please keep in mind, however, that it will still be subject to distortion when crossing screen corners.
   A bug exists that makes creating filled objects very difficult and until it is resolved or bypassed further developments
   related to filled objects are almost impossible.

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
