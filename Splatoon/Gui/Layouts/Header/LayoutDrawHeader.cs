using Newtonsoft.Json;
using Splatoon.ConfigGui;
using Splatoon.ConfigGui.CGuiLayouts.LayoutDrawHeader.Subcommands;
using Splatoon.Utils;

namespace Splatoon;

partial class CGui
{
    string NewGroupName = "";

    void LayoutDrawHeader(Layout layout)
    {
        if(ImGui.BeginTable("SingleLayoutEdit", 2, ImGuiTableFlags.NoSavedSettings | ImGuiTableFlags.BordersInnerH))
        {
            ImGui.TableSetupColumn("##LayoutEdit1", ImGuiTableColumnFlags.WidthFixed);
            ImGui.TableSetupColumn("##LayoutEdit2", ImGuiTableColumnFlags.WidthStretch);

            //ImGui.TableHeadersRow();
            ImGui.TableNextColumn();
            ImGuiEx.TextV("Group:");
            ImGui.TableNextColumn();
            ImGuiEx.SetNextItemFullWidth();
            if(ImGui.BeginCombo("##group", $"{(layout.Group == ""?"- No group -" : $"{layout.Group}")}"))
            {
                if (ImGui.Selectable("- No group -"))
                {
                    layout.Group = "";
                }
                foreach (var x in P.Config.GroupOrder)
                {
                    if (ImGui.Selectable(x))
                    {
                        layout.Group = x;
                    }
                }
                void Add()
                {
                    layout.Group = NewGroupName;
                    NewGroupName = "";
                    ImGui.CloseCurrentPopup();
                }
                ImGuiEx.InputWithRightButtonsArea("SelectGroup", delegate
                {
                    if(ImGui.InputTextWithHint("##NewGroupName", "New group...", ref NewGroupName, 100, ImGuiInputTextFlags.EnterReturnsTrue))
                    {
                        Add();
                    }
                    NewGroupName = NewGroupName.SanitizeName();
                }, delegate
                {
                    if (ImGui.Button("Add"))
                    {
                        Add();
                    }
                });
                ImGui.EndCombo();
            }


            ImGui.TableNextColumn();
            ImGuiEx.TextV("Export:");
            ImGui.TableNextColumn();
            if (ImGui.Button("Copy to clipboard"))
            {
                layout.ExportToClipboard();
            }
            ImGui.SameLine();
            ImGuiEx.TextV("Share:");
            ImGui.SameLine();
            if (ImGui.Button("GitHub"))
            {
                layout.ExportToClipboard();
                Contribute.OpenGithubPresetSubmit();
            }
            ImGui.SameLine(0, 1);
            if (ImGui.Button("Discord"))
            {
                layout.ExportToClipboard();
                Contribute.OpenDiscordLink();
            }
            ImGui.SameLine();
            if(ImGui.Button("Copy for Web API"))
            {
                HTTPExportToClipboard(layout);
            }
            ImGuiEx.Tooltip("Hold ALT to copy raw JSON (for usage with post body or you'll have to urlencode it yourself)\nHold CTRL and click to copy urlencoded raw");


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
            ImGuiEx.TextV("Name:");
            ImGui.TableNextColumn();
            ImGuiEx.SetNextItemFullWidth();
            if(ImGui.InputText("##name", ref layout.Name, 100))
            {
                layout.Name = layout.Name.SanitizeName();
            }

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
            ImGuiEx.Text("Zone blacklist: ");
            ImGui.TableNextColumn();
            layout.DrawZblacklistSelector();

            ImGui.TableNextColumn();
            ImGuiEx.TextV("Job lock");
            ImGui.TableNextColumn();
            layout.DrawJlockSelector();

            ImGui.TableNextColumn();
            ImGui.Checkbox("Distance limit", ref layout.UseDistanceLimit);
            ImGui.TableNextColumn();
            layout.DrawDistanceLimit();

            ImGui.TableNextColumn();
            ImGui.Checkbox("Freeze (beta)", ref layout.Freezing);
            ImGui.TableNextColumn();
            layout.DrawFreezing();

            ImGui.TableNextColumn();
            ImGui.Checkbox("Enable triggers", ref layout.UseTriggers);
            if (layout.UseTriggers)
            {
                if (ImGui.Button("Add new trigger"))
                {
                    layout.Triggers.Add(new Trigger());
                }
                if (ImGui.Button("Copy triggers"))
                {
                    ImGui.SetClipboardText(JsonConvert.SerializeObject(layout.Triggers));
                }
                if (ImGui.Button("Paste triggers") && (ImGui.GetIO().KeyCtrl || layout.Triggers.Count == 0))
                {
                    try
                    {
                        layout.Triggers = JsonConvert.DeserializeObject<List<Trigger>>(ImGui.GetClipboardText());
                    }
                    catch(Exception e)
                    {
                        Notify.Error(e.Message);
                    }
                }
                if(layout.Triggers.Count != 0)
                {
                    ImGuiEx.Tooltip("Hold CTRL and click. Existing triggers will be overwritten.");
                }
            }
            ImGui.TableNextColumn();
            layout.DrawTriggers();

            ImGui.EndTable();
        }


        var i = layout.Name;
        var topCursorPos = ImGui.GetCursorPos();
        //var layout = p.Config.Layouts[i];
        /*       
        ImGui.SetCursorPos(new Vector2(ImGui.GetColumnWidth() - ShareWidth - ImGui.GetStyle().ItemSpacing.X, topCursorPos.Y));
        ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.DalamudOrange);
        if(ImGui.Button("Share layout"))
        {
            CopyToCb(i);
            ProcessStart("https://github.com/NightmareXIV/Splatoon/tree/master/Presets#adding-your-preset");
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
        */
    }
}
