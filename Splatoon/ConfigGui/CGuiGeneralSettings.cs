using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Splatoon
{
    partial class CGui
    {
        void DisplayGeneralSettings()
        {
            SImGuiEx.SizedText("Use web API", WidthLayout);
            ImGui.SameLine();
            if (ImGui.Checkbox("##usewebapi", ref p.Config.UseHttpServer))
            {
                p.SetupShutdownHttp(p.Config.UseHttpServer);
            }
            ImGui.SameLine();
            if (p.Config.UseHttpServer)
            {
                ImGui.TextUnformatted("http://127.0.0.1:" + p.Config.port + "/");
                if (ImGui.IsItemHovered())
                {
                    ImGui.SetMouseCursor(ImGuiMouseCursor.Hand);
                    if (ImGui.IsMouseReleased(ImGuiMouseButton.Left) && ImGui.GetMouseDragDelta(ImGuiMouseButton.Left) == Vector2.Zero)
                    {
                        ProcessStart("http://127.0.0.1:" + p.Config.port + "/");
                    }
                }
            }
            else
            {
                ImGui.TextUnformatted("Port: ");
                ImGui.SameLine();
                ImGui.SetNextItemWidth(100f);
                ImGui.DragInt("##webapiport", ref p.Config.port, float.Epsilon, 1, 65535);
                if (ImGui.IsItemHovered())
                {
                    ImGui.SetTooltip("Please only change if you have really good reason");
                }
                if (p.Config.port < 1 || p.Config.port > 65535) p.Config.port = 47774;
                ImGui.SameLine();
                ImGui.SetNextItemWidth(100f);
                if (ImGui.Button("Default"))
                {
                    p.Config.port = 47774;
                }
            }
            ImGui.SameLine();
            ImGui.SetNextItemWidth(250f);
            if (ImGui.Button("Open web API guide"))
            {
                ProcessStart("https://github.com/Eternita-S/Splatoon#web-api-beta");
            }

            SImGuiEx.SizedText("Circle smoothness:", WidthLayout);
            ImGui.SameLine();
            ImGui.SetNextItemWidth(100f);
            ImGui.DragInt("##circlesmoothness", ref p.Config.segments, 0.1f, 10, 150);
            ImGui.SameLine();
            ImGui.Text("(?)");
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("Higher - smoother circle, higher cpu usage");

            SImGuiEx.SizedText("Drawing distance:", WidthLayout);
            ImGui.SameLine();
            ImGui.SetNextItemWidth(100f);
            ImGui.DragFloat("##maxdistance", ref p.Config.maxdistance, 0.25f, 10f, 200f);
            ImGui.SameLine();
            ImGui.Text("(?)");
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("Only try to draw objects that are not \n" +
                    "further away from you than this value");

            SImGuiEx.SizedText("Line segments:", WidthLayout);
            ImGui.SameLine();
            ImGui.SetNextItemWidth(100f);
            ImGui.DragInt("##linesegments", ref p.Config.lineSegments, 0.1f, 10, 50);
            ImGui.SameLine();
            ImGui.Text("(?)");
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("Increase this if your lines stop drawing too far from the screen edges \n" +
                    "   or if line disappears when you are zoomed in and near it's edge. \n" +
                    "Increasing this setting SIGNIFICANTLY increases processing time.");

            /*ImGuiEx.SizedText("Draw only when Y camera rotation is lower than:", WidthLayout * 2);
            ImGui.SameLine();
            ImGui.SetNextItemWidth(150f);
            ImGui.DragFloat("##camymax", ref p.Config.maxcamY, 0.005f, -1.48353f, 0.78540f, p.Config.maxcamY.ToString("0.#####"));
            ImGui.SameLine();
            if (ImGui.Button("Current: " + p.CamAngleY))
            {
                p.Config.maxcamY = p.CamAngleY;
            }
            ImGui.SameLine();
            if (ImGui.Button("Default"))
            {
                p.Config.maxcamY = 0.05f;
            }*/
            ImGui.Checkbox("Enable tether on Splatoon find command", ref p.Config.TetherOnFind);
            ImGui.Checkbox("Limit trigger message types to boss replies and system messages only", ref p.Config.LimitTriggerMessages);
            ImGui.Checkbox("Force show Splatoon's UI when game UI is hidden", ref p.Config.ShowOnUiHide);
            Svc.PluginInterface.UiBuilder.DisableUserUiHide = p.Config.ShowOnUiHide;
            ImGui.Checkbox("Force failsafe mode (requires plugin restart)", ref p.Config.NoMemory);
            //ImGui.Checkbox("Always compare names directly (debug option, ~4x performance loss)", ref p.Config.DirectNameComparison);
            if(ImGui.Button("Open backup directory"))
            {
                ProcessStart(Path.Combine(Svc.PluginInterface.GetPluginConfigDirectory(), "Backups"));
            }
            ImGui.Separator();
            ImGui.Text("Contact developer:");
            ImGui.SameLine();
            if (ImGui.Button("Github"))
            {
                ProcessStart("https://github.com/Eternita-S/Splatoon/issues");
            }
            ImGui.SameLine();
            if (ImGui.Button("Discord"))
            {
                ImGui.SetClipboardText(Splatoon.DiscordURL);
                Svc.Chat.Print("[Splatoon] Server invite link: "+ Splatoon.DiscordURL);
                ProcessStart(Splatoon.DiscordURL);
            }
        }
    }
}
