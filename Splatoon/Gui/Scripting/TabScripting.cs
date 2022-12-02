using Dalamud.Interface.Colors;
using ECommons;
using ECommons.LanguageHelpers;
using Splatoon.SplatoonScripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splatoon.Gui.Scripting
{
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
            for(var i = 0;i<ScriptingProcessor.Scripts.Count;i++)
            {
                var x = ScriptingProcessor.Scripts[i];
                ImGui.PushID(x.InternalData.GUID);
                var e = !P.Config.DisabledScripts.Contains(x.InternalData.FullName);
                if(ImGui.Checkbox($"##enable", ref e))
                {
                    if (!e)
                    {
                        P.Config.DisabledScripts.Add(x.InternalData.FullName);
                    }
                    else
                    {
                        P.Config.DisabledScripts.Remove(x.InternalData.FullName);
                    }
                    ScriptingProcessor.Scripts.ForEach(x => x.UpdateState());
                }
                ImGui.SameLine();
                ImGuiEx.Text($"{x.InternalData.Namespace}@{x.InternalData.Name} v{x.Metadata?.Version ?? 0}");
                ImGuiEx.Tooltip($"{x.InternalData.GUID}");
                ImGui.SameLine();
                ImGuiEx.Text(x.IsEnabled?ImGuiColors.ParsedGreen:ImGuiColors.DalamudRed, x.IsEnabled ? "Enabled" : "Disabled");
                if (x.InternalData.SettingsPresent)
                {
                    ImGui.SameLine();
                    if (ImGui.Button("Config".Loc()))
                    {
                        x.InternalData.ConfigOpen = !x.InternalData.ConfigOpen;
                    }
                }
                ImGui.SameLine();
                if (ImGui.Button("Delete".Loc()) && ImGui.GetIO().KeyCtrl)
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
                ImGuiEx.Tooltip("Hold CTRL + click".Loc());
                if (x.InternalData.ConfigOpen)
                {
                    try
                    {
                        x.OnSettingsDraw();
                    }
                    catch (Exception ex)
                    {
                        ex.Log();
                    }
                }
                ImGui.PopID();
                ImGui.Separator();
            }
            if(del != -1)
            {
                ScriptingProcessor.Scripts[del].Disable();
                ScriptingProcessor.Scripts.RemoveAt(del);
            }
        }
    }
}
