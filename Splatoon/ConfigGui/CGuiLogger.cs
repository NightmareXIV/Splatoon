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
        void DisplayLogger()
        {
            ImGui.Checkbox("Enable logger", ref p.LogObjects);
            if(ImGui.Button("Clear list"))
            {
                p.loggedObjectList.Clear();
            }
            ImGui.SameLine();
            ImGui.Text("Filter:");
            ImGui.SameLine();
            ImGui.SetNextItemWidth(ImGui.GetContentRegionAvail().X);
            ImGui.InputText("##filterLog", ref LoggerSearch, 100);
            ImGui.BeginTable("##logObjects", 8, ImGuiTableFlags.BordersInner | ImGuiTableFlags.RowBg | ImGuiTableFlags.SizingFixedFit);
            ImGui.TableSetupColumn("Object name", ImGuiTableColumnFlags.WidthStretch);
            ImGui.TableSetupColumn("Type");
            ImGui.TableSetupColumn("Object ID");
            ImGui.TableSetupColumn("Data ID");
            ImGui.TableSetupColumn("Model ID");
            ImGui.TableSetupColumn("Tar. %");
            ImGui.TableSetupColumn("Vis. %");
            ImGui.TableSetupColumn("Exist ticks");
            ImGui.TableHeadersRow();
            foreach (var x in p.loggedObjectList)
            {
                if(LoggerSearch != "")
                {
                    if (!x.Key.Name.ToString().Contains(LoggerSearch, StringComparison.OrdinalIgnoreCase)
                        && !x.Key.type.ToString().Contains(LoggerSearch, StringComparison.OrdinalIgnoreCase)
                        && !$"{x.Key.ObjectID:X8}".ToString().Contains(LoggerSearch, StringComparison.OrdinalIgnoreCase)
                        && !$"{x.Key.DataID:X8}".ToString().Contains(LoggerSearch, StringComparison.OrdinalIgnoreCase)
                        && !$"{x.Key.ModelID:X4}".ToString().Contains(LoggerSearch, StringComparison.OrdinalIgnoreCase)) continue;
                }
                ImGui.TableNextRow();
                ImGui.TableNextColumn();
                ImGuiEx.TextCopy(x.Key.Name);
                ImGui.TableNextColumn();
                ImGui.Text($"{x.Key.type}");
                ImGui.TableNextColumn();
                ImGuiEx.TextCopy($"{x.Key.ObjectID:X8}");
                ImGui.TableNextColumn();
                ImGuiEx.TextCopy($"{x.Key.DataID:X8}");
                ImGui.TableNextColumn();
                ImGuiEx.TextCopy($"{x.Key.ModelID:X4}");
                ImGui.TableNextColumn();
                ImGui.TextUnformatted($"{(int)(((double)x.Value.TargetableTicks / (double)x.Value.ExistenceTicks) * 100)}%");
                ImGui.TableNextColumn();
                ImGui.TextUnformatted($"{(int)(((double)x.Value.VisibleTicks / (double)x.Value.ExistenceTicks) * 100)}%");
                ImGui.TableNextColumn();
                ImGui.TextUnformatted($"{x.Value.ExistenceTicks}");
            }
            ImGui.EndTable();
        }
    }
}
