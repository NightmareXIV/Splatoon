using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Splatoon
{
    class MemerrGui
    {
        readonly Splatoon p;
        bool open = true;
        bool understood = false;
        public MemerrGui(Splatoon p)
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
            ImGui.Begin("Splatoon is running in failsafe mode", ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.AlwaysAutoResize);
            ImGui.TextColored(Colors.Red.ToVector4(), "Certain functions will perform differently or will be unavailable until plugin update.");
            ImGui.TextUnformatted(
@"Splatoon uses some native functions and fields to be able to work properly.
Due to game update some of them have failed to resolve. Normally in a situation like that you should disable plugin until it's updated by developer.
However, Splatoon was build for raiding, and often raiding does not accepts any waiting. Therefore a failsafe mode was introduced.
Instead of completely failing, Splatoon has disabled some functionality to keep it's basic functions working.
The following functions will work differently:
- When camera angle is down to the floor, you may see duplicate waymarks;
- Very large circles may have line duplication issue;
- All characters are considered visible since plugin can not check if they actually are;
- Lines require a lot more processing time than normally.");
            ImGui.TextColored(Colors.Red.ToVector4(), "This message will appear every time you are starting the game until plugin will be updated.");
            //ImGui.Checkbox("I have read and undrestood this message", ref understood);
            if (ImGui.Button("I have read and undrestood this message. Close this window."))
            {
                open = false;
            }
            ImGui.End();
            if (!open) Close();
        }

        void Close()
        {
            this.Dispose();
        }
    }
}
