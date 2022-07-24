using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Interface.Colors;
using Dalamud.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splatoon.ConfigGui.CGuiLayouts
{
    internal static class LayoutDrawSelector
    {
        internal static Layout CurrentLayout = null;
        internal static Element CurrentElement = null;
        internal static void DrawSelector(this Layout x, string group, int index)
        {
            if (CGui.layoutFilter != "" && !x.Name.Contains(CGui.layoutFilter, StringComparison.OrdinalIgnoreCase))
            {
                if(CGui.ScrollTo == x)
                {
                    CGui.ScrollTo = null;
                }
                return;
            }
            ImGui.PushID(x.GUID);
            {
                var col = false;
                if (!x.Enabled)
                {
                    col = true;
                    ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.DalamudGrey3);
                }
                ImGui.SetCursorPosX(group == null ? 0 : 10);
                var curpos = ImGui.GetCursorScreenPos();
                var contRegion = ImGui.GetContentRegionAvail().X;
                if (CGui.ScrollTo == x)
                {
                    ImGui.SetScrollHereY();
                    CGui.ScrollTo = null;
                }
                if (ImGui.Selectable($"{x.Name}", CurrentLayout == x))
                {
                    if (CurrentLayout == x && CurrentElement == null)
                    {
                        CurrentLayout = null;
                    }
                    else
                    {
                        CGui.OpenedGroup.Add(group);
                        CurrentLayout = x;
                        CurrentElement = null;
                    }
                }
                if (ImGui.IsItemClicked(ImGuiMouseButton.Middle))
                {
                    x.Enabled = !x.Enabled;
                }
                Safe(delegate
                {
                    if (ImGui.BeginDragDropSource())
                    {
                        ImGuiDragDrop.SetDragDropPayload("MoveLayout", index);
                        ImGuiEx.Text($"Moving layout\n{x.Name}");
                        ImGui.EndDragDropSource();
                    }
                    if (ImGui.BeginDragDropTarget())
                    {
                        //Svc.Chat.Print($"DragDropTarget: {index}");
                        if (ImGuiDragDrop.AcceptDragDropPayload("MoveLayout", out int indexOfMovedObj, 
                            ImGuiDragDropFlags.AcceptNoDrawDefaultRect | ImGuiDragDropFlags.AcceptBeforeDelivery))
                        {
                            SImGuiEx.DrawLine(curpos, contRegion);
                            if (ImGui.IsMouseReleased(ImGuiMouseButton.Left))
                            {
                                var exch = P.Config.LayoutsL[indexOfMovedObj];
                                exch.Group = group ?? "";
                                P.Config.LayoutsL[indexOfMovedObj] = null;
                                P.Config.LayoutsL.Insert(index, exch);
                                P.Config.LayoutsL.RemoveAll(x => x == null);
                            }
                        }
                        ImGui.EndDragDropTarget();
                    }
                });
                if (col)
                {
                    ImGui.PopStyleColor();
                }
            }
            if (CurrentLayout == x)
            {
                foreach (var e in CurrentLayout.ElementsL)
                {
                    ImGui.PushID(e.GUID);
                    ImGui.SetCursorPosX(group == null? 10 : 20);
                    var col = false;
                    if (!e.Enabled)
                    {
                        col = true;
                        ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.DalamudGrey3);
                    }
                    else if (!x.Enabled)
                    {
                        col = true;
                        ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.DalamudGrey);
                    }
                    if (ImGui.Selectable($"{e.Name}", CurrentElement == e))
                    {
                        CGui.OpenedGroup.Add(group);
                        CurrentElement = e;
                    }
                    if (ImGui.IsItemClicked(ImGuiMouseButton.Middle))
                    {
                        e.Enabled = !e.Enabled;
                    }
                    if (col)
                    {
                        ImGui.PopStyleColor();
                    }
                    ImGui.PopID();
                }
            }
            ImGui.PopID();
        }
    }
}
