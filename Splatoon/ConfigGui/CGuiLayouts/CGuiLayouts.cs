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
        void DislayLayouts()
        {
            /*if(p.CamAngleY > p.Config.maxcamY)
                    {
                        ImGui.PushStyleColor(ImGuiCol.Text, Colors.Red);
                        ImGuiEx.TextCentered("Your camera settings prevent waymarks from displaying in your current camera position.");
                        ImGuiEx.TextCentered("Either lift your camera up or adjust camera settings in general settings below.");
                        ImGui.PopStyleColor();
                    }*/

            ImGui.Checkbox("Allow layout deletion", ref enableDeletion);
            ImGui.SameLine();
            ImGui.Checkbox("Allow elements deletion", ref enableDeletionElement);

            ImGui.Separator();

            ImGui.SetNextItemWidth(350f);
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
            ImGui.SameLine();
            ImGui.TextUnformatted("Import layout from:");
            ImGui.SameLine();
            if (ImGui.Button("clipboard"))
            {
                Safe(delegate { ImportFromText(ImGui.GetClipboardText()); });
            }
            

            ImGui.BeginChild("##layoutlist");
            var open = false;
            foreach (var i in p.Config.Layouts.Keys.ToArray())
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
                        LayoutDrawHeader(i);
                        foreach (var k in p.Config.Layouts[i].Elements.Keys.ToArray())
                        {
                            LayoutDrawElement(i, k);
                        }
                    }
                }
                if (colored)
                {
                    ImGui.PopStyleColor();
                    colored = false;
                }
            }
            if (!open) curEdit = null;
            ImGui.EndChild();
        }
    }
}
