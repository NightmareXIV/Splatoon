using Dalamud.Interface.Colors;
using Dalamud.Interface.Internal.Notifications;
using Newtonsoft.Json;
using Splatoon.ConfigGui.CGuiLayouts.LayoutDrawHeader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Splatoon;

partial class CGui
{
    float ShareWidth = 0;

    void CopyToCb(string i)
    {
        //ImGui.SetClipboardText(i + "~" + JsonConvert.SerializeObject(p.Config.Layouts[i], Formatting.None, new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Ignore }));
        //Notify.Success("Copied to clipboard");
    }

    void LayoutDrawHeader(Layout layout)
    {
        if(ImGui.BeginTable("SingleLayoutEdit", 2, ImGuiTableFlags.NoSavedSettings | ImGuiTableFlags.SizingFixedFit | ImGuiTableFlags.BordersInnerH))
        {
            ImGui.TableSetupColumn("##LayoutEdit1");
            ImGui.TableSetupColumn("##LayoutEdit2", ImGuiTableColumnFlags.WidthStretch);

            //ImGui.TableHeadersRow();

            ImGui.TableNextColumn();
            ImGui.Checkbox("Enabled", ref layout.Enabled);
            ImGui.TableNextColumn();
            ImGui.SetNextItemWidth(150f.Scale());
            if (ImGui.BeginCombo("##phaseSelectorL", $"{(layout.Phase == 0 ? "Any phase" : $"Phase {layout.Phase}")}"))
            {
                if (ImGui.Selectable("Any phase")) layout.Phase = 0;
                if (ImGui.Selectable("Phase 1 (doorboss)")) layout.Phase = 1;
                if (ImGui.Selectable("Phase 2 (post-doorboss)")) layout.Phase = 2;
                ImGuiEx.Text("Manual phase selection:");
                ImGui.SameLine();
                ImGui.SetNextItemWidth(30f.Scale());
                ImGui.DragInt("##mPSel", ref layout.Phase, 0.1f, 0, 9);
                ImGui.EndCombo();
            }
            ImGui.SameLine();
            ImGui.Checkbox("Disable in duty", ref layout.DisableInDuty);

            ImGui.TableNextColumn();
            ImGuiEx.TextV("Display conditions:");
            ImGui.TableNextColumn();
            ImGuiEx.SetNextItemFullWidth();
            ImGui.Combo("##dcn", ref layout.DCond, Layout.DisplayConditions, Layout.DisplayConditions.Length);

            ImGui.TableNextColumn();
            ImGuiEx.TextV("Zone lock: ");
            ImGui.TableNextColumn();
            layout.DrawZlockSelector();

            ImGui.TableNextColumn();
            ImGuiEx.TextV("Job lock");
            ImGui.TableNextColumn();
            layout.DrawJlockSelector();

            ImGui.TableNextColumn();
            ImGui.Checkbox("Distance limit", ref layout.UseDistanceLimit);
            ImGui.TableNextColumn();
            layout.DrawDistanceLimit();

            ImGui.EndTable();
        }


        var i = layout.Name;
        var topCursorPos = ImGui.GetCursorPos();
        //var layout = p.Config.Layouts[i];
        
        ImGui.SameLine();
        
        
        ImGui.SetCursorPos(new Vector2(ImGui.GetColumnWidth() - ShareWidth - ImGui.GetStyle().ItemSpacing.X, topCursorPos.Y));
        ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.DalamudOrange);
        if(ImGui.Button("Share layout"))
        {
            CopyToCb(i);
            ProcessStart("https://github.com/Eternita-S/Splatoon/tree/master/Presets#adding-your-preset");
        }
        ImGui.PopStyleColor();
        ShareWidth = ImGui.GetItemRectSize().X;
        if (ImGui.IsItemHovered()) ImGui.SetTooltip("Send your preset to other users of Splatoon to use!");
        ImGui.SameLine();
        ImGui.SetNextItemWidth(100f);
        if (ImGui.BeginCombo("##copy" + i, "Copy..."))
        {
            if (ImGui.Selectable("Export to clipboard"))
            {
                CopyToCb(i); 
            }
            if (ImGui.Selectable("Copy enable command"))
            {
                ImGui.SetClipboardText("/splatoon enable " + i);
                Notify.Success("Copied to clipboard");
            }
            if (ImGui.Selectable("Copy disable command"))
            {
                ImGui.SetClipboardText("/splatoon disable " + i);
                Notify.Success("Copied to clipboard");
            }
            if (ImGui.Selectable("Copy as HTTP param##" + i))
            {
                HTTPExportToClipboard(layout);
                Notify.Success("Copied to clipboard");
            }
            if (ImGui.IsItemHovered())
            {
                ImGui.SetTooltip("Hold ALT to copy raw JSON (for usage with post body or you'll have to urlencode it yourself)\nHold CTRL and click to copy urlencoded raw");
            }
            ImGui.EndCombo();
        }
        ShareWidth += ImGui.GetItemRectSize().X;
              
        

        
        

        if (layout.UseTriggers)
        {
            ImGui.Checkbox("##usetrigger" + i, ref layout.UseTriggers);
            ImGui.SameLine();
            ImGui.PushStyleColor(ImGuiCol.Header, Colors.Transparent);
            if (ImGui.CollapsingHeader("Trigger settings##" + i))
            {
                if (ImGui.Button("Add new trigger##" + i))
                {
                    layout.Triggers.Add(new Trigger());
                }
                var deleteTrigger = -1;
                for (var n = 0; n < layout.Triggers.Count; n++)
                {
                    if (ImGui.Button("[X]##" + n + i) && ImGui.GetIO().KeyCtrl)
                    {
                        deleteTrigger = n;
                    }
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("Hold CTRL + left click to delete");
                    }
                    ImGui.SameLine();
                    ImGui.SetNextItemWidth(WidthCombo);
                    ImGui.Combo("##trigger" + i + n, ref layout.Triggers[n].Type, Trigger.Types, Trigger.Types.Length);
                    ImGui.SameLine();
                    ImGuiEx.Text("Reset on:");
                    ImGui.SameLine();
                    ImGui.Checkbox("Combat exit##" + i + n, ref layout.Triggers[n].ResetOnCombatExit);
                    ImGui.SameLine();
                    ImGui.Checkbox("Territory change##" + i + n, ref layout.Triggers[n].ResetOnTChange);
                    ImGui.SameLine();
                    ImGuiEx.Text("State: " + layout.Triggers[n].FiredState);
                    if (layout.Triggers[n].Type == 0 || layout.Triggers[n].Type == 1)
                    {
                        ImGuiEx.Text("Time: ");
                        ImGui.SameLine();
                        ImGui.SetNextItemWidth(50f);
                        ImGui.DragFloat("##triggertime1" + i + n, ref layout.Triggers[n].TimeBegin, 0.1f, 0, 3599, "%.1f");
                        ImGui.SameLine();
                        ImGuiEx.Text(DateTimeOffset.FromUnixTimeMilliseconds((long)(layout.Triggers[n].TimeBegin * 1000)).ToString("mm:ss.f"));
                    }
                    else
                    {
                        ImGui.SetNextItemWidth(300f);
                        layout.Triggers[n].MatchIntl.ImGuiEdit(ref layout.Triggers[n].Match, "Case-insensitive (partial) message");
                        //ImGui.InputTextWithHint("##textinput1" + n + i, "Case-insensitive message", ref layout.Triggers[n].Match, 1000);

                        ImGui.SameLine(); 
                        ImGuiEx.Text("Delay: ");
                        ImGui.SameLine(); 
                        ImGui.SetNextItemWidth(50f);
                        ImGui.DragFloat("##triggertime1" + i + n, ref layout.Triggers[n].MatchDelay, 0.1f, 0, 3599, "%.1f");
                        ImGui.SameLine();
                        ImGuiEx.Text(DateTimeOffset.FromUnixTimeMilliseconds((long)(layout.Triggers[n].MatchDelay*1000)).ToString("mm:ss.f"));
                        layout.Triggers[n].Match = layout.Triggers[n].Match.RemoveSymbols(Splatoon.InvalidSymbols);
                    }
                    ImGui.SameLine();
                    ImGuiEx.Text("Duration: ");
                    ImGui.SameLine();
                    ImGui.SetNextItemWidth(50f);
                    ImGui.DragFloat("##triggertime2" + i + n, ref layout.Triggers[n].Duration, 0.1f, 0, 3599, "%.1f");
                    ImGui.SameLine();
                    ImGuiEx.Text(layout.Triggers[n].Duration == 0 ? "Infinite" : DateTimeOffset.FromUnixTimeMilliseconds((long)(layout.Triggers[n].Duration * 1000)).ToString("mm:ss.f"));
                    ImGui.Separator();
                }
                if (deleteTrigger != -1)
                {
                    try
                    {
                        layout.Triggers.RemoveAt(deleteTrigger);
                    }
                    catch (Exception e)
                    {
                        p.Log(e.Message + "\n" + e.StackTrace);
                    }
                }
                ImGui.PopStyleColor(1);
            }
            else
            {
                ImGui.PopStyleColor(1);
            }
        }
        else
        {
            ImGui.Checkbox("Use trigger system##usetrigger" + i, ref layout.UseTriggers);
        }

        ImGui.TextColored(ImGui.ColorConvertU32ToFloat4(Colors.Green), "Add elements to the layout to create markers:");
        ImGui.SameLine();
        ImGui.PushItemWidth(WidthCombo);
        ImGui.InputTextWithHint("##elnameadd" + i, "Unique element name", ref ename, 100);
        ImGui.PopItemWidth();
        ImGui.SameLine();
        if (ImGui.Button("Add element##addelement" + i))
        {
            if (layout.ElementsL.Any(x => x.Name == ename))
            {
                p.Log("Error: this name already exists", true);
            }
            else if (ename.Length == 0)
            {
                p.Log("Error: you must name element", true);
            }
            else
            {
                var el = new Element(0);
                el.refX = GetPlayerPositionXZY().X;
                el.refY = GetPlayerPositionXZY().Y;
                el.refZ = GetPlayerPositionXZY().Z;
                layout.AddLegacyElement(ename, el);
                ename = "";
            }
        }
        ImGui.SameLine();
        if (ImGui.Button("Paste from clipboard##addelement" + i))
        {
            if (layout.ElementsL.Any(x => x.Name == ename))
            {
                p.Log("Error: this name already exists", true);
            }
            else if (ename.Length == 0)
            {
                p.Log("Error: you must name element", true);
            }
            else
            {
                try
                {
                    var el = JsonConvert.DeserializeObject<Element>(ImGui.GetClipboardText());
                    layout.AddLegacyElement(ename, el);
                    ename = "";
                }
                catch(Exception e)
                {
                    LogErrorAndNotify(e);
                }
            }
        }
    }
}
