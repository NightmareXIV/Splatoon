using ECommons.LanguageHelpers;

namespace Splatoon.ConfigGui
{
    internal class Contribute
    {
        internal static void OpenGithubPresetSubmit()
        {
            var url = "https://github.com/NightmareXIV/Splatoon/tree/master/Presets#adding-your-preset";
            Svc.Chat.Print("[Splatoon] How to submit your preset: ".Loc() + url);
            ProcessStart(url);
        }

        internal static void OpenDiscordLink()
        {
            Svc.Chat.Print("[Splatoon] Server invite link: ".Loc() + Splatoon.DiscordURL);
            ProcessStart(Splatoon.DiscordURL);
        }

        internal static void Draw()
        {
            ImGui.PushTextWrapPos();
            ImGuiEx.Text("If you like Splatoon, you may consider contributing in any following way:".Loc());
            ImGui.Separator();
            ImGuiEx.Text("- Sending your own presets to public".Loc());
            ImGuiEx.Text("Did Splatoon helped you to clear a raid, to resolve a mechanic, to improve your gameplay in any way? Please consider submitting your preset to the public so others may enjoy it as well!".Loc());
            ImGuiEx.Text("You may send it to Github if you have account or to my Discord server.".Loc());
            if(ImGui.Button("Open Github page".Loc()))
            {
                OpenGithubPresetSubmit();
            }
            ImGui.SameLine();
            if (ImGui.Button("Open Discord server".Loc()))
            {
                OpenDiscordLink();
            }
            ImGui.Separator();
            ImGuiEx.Text("- Adding a star to the repo".Loc());
            ImGuiEx.Text("Don't have any presets to send? You may still help by simply adding a star to Splatoon and my plugins' repo!".Loc());
            ImGuiEx.Text("To do so, all you need is Github account. After logging in, proceed to the links below and click \"Star\" button in top right corner of the page.".Loc());
            if (ImGui.Button("Open Splatoon repo".Loc()))
            {
                var url = "https://github.com/NightmareXIV/Splatoon";
                Svc.Chat.Print("[Splatoon] Splatoon repo: ".Loc() + url);
                ProcessStart(url);
            }
            ImGui.SameLine();
            if (ImGui.Button("Open NightmareXIV plugins repo".Loc()))
            {
                var url = "https://github.com/NightmareXIV/MyDalamudPlugins";
                Svc.Chat.Print("[Splatoon] NightmareXIV plugin repo: ".Loc() + url);
                ProcessStart(url);
            }
            ImGui.Separator();
            ImGuiEx.Text("- Financial".Loc());
            ImGuiEx.Text("Should you like my work and have a coin to spare, I will be happy to accept it. Please note that work on the plugin will continue regardless of donations; I do not require them.".Loc());
            ImGuiEx.Text("All donations will be spent on the game and will ensure that I can pay for it regardless of my personal financial situation.".Loc());
            ImGuiEx.Text("You may send tokens to any of the following crypto wallets: (click on the button to copy address)".Loc());
            ECommons.ImGuiMethods.Donation.PrintDonationInfo();
            ImGui.Separator();
            ImGuiEx.Text("Thank you for your contributions!".Loc());
            ImGui.PopTextWrapPos();
        }
    }
}
