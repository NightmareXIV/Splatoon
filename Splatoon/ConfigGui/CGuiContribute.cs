using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splatoon
{
    partial class CGui
    {
        void DisplayContribute()
        {
            ImGui.PushTextWrapPos();
            ImGui.Text("If you like Splatoon, you may consider contributing in any following way:");
            ImGui.Separator();
            ImGui.Text("- Sending your own presets to public");
            ImGui.Text("Did Splatoon helped you to clear a raid, to resolve a mechanic, to improve your gameplay in any way? Please consider submitting your preset to the public so others may enjoy it as well!");
            ImGui.Text("You may send it to Github if you have account or to my Discord server.");
            if(ImGui.Button("Open Github page"))
            {
                var url = "https://github.com/Eternita-S/Splatoon/tree/master/Presets#adding-your-preset";
                Svc.Chat.Print("[Splatoon] How to submit your preset: "+url);
                ProcessStart(url);
            }
            ImGui.SameLine();
            if (ImGui.Button("Open Discord server"))
            {
                Svc.Chat.Print("[Splatoon] Server invite link: " + Splatoon.DiscordURL);
                ProcessStart(Splatoon.DiscordURL);
            }
            ImGui.Separator();
            ImGui.Text("- Adding a star to the repo");
            ImGui.Text("Don't have any presets to send? You may still help by simply adding a star to Splatoon and my plugins' repo!");
            ImGui.Text("To do so, all you need is Github account. After logging in, proceed to the links below and click \"Star\" button in top right corner of the page.");
            if (ImGui.Button("Open Splatoon repo"))
            {
                var url = "https://github.com/Eternita-S/Splatoon";
                Svc.Chat.Print("[Splatoon] Splatoon repo: " + url);
                ProcessStart(url);
            }
            ImGui.SameLine();
            if (ImGui.Button("Open Eternita's plugins repo"))
            {
                var url = "https://github.com/Eternita-S/MyDalamudPlugins";
                Svc.Chat.Print("[Splatoon] Eternita's plugin repo: " + url);
                ProcessStart(url);
            }
            ImGui.Separator();
            ImGui.Text("- Financial");
            ImGui.Text("Should you like my work and have a coin to spare, I will be happy to accept it. Please note that work on the plugin will continue regardless of donations; I do not require them.");
            ImGui.Text("All donations will be spent on the game and will ensure that I can pay for it regardless of my personal financial situation.");
            ImGui.Text("You may send tokens to any of the following crypto wallets: (click on the button to copy address)");
            ECommons.ImGuiMethods.Donation.PrintDonationInfo();
            ImGui.Separator();
            ImGui.Text("Thank you for your contributions!");
            ImGui.PopTextWrapPos();
        }
    }
}
