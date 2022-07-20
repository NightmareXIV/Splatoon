using Dalamud.Interface.Colors;
using Dalamud.Interface.Components;
using ECommons.ImGuiMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splatoon
{
    internal partial class CGui
    {
        string LoggerSearch = "";
        bool IsViewer = false;
        void DisplayLogger()
        {
            ImGui.Checkbox("Enable logger", ref p.LogObjects);
            ImGui.SameLine();
            ImGui.Checkbox("Viewer mode", ref IsViewer);
            ImGuiComponents.HelpMarker("When enabled, only currently present objects are displayed");
            ImGui.SameLine();
            if(ImGui.Button("Clear list"))
            {
                p.loggedObjectList.Clear();
            }
            ImGui.SameLine();
            ImGuiEx.Text("Filter:");
            ImGui.SameLine();
            ImGui.SetNextItemWidth(ImGui.GetContentRegionAvail().X);
            ImGui.InputText("##filterLog", ref LoggerSearch, 100);
            ImGui.BeginTable("##logObjects", 13, ImGuiTableFlags.BordersInner | ImGuiTableFlags.RowBg | ImGuiTableFlags.SizingFixedFit);
            ImGui.TableSetupColumn("Object name", ImGuiTableColumnFlags.WidthStretch);
            ImGui.TableSetupColumn("Type");
            ImGui.TableSetupColumn("Object ID");
            ImGui.TableSetupColumn("Data ID");
            ImGui.TableSetupColumn("Model ID");
            ImGui.TableSetupColumn("NPC ID");
            ImGui.TableSetupColumn("Name ID");
            ImGui.TableSetupColumn("Tar. %");
            ImGui.TableSetupColumn("Vis. %");
            ImGui.TableSetupColumn("Exist");
            ImGui.TableSetupColumn("Dist.");
            ImGui.TableSetupColumn("Hibox");
            ImGui.TableSetupColumn("Life");
            ImGui.TableHeadersRow();
            var i = 0;
            foreach (var x in p.loggedObjectList)
            {
                i++;
                var mid = !x.Value.IsChar ? "--" : $"{x.Key.ModelID.Format()}";
                var oid = x.Key.ObjectID == 0xE0000000 ? "--" : $"{x.Key.ObjectID.Format()}";
                var did = x.Key.DataID == 0 ? "--" : $"{x.Key.DataID.Format()}";
                var npcid = $"{x.Key.NPCID.Format()}";
                var nameid = !x.Value.IsChar ? "--" : $"{x.Key.NameID.Format()}";
                if (LoggerSearch != "")
                {
                    if (!x.Key.Name.ToString().Contains(LoggerSearch, StringComparison.OrdinalIgnoreCase)
                        && !x.Key.type.ToString().Contains(LoggerSearch, StringComparison.OrdinalIgnoreCase)
                        && !oid.Contains(LoggerSearch, StringComparison.OrdinalIgnoreCase)
                        && !did.Contains(LoggerSearch, StringComparison.OrdinalIgnoreCase)
                        && !mid.Contains(LoggerSearch, StringComparison.OrdinalIgnoreCase)
                        && !nameid.Contains(LoggerSearch, StringComparison.OrdinalIgnoreCase)) continue;
                }
                ImGui.TableNextRow();
                ImGui.TableNextColumn();
                ImGuiEx.TextCopy(x.Key.Name);
                ImGui.TableNextColumn();
                ImGuiEx.Text($"{x.Key.type}");
                ImGui.TableNextColumn();
                ImGuiEx.TextCopy(oid);
                ImGui.SameLine();
                if (ImGui.SmallButton("Find##"+i))
                {
                    p.SFind = new()
                    {
                        includeUntargetable = true,
                        oid = x.Key.ObjectID,
                        SearchAttribute = 2
                    };
                }
                ImGui.TableNextColumn();
                ImGuiEx.TextCopy(did);
                ImGui.TableNextColumn();
                ImGuiEx.TextCopy(mid);
                ImGui.TableNextColumn();
                ImGuiEx.TextCopy(npcid);
                ImGui.TableNextColumn();
                ImGuiEx.TextCopy(nameid);
                ImGui.TableNextColumn();
                if (x.Value.Targetable) ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.HealerGreen);
                ImGui.TextUnformatted($"{(int)(((double)x.Value.TargetableTicks / (double)x.Value.ExistenceTicks) * 100)}%");
                if (x.Value.Targetable) ImGui.PopStyleColor();
                ImGui.TableNextColumn();
                if (x.Value.Visible) ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.HealerGreen);
                ImGui.TextUnformatted(!x.Value.IsChar ? "--":$"{(int)(((double)x.Value.VisibleTicks / (double)x.Value.ExistenceTicks) * 100)}%");
                if (x.Value.Visible) ImGui.PopStyleColor();
                ImGui.TableNextColumn();
                ImGui.TextUnformatted($"{x.Value.ExistenceTicks}");
                ImGui.TableNextColumn();
                ImGuiEx.Text($"{x.Value.Distance:F1}");
                ImGui.TableNextColumn();
                ImGuiEx.Text($"{x.Value.HitboxRadius:F1}");
                ImGui.TableNextColumn();
                ImGuiEx.Text($"{x.Value.Life:F1}");
            }
            ImGui.EndTable();
            if (IsViewer)
            {
                p.loggedObjectList.Clear();
            }
        }
    }
}
