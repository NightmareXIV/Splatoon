using Dalamud.Interface.Internal.Notifications;
using ImGuiNET;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splatoon
{
    partial class CGui
    {
        float GetPresetWidth = 0;
        string layoutFilter = "";
        Layout CurrentLayout = null;
        Element CurrentElement = null;
        void DislayLayouts()
        {
            ImGui.BeginChild("TableWrapper", ImGui.GetContentRegionAvail(), false, ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse);
            if (ImGui.BeginTable("LayoutsTable", 2, ImGuiTableFlags.Resizable | ImGuiTableFlags.NoSavedSettings))
            {
                ImGui.TableSetupColumn("Layout list###Layout id", ImGuiTableColumnFlags.None, 200);
                ImGui.TableSetupColumn($"{(CurrentLayout == null ? "" : $"{CurrentLayout.Name}") + (CurrentElement == null ? "" : $" | {CurrentElement.Name}")}###Layout edit", ImGuiTableColumnFlags.None, 600);

                ImGui.TableHeadersRow();

                ImGui.TableNextColumn();

                ImGui.BeginChild("LayoutsTableSelector");
                foreach(var x in P.Config.LayoutsL)
                {
                    ImGui.PushID(x.GUID);
                    if (ImGui.Selectable($"{x.Name}", CurrentLayout == x))
                    {
                        if(CurrentLayout == x)
                        {
                            CurrentLayout = null;
                            CurrentElement = null;
                        }
                        else
                        {
                            CurrentLayout = x;
                        }
                    }
                    if(CurrentLayout == x)
                    {
                        foreach(var e in CurrentLayout.ElementsL)
                        {
                            ImGui.PushID(e.GUID);
                            ImGui.SetCursorPosX(20);
                            if (ImGui.Selectable($"{e.Name}", CurrentElement == e))
                            {
                                if(CurrentElement == e)
                                {
                                    CurrentElement = null;
                                }
                                else
                                {
                                    CurrentElement = e;
                                }
                            }
                            ImGui.PopID();
                        }
                    }
                    ImGui.PopID();
                }
                ImGui.EndChild();

                ImGui.TableNextColumn();

                ImGui.BeginChild("LayoutsTableEdit");
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
