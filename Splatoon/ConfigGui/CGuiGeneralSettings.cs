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
            ImGuiEx.SizedText("Use web API", WidthLayout);
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
                        try
                        {
                            Process.Start(new ProcessStartInfo()
                            {
                                UseShellExecute = true,
                                FileName = "http://127.0.0.1:" + p.Config.port + "/"
                            });
                        }
                        catch(Exception e)
                        {
                            Svc.Chat.Print("Error: " + e.Message + "\n" + e.StackTrace);
                        }
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
                Process.Start("https://github.com/Eternita-S/Splatoon#web-api-beta");
            }

            ImGuiEx.SizedText("Circle smoothness:", WidthLayout);
            ImGui.SameLine();
            ImGui.SetNextItemWidth(100f);
            ImGui.DragInt("##circlesmoothness", ref p.Config.segments, 0.1f, 10, 150);
            ImGui.SameLine();
            ImGui.Text("(?)");
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("Higher - smoother circle, higher cpu usage");

            ImGuiEx.SizedText("Drawing distance:", WidthLayout);
            ImGui.SameLine();
            ImGui.SetNextItemWidth(100f);
            ImGui.DragFloat("##maxdistance", ref p.Config.maxdistance, 0.25f, 10f, 200f);
            ImGui.SameLine();
            ImGui.Text("(?)");
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("Only try to draw objects that are not \n" +
                    "further away from you than this value");

            ImGuiEx.SizedText("Line segments:", WidthLayout);
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
            ImGui.Text("Contact developer:");
            ImGui.SameLine();
            if (ImGui.Button("Github"))
            {
                Process.Start("https://github.com/Eternita-S/Splatoon/issues");
            }
            ImGui.SameLine();
            if (ImGui.Button("Discord"))
            {
                ImGui.SetClipboardText("Eternita#4037");
                Svc.Chat.Print("[Splatoon] Contact me in discord: Eternita#4037. Discord ID copied into your clipboard.");
            }
        }
    }
}
