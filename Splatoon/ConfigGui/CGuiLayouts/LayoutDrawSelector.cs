using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Interface.Colors;
using Dalamud.Memory;
using Newtonsoft.Json;
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
            if (CGui.layoutFilter != "" && !x.GetName().Contains(CGui.layoutFilter, StringComparison.OrdinalIgnoreCase))
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
                if (ImGui.Selectable($"{x.GetName()}", CurrentLayout == x))
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
                if (col)
                {
                    ImGui.PopStyleColor();
                }
                if (ImGui.IsItemClicked(ImGuiMouseButton.Middle))
                {
                    x.Enabled = !x.Enabled;
                }
                if (ImGui.IsItemClicked(ImGuiMouseButton.Right))
                {
                    ImGui.OpenPopup("LayoutContext");
                }
                Safe(delegate
                {
                    if (ImGui.BeginDragDropSource())
                    {
                        ImGuiDragDrop.SetDragDropPayload("MoveLayout", index);
                        ImGuiEx.Text($"Moving layout\n{x.GetName()}");
                        ImGui.EndDragDropSource();
                    }
                    if (ImGui.BeginDragDropTarget())
                    {
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
                if (ImGui.BeginPopup("LayoutContext"))
                {
                    ImGuiEx.Text($"Layout {x.GetName()}");
                    if (ImGui.Selectable("Delete layout"))
                    {
                        x.Delete = true;
                    }
                    ImGui.EndPopup();
                }
            }
            if (CurrentLayout == x)
            {
                for (var i = 0;i<CurrentLayout.ElementsL.Count;i++)
                {
                    var e = CurrentLayout.ElementsL[i];
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
                    var curpos = ImGui.GetCursorScreenPos();
                    var contRegion = ImGui.GetContentRegionAvail().X;
                    if (ImGui.Selectable($"{e.GetName()}", CurrentElement == e))
                    {
                        if (CurrentElement == e)
                        {
                            CurrentElement = null;
                        }
                        else
                        {
                            CGui.OpenedGroup.Add(group);
                            CurrentElement = e;
                        }
                    }
                    if (col)
                    {
                        ImGui.PopStyleColor();
                    }
                    if (ImGui.IsItemClicked(ImGuiMouseButton.Middle))
                    {
                        e.Enabled = !e.Enabled;
                    }
                    if (ImGui.IsItemClicked(ImGuiMouseButton.Right))
                    {
                        ImGui.OpenPopup("ElementContext");
                    }
                    if (ImGui.BeginDragDropSource())
                    {
                        ImGuiDragDrop.SetDragDropPayload($"MoveElement{index}", i);
                        ImGuiEx.Text($"Moving element\n{x.GetName()}");
                        ImGui.EndDragDropSource();
                    }
                    if (ImGui.BeginDragDropTarget())
                    {
                        if (ImGuiDragDrop.AcceptDragDropPayload($"MoveElement{index}", out int indexOfMovedObj,
                            ImGuiDragDropFlags.AcceptNoDrawDefaultRect | ImGuiDragDropFlags.AcceptBeforeDelivery))
                        {
                            SImGuiEx.DrawLine(curpos, contRegion);
                            if (ImGui.IsMouseReleased(ImGuiMouseButton.Left))
                            {
                                var exch = CurrentLayout.ElementsL[indexOfMovedObj];
                                CurrentLayout.ElementsL[indexOfMovedObj] = null;
                                CurrentLayout.ElementsL.Insert(i, exch);
                                CurrentLayout.ElementsL.RemoveAll(x => x == null);
                            }
                        }
                        ImGui.EndDragDropTarget();
                    }
                    if (ImGui.BeginPopup("ElementContext"))
                    {
                        ImGuiEx.Text($"Layout {x.GetName()}\nElement {e.GetName()}");
                        if (ImGui.Selectable("Delete element"))
                        {
                            e.Delete = true;
                        }
                        ImGui.EndPopup();
                    }
                    ImGui.PopID();
                }
                ImGuiEx.ImGuiLineCentered("AddElement", delegate
                {
                    if(ImGui.SmallButton("Add element"))
                    {
                        x.ElementsL.Add(new(0));
                    }
                    ImGui.SameLine(); 
                    if (ImGui.SmallButton("Paste"))
                    {
                        try
                        {
                            x.ElementsL.Add(JsonConvert.DeserializeObject<Element>(ImGui.GetClipboardText()));
                        }
                        catch(Exception e)
                        {
                            Notify.Error($"{e.Message}");
                        }
                    }
                });
            }
            ImGui.PopID();
        }
    }
}
