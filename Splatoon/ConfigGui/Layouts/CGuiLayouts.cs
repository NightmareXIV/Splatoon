using Dalamud.Interface.Colors;
using Dalamud.Interface.Internal.Notifications;
using ImGuiNET;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Splatoon.ConfigGui.CGuiLayouts.LayoutDrawSelector;

namespace Splatoon
{
    partial class CGui
    {
        internal static string layoutFilter = "";
        string PopupRename = "";
        //internal static string CurrentGroup = null;
        internal static string HighlightGroup = null;
        internal static HashSet<string> OpenedGroup = new();
        internal static string NewLayoytName = "";
        internal static Layout ScrollTo = null;
        void DislayLayouts()
        {
            {
                var deleted = P.Config.LayoutsL.RemoveAll(x => x.Delete);
                if (deleted > 0)
                {
                    Notify.Info($"Removed {deleted} layouts");
                    if (!P.Config.LayoutsL.Contains(CurrentLayout))
                    {
                        CurrentLayout = null;
                        CurrentElement = null;
                    }
                }
            }
            ImGui.BeginChild("TableWrapper", ImGui.GetContentRegionAvail(), false, ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse);
            if (ImGui.BeginTable("LayoutsTable", 2, ImGuiTableFlags.Resizable))
            {
                ImGui.TableSetupColumn("Layout list###Layout id", ImGuiTableColumnFlags.None, 200);
                ImGui.TableSetupColumn($"{(CurrentLayout == null ? "" : $"{CurrentLayout.GetName()}") + (CurrentElement == null ? "" : $" | {CurrentElement.GetName()}")}###Layout edit", ImGuiTableColumnFlags.None, 600);

                ImGui.TableHeadersRow();

                ImGui.TableNextColumn();
                ImGuiEx.InputWithRightButtonsArea("Search layouts", delegate
                {
                    ImGui.InputTextWithHint("##layoutFilter", "Search layouts...", ref layoutFilter, 100);
                }, delegate
                {
                    if (ImGuiEx.IconButton(FontAwesomeIcon.Plus))
                    {
                        ImGui.OpenPopup("Add layout");
                    }
                    ImGuiEx.Tooltip("Add new layout...");
                    ImGui.SameLine(0, 1);
                    if (ImGuiEx.IconButton(FontAwesomeIcon.FileImport))
                    {
                        if(Static.TryImportLayout(ImGui.GetClipboardText(), out var l))
                        {
                            CurrentLayout = l;
                            if(l.Group != "")
                            {
                                OpenedGroup.Add(l.Group);
                            }
                        }
                    }
                    ImGuiEx.Tooltip("Import from clipboard");
                    ImGui.SameLine(0, 1);
                    if(ImGuiEx.IconButton(P.Config.FocusMode? FontAwesomeIcon.SearchMinus: FontAwesomeIcon.SearchPlus))
                    {
                        P.Config.FocusMode = !P.Config.FocusMode;
                    }
                    ImGuiEx.Tooltip("Toggle focus mode.\nFocus mode: when layout is selected, hide all other layouts.");
                });
                if(ImGui.BeginPopup("Add layout"))
                {
                    ImGui.InputTextWithHint("", "Layout name", ref NewLayoytName, 100);
                    ImGui.SameLine();
                    if (ImGui.Button("Add"))
                    {
                        if (CGui.AddEmptyLayout(out var newLayout))
                        {
                            ImGui.CloseCurrentPopup();
                            Notify.Success($"Layout created: {newLayout.GetName()}");
                            ScrollTo = newLayout;
                            CurrentLayout = newLayout;
                        }
                    }
                    ImGui.EndPopup();
                }
                ImGui.BeginChild("LayoutsTableSelector");
                foreach (var x in P.Config.LayoutsL)
                {
                    var deleted = x.ElementsL.RemoveAll(k => k.Delete);
                    if(deleted > 0)
                    {
                        Notify.Info($"Deleted {deleted} elements");
                        if(!P.Config.LayoutsL.Any(l => l.ElementsL.Contains(CurrentElement)))
                        {
                            CurrentElement = null;
                        }
                    }
                    if (x.Group == null) x.Group = "";
                    if(x.Group != "" && !P.Config.GroupOrder.Contains(x.Group))
                    {
                        P.Config.GroupOrder.Add(x.Group);
                    }
                }
                P.Config.GroupOrder.RemoveAll(x => x.IsNullOrEmpty());
                Layout[] takenLayouts = P.Config.LayoutsL.ToArray();
                var groupToRemove = -1;
                if (!P.Config.FocusMode || CurrentLayout == null)
                {
                    for (var i = 0; i < P.Config.GroupOrder.Count; i++)
                    {
                        var g = P.Config.GroupOrder[i];
                        if (layoutFilter != "" &&
                            !P.Config.LayoutsL.Any(x => x.Group == g && x.GetName().Contains(layoutFilter, StringComparison.OrdinalIgnoreCase))) continue;

                        ImGui.PushID(g);
                        ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.DalamudYellow);

                        if (HighlightGroup == g)
                        {
                            ImGui.PushStyleColor(ImGuiCol.Header, ImGuiColors.DalamudYellow with { W = 0.5f });
                            ImGui.PushStyleColor(ImGuiCol.HeaderActive, ImGuiColors.DalamudYellow with { W = 0.5f });
                            ImGui.PushStyleColor(ImGuiCol.HeaderHovered, ImGuiColors.DalamudYellow with { W = 0.5f });
                        }
                        var curpos = ImGui.GetCursorScreenPos();
                        var contRegion = ImGui.GetContentRegionAvail().X;
                        if (ImGui.Selectable($"[{g}]", HighlightGroup == g))
                        {
                            if (!OpenedGroup.Toggle(g))
                            {
                                if (CurrentLayout?.Group == g)
                                {
                                    CurrentLayout = null;
                                    CurrentElement = null;
                                }
                            }
                        }
                        if (HighlightGroup == g)
                        {
                            ImGui.PopStyleColor(3);
                            HighlightGroup = null;
                        }
                        ImGui.PopStyleColor();
                        if (ImGui.BeginDragDropSource())
                        {
                            ImGuiDragDrop.SetDragDropPayload("MoveGroup", i);
                            ImGuiEx.Text($"Moving group\n[{g}]");
                            ImGui.EndDragDropSource();
                        }
                        if (ImGui.BeginDragDropTarget())
                        {
                            if (ImGuiDragDrop.AcceptDragDropPayload("MoveLayout", out int indexOfMovedObj
                                , ImGuiDragDropFlags.AcceptNoDrawDefaultRect | ImGuiDragDropFlags.AcceptBeforeDelivery))
                            {
                                HighlightGroup = g;
                                if (ImGui.IsMouseReleased(ImGuiMouseButton.Left))
                                {
                                    P.Config.LayoutsL[indexOfMovedObj].Group = g;
                                }
                            }
                            if (ImGuiDragDrop.AcceptDragDropPayload("MoveGroup", out int indexOfMovedGroup
                                , ImGuiDragDropFlags.AcceptNoDrawDefaultRect | ImGuiDragDropFlags.AcceptBeforeDelivery))
                            {
                                SImGuiEx.DrawLine(curpos, contRegion);
                                if (ImGui.IsMouseReleased(ImGuiMouseButton.Left))
                                {
                                    var exch = P.Config.GroupOrder[indexOfMovedGroup];
                                    P.Config.GroupOrder[indexOfMovedGroup] = null;
                                    P.Config.GroupOrder.Insert(i, exch);
                                    P.Config.GroupOrder.RemoveAll(x => x == null);
                                }
                            }
                            ImGui.EndDragDropTarget();
                        }
                        if (ImGui.IsItemClicked(ImGuiMouseButton.Right))
                        {
                            ImGui.OpenPopup("GroupPopup");
                        }
                        if (ImGui.BeginPopup("GroupPopup"))
                        {
                            ImGuiEx.Text($"[{g}]");
                            ImGui.SetNextItemWidth(200f);
                            var result = ImGui.InputTextWithHint("##GroupRename", "Enter new name...", ref PopupRename, 100, ImGuiInputTextFlags.EnterReturnsTrue);
                            PopupRename = PopupRename.SanitizeName();
                            ImGui.SameLine();
                            if (ImGui.Button("OK") || result)
                            {
                                if (P.Config.GroupOrder.Contains(PopupRename))
                                {
                                    Notify.Error("Error: this name is already exists");
                                }
                                else if (PopupRename.Length == 0)
                                {
                                    Notify.Error("Error: empty names are not allowed");
                                }
                                else
                                {
                                    if (OpenedGroup.Contains(g))
                                    {
                                        OpenedGroup.Add(PopupRename);
                                        OpenedGroup.Remove(g);
                                    }
                                    foreach (var x in P.Config.LayoutsL)
                                    {
                                        if (x.Group == g)
                                        {
                                            x.Group = PopupRename;
                                        }
                                    }
                                    P.Config.GroupOrder[i] = PopupRename;
                                    PopupRename = "";
                                }
                            }
                            if (ImGui.Selectable("Remove group and disband layouts") && ImGui.GetIO().KeyCtrl)
                            {
                                foreach (var l in P.Config.LayoutsL)
                                {
                                    if (l.Group == g)
                                    {
                                        l.Group = "";
                                    }
                                }
                                groupToRemove = i;
                            }
                            ImGuiEx.Tooltip("Hold CTRL+click");
                            if (ImGui.Selectable("Remove group and it's layouts") && ImGui.GetIO().KeyCtrl && ImGui.GetIO().KeyShift)
                            {
                                foreach (var l in P.Config.LayoutsL)
                                {
                                    if (l.Group == g)
                                    {
                                        l.Group = "";
                                        l.Delete = true;
                                    }
                                }
                                groupToRemove = i;
                            }
                            ImGuiEx.Tooltip("Hold CTRL+SHIFT+click");
                            ImGui.EndPopup();
                        }
                        for (var n = 0; n < takenLayouts.Length; n++)
                        {
                            var x = takenLayouts[n];
                            if (x != null && (x.Group == g))
                            {
                                if (OpenedGroup.Contains(g) || layoutFilter != "")
                                {
                                    x.DrawSelector(g, n);
                                }
                                takenLayouts[n] = null;
                            }
                        }
                        ImGui.PopID();
                    }
                }
                for (var i = 0; i < takenLayouts.Length; i++)
                {
                    var x = takenLayouts[i];
                    if (!P.Config.FocusMode || CurrentLayout == x || CurrentLayout == null)
                    {
                        if (x != null)
                        {
                            x.DrawSelector(null, i);
                        }
                    }
                }
                if(groupToRemove != -1)
                {
                    P.Config.GroupOrder.RemoveAt(groupToRemove);
                }
                ImGui.EndChild();

                ImGui.TableNextColumn();

                ImGui.BeginChild("LayoutsTableEdit", ImGui.GetContentRegionAvail(), false, ImGuiWindowFlags.HorizontalScrollbar);
                if(CurrentLayout != null)
                {
                    if(CurrentElement != null && CurrentLayout.ElementsL.Contains(CurrentElement))
                    {
                        LayoutDrawElement(CurrentLayout, CurrentElement);
                    }
                    else
                    {
                        LayoutDrawHeader(CurrentLayout);
                    }
                }
                else
                {
                    ImGuiEx.Text("UI Help:");
                    ImGuiEx.Text("- Left panel contains groups, layouts and elements.");
                    ImGuiEx.Text("- You can drag and drop layouts, elements and groups to reorder them.");
                    ImGuiEx.Text("- Right click on a group to rename or delete it.");
                    ImGuiEx.Text("- Right click on a layout/element to delete it.");
                    ImGuiEx.Text("- Middle click on layout/element for quick enable/disable");
                }
                ImGui.EndChild();

                ImGui.EndTable();
            }
            ImGui.EndChild();
            /*if (curEdit == -1)
            {
                ImGui.SetNextItemWidth(ImGui.GetContentRegionAvail().X);
                ImGui.InputTextWithHint("##layoutFilter", "Filter...", ref layoutFilter, 1000);
            }
            else
            {
                ImGui.Checkbox("Allow layout deletion", ref enableDeletion);
                ImGui.SameLine();
                ImGui.Checkbox("Allow elements deletion", ref enableDeletionElement);
            }


            if (curEdit == -1)
            {
                ImGui.Separator();
                ImGui.SetNextItemWidth(ImGui.GetContentRegionAvail().X - 3f * ImGui.GetStyle().ItemSpacing.X - GetPresetWidth);
                ImGui.InputTextWithHint("##lname", "Unique layout name", ref lname, 100);
                lname.Trim();
                ImGui.SameLine();
                if (ImGui.Button("Add layout"))
                {
                    if (p.Config.LayoutsL.Any(x => x.Name == lname))
                    {
                        p.Log("Error: this name already exists", true);
                    }
                    else if (lname.Length == 0)
                    {
                        p.Log("Error: you must name layout", true);
                    }
                    else if (lname.Contains("~"))
                    {
                        p.Log("Name can't contain reserved characters: ~", true);
                    }
                    else if (lname.Contains(","))
                    {
                        p.Log("Name can't contain reserved characters: ,", true);
                    }
                    else
                    {
                        var l = new Layout();
                        if (Svc.ClientState != null) l.ZoneLockH.Add(Svc.ClientState.TerritoryType);
                        p.Config.AddLegacyLayout(lname, l);
                        lname = "";
                    }
                }
                GetPresetWidth = ImGui.GetItemRectSize().X;
                ImGui.SameLine();
                if (ImGui.Button("Import from clipboard"))
                {
                    Safe(delegate { ImportFromText(ImGui.GetClipboardText()); });
                }
                GetPresetWidth += ImGui.GetItemRectSize().X;
                ImGui.SameLine();
                if (ImGui.Button("Get presets from github"))
                {
                    ProcessStart("https://github.com/Eternita-S/Splatoon/tree/master/Presets");
                }
                GetPresetWidth += ImGui.GetItemRectSize().X;
            }

            ImGui.BeginChild("##layoutlist");
            var open = false;
            for (var i = 0;i < p.Config.LayoutsL.Count; i++)
            {
                var layout = p.Config.LayoutsL[i];
                if (layoutFilter.Length == 0 || layout.Name.Contains(layoutFilter, StringComparison.OrdinalIgnoreCase))
                {
                    var colored = false;
                    if (!layout.Enabled)
                    {
                        colored = true;
                        ImGui.PushStyleColor(ImGuiCol.Text, Colors.Gray);
                    }
                    else if (layout.DisableDisabling)
                    {
                        colored = true;
                        ImGui.PushStyleColor(ImGuiCol.Text, Colors.Orange);
                    }
                    if (curEdit == i)
                    {
                        ImGui.PushStyleColor(ImGuiCol.Text, Colors.Green);
                        SImGuiEx.TextCentered("Editing layout: " + curEdit);
                        ImGui.PopStyleColor();
                    }
                    if ((curEdit == -1 || curEdit == i) && ImGui.CollapsingHeader(layout.Name))
                    {
                        if (colored)
                        {
                            ImGui.PopStyleColor();
                            colored = false;
                        }
                        open = true;
                        curEdit = i;
                        if (enableDeletion)
                        {
                            ImGui.PushStyleColor(ImGuiCol.Button, Colors.Red);
                            if (ImGui.Button("Delete##dltlt" + i))
                            {
                                //p.Config.Layouts.Remove(i);
                                enableDeletion = false;
                            }
                            ImGui.PopStyleColor();
                            ImGui.SameLine();
                        }
                        try
                        {
                            LayoutDrawHeader(layout);
                            foreach (var k in layout.ElementsL)
                            {
                                LayoutDrawElement(layout, k);
                            }
                        }
                        catch (Exception e)
                        {
                            ImGuiEx.Text("Error");
                            PluginLog.Error($"Error: {e.Message}\n{e.StackTrace}");
                        }
                    }
                    if (colored)
                    {
                        ImGui.PopStyleColor();
                        colored = false;
                    }
                }
            }
            if (!open) curEdit = -1;
            ImGui.EndChild();*/
        }
    }
}
