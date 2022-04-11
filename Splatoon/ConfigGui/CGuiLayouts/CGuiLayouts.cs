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
        void DislayLayouts()
        {
            /*if(p.CamAngleY > p.Config.maxcamY)
                    {
                        ImGui.PushStyleColor(ImGuiCol.Text, Colors.Red);
                        ImGuiEx.TextCentered("Your camera settings prevent waymarks from displaying in your current camera position.");
                        ImGuiEx.TextCentered("Either lift your camera up or adjust camera settings in general settings below.");
                        ImGui.PopStyleColor();
                    }*/
            if (curEdit == null)
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


            if (curEdit == null)
            {
                ImGui.Separator();
                ImGui.SetNextItemWidth(ImGui.GetContentRegionAvail().X - 3f * ImGui.GetStyle().ItemSpacing.X - GetPresetWidth);
                ImGui.InputTextWithHint("##lname", "Unique layout name", ref lname, 100);
                lname.Trim();
                ImGui.SameLine();
                if (ImGui.Button("Add layout"))
                {
                    if (p.Config.Layouts.ContainsKey(lname))
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
                        p.Config.Layouts.Add(lname, l);
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
            foreach (var i in p.Config.Layouts.Keys.ToArray())
            {
                if (layoutFilter.Length == 0 || i.Contains(layoutFilter, StringComparison.OrdinalIgnoreCase))
                {
                    var colored = false;
                    if (!p.Config.Layouts[i].Enabled)
                    {
                        colored = true;
                        ImGui.PushStyleColor(ImGuiCol.Text, Colors.Gray);
                    }
                    else if (p.Config.Layouts[i].DisableDisabling)
                    {
                        colored = true;
                        ImGui.PushStyleColor(ImGuiCol.Text, Colors.Orange);
                    }
                    if (curEdit == i)
                    {
                        ImGui.PushStyleColor(ImGuiCol.Text, Colors.Green);
                        ImGuiEx.TextCentered("Editing layout: " + curEdit);
                        ImGui.PopStyleColor();
                    }
                    if ((curEdit == null || curEdit == i) && ImGui.CollapsingHeader(i))
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
                                p.Config.Layouts.Remove(i);
                                enableDeletion = false;
                            }
                            ImGui.PopStyleColor();
                            ImGui.SameLine();
                        }
                        if (p.Config.Layouts.ContainsKey(i))
                        {
                            try
                            {
                                LayoutDrawHeader(i);
                                foreach (var k in p.Config.Layouts[i].Elements.Keys.ToArray())
                                {
                                    LayoutDrawElement(i, k);
                                }
                            }
                            catch(Exception e)
                            {
                                ImGui.Text("Error");
                                PluginLog.Error($"Error: {e.Message}\n{e.StackTrace}");
                            }
                        }
                    }
                    if (colored)
                    {
                        ImGui.PopStyleColor();
                        colored = false;
                    }
                }
            }
            if (!open) curEdit = null;
            ImGui.EndChild();
        }
    }
}
