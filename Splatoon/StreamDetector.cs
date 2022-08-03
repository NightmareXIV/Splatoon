using Dalamud.Interface.Colors;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Splatoon
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
                while (!P.Disposed)
                {
                    if (!Svc.Condition[Dalamud.Game.ClientState.Conditions.ConditionFlag.InCombat])
                    {
                        var processes = Process.GetProcesses();
                        if (processes.Any(x => x.ProcessName.EqualsIgnoreCaseAny("obs32", "obs64")))
                        {
                            Svc.PluginInterface.UiBuilder.Draw += Draw;
                            break;
                        }
                    }
                    Thread.Sleep(10000);
                }
            }).Start();
        }

        static void Draw()
        {
            if(ImGui.Begin("Hold on!", ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoCollapse))
            {
                ImGui.SetWindowFontScale(2f);
                ImGuiEx.Text(Environment.TickCount % 1000 > 500? ImGuiColors.DalamudRed:ImGuiColors.DalamudYellow, "Please do not stream with third party tools visible.");
                ImGui.SetWindowFontScale(1f);
                ImGuiEx.Text("Normally, most of plugins are completely safe to use. Square Enix will not be able to detect their usage, including Splatoon.");
                ImGuiEx.Text(ImGuiColors.DalamudOrange, "However, streaming with third party tools visible may result in consequences.");
                ImGuiEx.Text("Regardless of how innocent a plugin or modification might be, modification is a modification and a violation of FFXIV's terms of service.");
                ImGuiEx.Text("This includes not only third party tools, but also official Dalamud plugins, Advanced Combat Tracker and visual mods as well.");
                ImGuiEx.Text(" ");
                ImGui.SetWindowFontScale(1.5f);
                ImGuiEx.Text(ImGuiColors.DalamudYellow, "If you intended to stream your game, absolutely make sure that your plugins\nand other third party tools are NOT VISIBLE ON STREAM.");
                ImGui.SetWindowFontScale(1f);
                ImGuiEx.Text("And it does not matters how many viewers you have - even just one is already enough.");
                if(ImGui.Button("I understand and will not stream with third party tools visible"))
                {
                    Svc.PluginInterface.UiBuilder.Draw -= Draw;
                }
                ImGuiEx.Text(ImGuiColors.DalamudGrey, "You are seeing this message because a streaming software has been detected.\nYou will not see it again in your current game session.\nIf you believe this is an error, please contact the developer.");
                if (ImGui.Button("Never show this message again"))
                {
                    P.Config.NoStreamWarning = true;
                    Svc.PluginInterface.UiBuilder.Draw -= Draw;
                }
                ImGuiEx.Tooltip("Be reasonable, okay?");
            }
            ImGui.End();
        }
    }
}
