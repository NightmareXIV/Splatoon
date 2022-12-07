using Dalamud.Interface.Colors;
using Dalamud.Interface.Components;
using ECommons;
using ECommons.LanguageHelpers;
using Splatoon.SplatoonScripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Splatoon.Gui.Scripting;

internal static class TabScripting
{
    internal static void Draw()
    {
        if (ScriptingProcessor.ThreadIsRunning)
        {
            ImGuiEx.ImGuiLineCentered("ThreadCompilerRunning", delegate
            {
                ImGuiEx.Text(GradientColor.Get(ImGuiColors.DalamudWhite, ImGuiColors.ParsedPink), "Scripts are being installed, please wait...".Loc());
            });
        }
        ImGuiEx.TextWrapped(ImGuiColors.DPSRed, $"Warning: scripting function is under alpha testing. Changes may come to the scripting system at any moment. Any scripts you write now may require manual updates later. There is no guarantee yet that all currently available methods will be kept. There is no guarantee that scripts which have been made now will keep working through further updates.".Loc());
        ImGuiEx.TextWrapped(ImGuiColors.DalamudOrange, "Please note that scripts have direct and unrestricted access to your PC and game. Ensure that you know what you're installing.".Loc());
        if(ImGui.Button("Clear cache, rescan directory and reload all scripts".Loc()))
        {
            var dir = Path.Combine(Svc.PluginInterface.GetPluginConfigDirectory(), "ScriptCache");
            foreach(var x in Directory.GetFiles(dir))
            {
                if (x.EndsWith(".bin"))
                {
                    PluginLog.Information($"Deleting {x}");
                    File.Delete(x);
                }
            }
            ScriptingProcessor.ReloadAll();
        }
        ImGui.SameLine();
        if(ImGui.Button("Install from clipboard (code or trusted URL)".Loc()))
        {
            var text = ImGui.GetClipboardText();
            if (ScriptingProcessor.IsUrlTrusted(text))
            {
                ScriptingProcessor.DownloadScript(text);
            }
            else 
            {
                ScriptingProcessor.CompileAndLoad(text, null);
            }
        }
        var del = -1;
        ImGui.BeginTable("##scriptsTable", 3, ImGuiTableFlags.BordersInner | ImGuiTableFlags.BordersOuter | ImGuiTableFlags.RowBg | ImGuiTableFlags.SizingFixedFit);
        ImGui.TableSetupColumn("Name", ImGuiTableColumnFlags.WidthStretch);
        ImGui.TableSetupColumn("State");
        ImGui.TableSetupColumn("Controls");
        ImGui.TableHeadersRow();

        var openConfig = ScriptingProcessor.Scripts.FirstOrDefault(x => x.InternalData.ConfigOpen);

        for (var i = 0;i<ScriptingProcessor.Scripts.Count;i++)
        {
            var x = ScriptingProcessor.Scripts[i];
            if (openConfig != null && !ReferenceEquals(x, openConfig)) continue;
            ImGui.TableNextRow();
            ImGui.TableNextColumn();
            ImGui.PushID(x.InternalData.GUID);
            ImGuiEx.TextV($"{x.InternalData.Name.Replace("_", " ")}");
            if(x.Metadata?.Description == null)
            {
                ImGuiEx.Tooltip($"{x.InternalData.Namespace}");
            }
            else
            {
                ImGuiEx.Tooltip($"{x.InternalData.Namespace}\n{x.Metadata.Description}");
            }
            if(ImGui.IsItemHovered() && ImGui.IsMouseDoubleClicked(ImGuiMouseButton.Left))
            {
                ImGui.SetClipboardText($"{x.InternalData.FullName}");
                Notify.Success("Copied to clipboard");
            }
            if (x.Metadata?.Version != null)
            {
                ImGui.SameLine();
                ImGuiEx.Text(ImGuiColors.DalamudGrey2, $"v{x.Metadata.Version}");
            }
            if (x.Metadata?.Author != null)
            {
                ImGui.SameLine();
                ImGuiEx.Text(ImGuiColors.DalamudGrey2, $"by {x.Metadata.Author}");
            }

            ImGui.TableNextColumn();

            if (x.IsDisabledByUser)
            {
                ImGuiEx.TextV(ImGuiColors.DalamudRed, "Disabled".Loc());
                ImGuiComponents.HelpMarker("This script has been disabled by you.".Loc());
            }
            else if (!x.InternalData.Allowed)
            {
                ImGuiEx.TextV(ImGuiColors.ParsedGold, "Preparing".Loc());
                ImGuiComponents.HelpMarker("This script is being prepared for enabling and will be available shortly.".Loc());
            }
            else if (x.InternalData.Blacklisted)
            {
                ImGuiEx.TextV(ImGuiColors.DalamudGrey3, "Blacklisted".Loc());
                ImGuiComponents.HelpMarker("This script was blacklisted due to compatibility issues. Please wait for it's new version to be released.".Loc());
            }
            else if (x.IsEnabled)
            {
                ImGuiEx.TextV(ImGuiColors.ParsedGreen, "Active".Loc());
                ImGuiComponents.HelpMarker("This script is currently active and being executed.".Loc());
            }
            else
            {
                ImGuiEx.TextV(ImGuiColors.DalamudYellow, "Inactive".Loc());
                ImGuiComponents.HelpMarker("This script is currently inactive because you're not in a zone for which it was designed.".Loc());
            }
            ImGui.TableNextColumn();

            var e = P.Config.DisabledScripts.Contains(x.InternalData.FullName);
            if (ImGuiEx.IconButton(e?FontAwesomeIcon.PlayCircle : FontAwesomeIcon.PauseCircle))
            {
                if (e)
                {
                    P.Config.DisabledScripts.Remove(x.InternalData.FullName);
                }
                else
                {
                    P.Config.DisabledScripts.Add(x.InternalData.FullName);
                }
                ScriptingProcessor.Scripts.ForEach(x => x.UpdateState());
            }
            ImGuiEx.Tooltip(e?"Enable script".Loc() : "Disable script".Loc());

            ImGui.SameLine();

            if (x.InternalData.SettingsPresent)
            {
                if (ImGuiEx.IconButton(FontAwesomeIcon.Cog))
                {
                    if (x.InternalData.ConfigOpen)
                    {
                        openConfig.Controller.SaveConfig();
                    }
                    x.InternalData.ConfigOpen = !x.InternalData.ConfigOpen;
                }
                ImGuiEx.Tooltip("Open script's settings".Loc());
            }

            ImGui.SameLine();

            if (ImGuiEx.IconButton(FontAwesomeIcon.Trash) && ImGui.GetIO().KeyCtrl)
            {
                if (!x.InternalData.Path.IsNullOrEmpty() && x.InternalData.Path.EndsWith(".cs", StringComparison.OrdinalIgnoreCase))
                {
                    del = i;
                    DeleteFileToRecycleBin(x.InternalData.Path);
                }
                else
                {
                    Notify.Error("Error deleting".Loc());
                }
            }
            ImGuiEx.Tooltip("Delete script. Hold CTRL + click".Loc());
            ImGui.PopID();
        }
        if(del != -1)
        {
            ScriptingProcessor.Scripts[del].Disable();
            ScriptingProcessor.Scripts.RemoveAt(del);
        }
        ImGui.EndTable();

        if (openConfig != null)
        {
            ImGuiEx.ImGuiLineCentered("ScriptConfigTitle", delegate
            {
                ImGuiEx.Text(ImGuiColors.DalamudYellow, $"{openConfig.InternalData.FullName} configuration");
            });
            ImGui.Separator();
            try
            {
                openConfig.OnSettingsDraw();
            }
            catch (Exception ex)
            {
                ex.Log();
            }
            ImGuiEx.ImGuiLineCentered("ScriptConfig", delegate
            {
                if (ImGui.Button("Close and save configuration"))
                {
                    openConfig.InternalData.ConfigOpen = false;
                    openConfig.Controller.SaveConfig();
                }
            });
        }
    }
}
