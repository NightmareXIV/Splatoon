using Dalamud.Interface.Colors;
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
            if(ImGui.Button("Temporarily load from clipboard"))
            {
                ScriptingProcessor.CompileAndLoad(ImGui.GetClipboardText());
            }
            var del = -1;
            for(var i = 0;i<ScriptingProcessor.Scripts.Count;i++)
            {
                var x = ScriptingProcessor.Scripts[i];
                ImGui.PushID(x.InternalData.GUID);
                var e = !P.Config.DisabledScripts.Contains(x.InternalData.FullName);
                if(ImGui.Checkbox($"##enable", ref e))
                {
                    if (e)
                    {
                        P.Config.DisabledScripts.Add(x.InternalData.FullName);
                        x.Disable();
                    }
                    else
                    {
                        P.Config.DisabledScripts.Remove(x.InternalData.FullName);
                        x.Enable();
                    }
                }
                ImGuiEx.Text($"{x.InternalData.Namespace}/{x.InternalData.Name}");
                ImGui.SameLine();
                ImGuiEx.Text(x.IsEnabled?ImGuiColors.ParsedGreen:ImGuiColors.DalamudRed, x.IsEnabled ? "Enabled" : "Disabled");
                ImGui.SameLine();
                if (ImGui.Button("Unload"))
                {
                    del = i;
                }
                ImGui.SameLine();
                if (ImGui.Button("Delete"))
                {
                    del = i;
                }
                ImGui.PopID();
            }
            if(del != -1)
            {
                ScriptingProcessor.Scripts[del].Disable();
                ScriptingProcessor.Scripts.RemoveAt(del);
            }
        }
    }
}
