using Dalamud.Interface.Colors;
using System.Diagnostics;
using System.Threading;

namespace Splatoon.Modules
{
    internal static class StreamDetector
    {
        static bool started = false;
        internal static void Start()
        {
            if (P.Config.NoStreamWarning) return;
            if (started) return;
            started = true;
            new Thread(() =>
            {
                try
                {
                    while (P != null && !P.Disposed)
                    {
                        if (!Svc.Condition[Dalamud.Game.ClientState.Conditions.ConditionFlag.InCombat])
                        {
                            var processes = Process.GetProcesses();
                            if (processes.Any(x => x.ProcessName.EqualsIgnoreCaseAny("obs32", "obs64") || x.ProcessName.StartsWithIgnoreCase("XSplit")))
                            {
                                Svc.PluginInterface.UiBuilder.Draw += Draw;
                                break;
                            }
                        }
                        Thread.Sleep(10000);
                    }
                }
                catch(Exception e)
                {
                    e.Log();
                }
            }).Start();
        }

        static void Draw()
        {
            if (ImGui.Begin("Splatoon - Hold on!", ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoCollapse))
            {
                ImGui.SetWindowFontScale(2f);
                ImGuiEx.Text(Environment.TickCount % 1000 > 500 ? ImGuiColors.DalamudRed : ImGuiColors.DalamudYellow, "Please do not stream with third party tools visible.");
                ImGui.SetWindowFontScale(1f);
                ImGuiEx.Text("Normally, most of plugins are completely safe to use. Square Enix will not be able to detect their usage.");
                ImGuiEx.Text(ImGuiColors.DalamudOrange, "However, streaming with third party tools visible may result in consequences.");
                ImGuiEx.Text("Regardless of how innocent a plugin or modification might be, it IS a violation of FFXIV's terms of service.");
                ImGuiEx.Text("This includes not only third party tools, but also official Dalamud plugins, Advanced Combat Tracker and visual mods as well.");
                ImGui.Separator();
                ImGui.SetWindowFontScale(1.5f);
                ImGuiEx.Text(ImGuiColors.DalamudYellow, "If you intended to stream your game, absolutely make sure that your plugins\nand other third party tools are NOT VISIBLE ON STREAM.");
                ImGui.SetWindowFontScale(1f);
                ImGuiEx.Text("And it does not matters how many viewers you have - even just one is already enough.");
                if (ImGui.Button("I understand and will not stream with third party tools visible"))
                {
                    Svc.PluginInterface.UiBuilder.Draw -= Draw;
                }
                ImGuiEx.Text(ImGuiColors.DalamudGrey, "You are seeing this message because a streaming software has been detected.\nYou will not see it again in your current game session.");
                ImGui.SameLine();
                if (ImGui.SmallButton("contact the developer."))
                {
                    ShellStart("https://discord.gg/m8NRt4X8Gf");
                }

                ImGui.Checkbox("Never show this message again", ref P.Config.NoStreamWarning);
            }
            ImGui.End();
        }
    }
}
