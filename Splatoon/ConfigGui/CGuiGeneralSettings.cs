﻿using Dalamud.Interface.Colors;
using Dalamud.Interface.Components;
using Splatoon.Utils;

namespace Splatoon
{
    partial class CGui
    {
        void DisplayGeneralSettings()
        {
            ImGuiEx.Text("Game version: ");
            ImGui.SameLine(0, 0);
            ImGuiEx.TextCopy(p.loader.gVersion);
            SImGuiEx.SizedText("Use web API", WidthLayout);
            ImGui.SameLine();
            if (ImGui.Checkbox("##usewebapi", ref p.Config.UseHttpServer))
            {
                p.SetupShutdownHttp(p.Config.UseHttpServer);
            }
            ImGui.SameLine();
            if (p.Config.UseHttpServer)
            {
                ImGuiEx.Text("http://127.0.0.1:" + p.Config.port + "/");
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
                ImGuiEx.Text("Port: ");
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
            ImGuiEx.Text("(?)");
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("Higher - smoother circle, higher cpu usage");

            SImGuiEx.SizedText("Drawing distance:", WidthLayout);
            ImGui.SameLine();
            ImGui.SetNextItemWidth(100f);
            ImGui.DragFloat("##maxdistance", ref p.Config.maxdistance, 0.25f, 10f, 200f);
            ImGui.SameLine();
            ImGuiEx.Text("(?)");
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("Only try to draw objects that are not \n" +
                    "further away from you than this value");

            SImGuiEx.SizedText("Line segments:", WidthLayout);
            ImGui.SameLine();
            ImGui.SetNextItemWidth(100f);
            ImGui.DragInt("##linesegments", ref p.Config.lineSegments, 0.1f, 10, 50);
            p.Config.lineSegments.ValidateRange(10, 100);
            ImGuiComponents.HelpMarker("Increase this if your lines stop drawing too far from the screen edges or if line disappears when " +
                "you are zoomed in and near it's edge. Increasing this setting hurts performance EXTRAORDINARILY.");
            if(p.Config.lineSegments > 10)
            {
                ImGuiEx.TextWrapped(ImGuiColors.DalamudOrange, "Non-standard line segment setting. Performance of your game may be impacted. " +
                    "Please CAREFULLY increase this setting until everything works as intended and do not increase it further. \n" +
                    "Consider increasing minimal rectangle fill line thickness to mitigate performance loss, if you will experience it.");
            }
            if (p.Config.lineSegments > 25)
            {
                ImGuiEx.TextWrapped(Environment.TickCount % 1000 > 500 ? ImGuiColors.DalamudRed : ImGuiColors.DalamudYellow,
                    "Your line segment setting IS EXTREMELY HIGH AND MAY SIGNIFICANTLY IMPACT PERFORMANCE.\n" +
                    "If you really have to set it to this value to make it work, please contact developer and provide details.");
            }
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
            ImGui.Separator();
            ImGuiEx.Text("Fill settings:");
            ImGui.SameLine();
            ImGuiEx.Text("            Screwed up?");
            ImGui.SameLine();
            if(ImGui.SmallButton("Reset this section"))
            {
                var def = new Configuration();
                P.Config.AltConeStep = def.AltConeStep;
                P.Config.AltConeStepOverride = def.AltConeStepOverride;
                P.Config.AltDonutStep = def.AltDonutStep;
                P.Config.AltDonutStepOverride = def.AltDonutStepOverride;
                P.Config.AltRectFill = def.AltRectFill;
                P.Config.AltRectForceMinLineThickness = def.AltRectForceMinLineThickness;
                P.Config.AltRectHighlightOutline = def.AltRectHighlightOutline;
                P.Config.AltRectMinLineThickness = def.AltRectMinLineThickness;
                P.Config.AltRectStep = def.AltRectStep;
                P.Config.AltRectStepOverride = def.AltRectStepOverride;
            }
            ImGui.Checkbox("Use line rectangle filling", ref p.Config.AltRectFill);
            ImGuiComponents.HelpMarker("Fill rectangles with stroke instead of full color. This will remove clipping issues, but may feel more disturbing.");
            

            ImGui.SetNextItemWidth(60f);
            ImGui.DragFloat("Minimal rectangle fill line interval", ref p.Config.AltRectStep, 0.001f, 0, float.MaxValue);
            ImGui.SameLine();
            ImGui.Checkbox("Always force this value##1", ref P.Config.AltRectStepOverride);

            ImGui.SetNextItemWidth(60f);
            ImGui.DragFloat("Minimal rectangle fill line thickness", ref p.Config.AltRectMinLineThickness, 0.001f, 0.01f, float.MaxValue);
            ImGuiComponents.HelpMarker("Problems with performance while rectangles are visible? Increase this value.");
            ImGui.SameLine();
            ImGui.Checkbox("Always force this value##2", ref P.Config.AltRectForceMinLineThickness);
            ImGui.Checkbox("Additionally highlight rectangle outline", ref p.Config.AltRectHighlightOutline);

            ImGui.SetNextItemWidth(60f);
            ImGui.DragFloat("Minimal donut fill line interval", ref p.Config.AltDonutStep, 0.001f, 0.01f, float.MaxValue);
            ImGuiComponents.HelpMarker("Problems with performance while rectangles are visible? Increase this value.");
            ImGui.SameLine();
            ImGui.Checkbox("Always force this value##3", ref P.Config.AltDonutStepOverride);

            ImGui.SetNextItemWidth(60f);
            ImGui.DragInt("Minimal cone fill line interval", ref p.Config.AltConeStep, 0.1f, 1, int.MaxValue);
            ImGui.SameLine();
            ImGui.Checkbox("Always force this value##4", ref P.Config.AltConeStepOverride);

            ImGui.Separator();
            ImGui.Checkbox("Use hexadecimal numbers", ref p.Config.Hexadecimal);
            ImGui.Checkbox("Enable tether on Splatoon find command", ref p.Config.TetherOnFind);
            ImGui.Checkbox("Force show Splatoon's UI when game UI is hidden", ref p.Config.ShowOnUiHide);
            Svc.PluginInterface.UiBuilder.DisableUserUiHide = p.Config.ShowOnUiHide;
            //ImGui.Checkbox("Always compare names directly (debug option, ~4x performance loss)", ref p.Config.DirectNameComparison);
            if(ImGui.Button("Open backup directory"))
            {
                ProcessStart(Path.Combine(Svc.PluginInterface.GetPluginConfigDirectory(), "Backups"));
            }
            ImGui.Separator();
            ImGuiEx.Text("Contact developer:");
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
