using Dalamud.Interface.Colors;
using Dalamud.Interface.Internal.Notifications;
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
        float ShareWidth = 0;

        void CopyToCb(string i)
        {
            ImGui.SetClipboardText(i + "~" + JsonConvert.SerializeObject(p.Config.Layouts[i], Formatting.None, new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Ignore }));
            Notify("Copied to clipboard", NotificationType.Success);
        }

        void LayoutDrawHeader(string i)
        {
            var topCursorPos = ImGui.GetCursorPos();
            var layout = p.Config.Layouts[i];
            ImGui.Checkbox("Enabled##" + i, ref layout.Enabled);
            ImGui.SameLine();
            ImGui.Checkbox("Disable in duty##" + i, ref layout.DisableInDuty);
            ImGui.SameLine();
            ImGui.SetNextItemWidth(100f);
            if(ImGui.BeginCombo("##phaseSelectorL", $"{(layout.Phase == 0?"Any phase":$"Phase {layout.Phase}")}"))
            {
                if (ImGui.Selectable("Any phase")) layout.Phase = 0;
                if (ImGui.Selectable("Phase 1 (doorboss)")) layout.Phase = 1;
                if (ImGui.Selectable("Phase 2 (post-doorboss)")) layout.Phase = 2;
                ImGui.Text("Manual phase selection:");
                ImGui.SameLine();
                ImGui.SetNextItemWidth(30f);
                ImGui.DragInt("##mPSel", ref layout.Phase, 0.1f, 0, 9);
                ImGui.EndCombo();
            }
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
                    Notify("Copied to clipboard", NotificationType.Success);
                }
                if (ImGui.Selectable("Copy disable command"))
                {
                    ImGui.SetClipboardText("/splatoon disable " + i);
                    Notify("Copied to clipboard", NotificationType.Success);
                }
                if (ImGui.Selectable("Copy as HTTP param##" + i))
                {
                    HTTPExportToClipboard(p.Config.Layouts[i]);
                    Notify("Copied to clipboard", NotificationType.Success);
                }
                if (ImGui.IsItemHovered())
                {
                    ImGui.SetTooltip("Hold ALT to copy raw JSON (for usage with post body or you'll have to urlencode it yourself)\nHold CTRL and click to copy urlencoded raw");
                }
                ImGui.EndCombo();
            }
            ShareWidth += ImGui.GetItemRectSize().X;

            SImGuiEx.GSameLine(delegate
            {
                ImGui.Text("Display conditions:");
                ImGui.SetNextItemWidth(WidthCombo);
                ImGui.Combo("##dcn" + i, ref layout.DCond, Layout.DisplayConditions, Layout.DisplayConditions.Length);
            }, out var upperTextCursor);
            var delta = (ImGui.GetWindowContentRegionWidth() - WidthCombo * 3f - ImGui.GetStyle().ItemSpacing.X * 2f) / 2;
            upperTextCursor += delta;

            var colorZLock = Svc.ClientState?.TerritoryType != null
                && layout.ZoneLockH.Count != 0
                && !layout.ZoneLockH.Contains(Svc.ClientState.TerritoryType)
                && Environment.TickCount64 % 1000 < 500;
            if (colorZLock) ImGui.PushStyleColor(ImGuiCol.Text, Colors.Red);
            SImGuiEx.GSameLine(delegate
            {
                ImGui.SetCursorPosX(ImGui.GetCursorPosX() + delta);
                ImGui.Text("Zone lock: ");
                ImGui.SetNextItemWidth(WidthCombo);
                ImGui.SetCursorPosX(upperTextCursor);
                layout.ZoneLockH.RemoveWhere(el => !p.Zones.ContainsKey(el));
                if (ImGui.BeginCombo("##zlk" + i, layout.ZoneLockH.Count == 0 ? "All zones" :
                    layout.ZoneLockH.Count == 1 ? layout.ZoneLockH.First() + " / "
                    + p.Zones[layout.ZoneLockH.First()].PlaceName.Value.Name :
                    layout.ZoneLockH.Count + " zones"
                    ))
                {
                    if (colorZLock) ImGui.PopStyleColor();
                    ImGui.SetNextItemWidth(100f);
                    ImGui.InputTextWithHint("##zfltr" + i, "Filter", ref zlockf, 100);
                    ImGui.SameLine();
                    ImGui.Checkbox("Only selected", ref zlockcur);
                    if (p.Zones.ContainsKey(Svc.ClientState.TerritoryType))
                    {
                        ImGui.PushStyleColor(ImGuiCol.Text, Colors.Yellow);
                        if (layout.ZoneLockH.Contains(Svc.ClientState.TerritoryType))
                        {
                            SImGuiEx.ColorButton(Colors.Red);
                        }
                        string zcfc = p.Zones[Svc.ClientState.TerritoryType].ContentFinderCondition?.Value.Name?.ToString();
                        if (p.Zones.ContainsKey(Svc.ClientState.TerritoryType) && ImGui.SmallButton("Current zone: " + Svc.ClientState.TerritoryType + " / "
                            + p.Zones[Svc.ClientState.TerritoryType].PlaceName.Value.Name +
                            (string.IsNullOrEmpty(zcfc) ? "" : $" ({zcfc})")))
                        {
                            layout.ZoneLockH.Toggle(Svc.ClientState.TerritoryType);
                        }
                        SImGuiEx.UncolorButton();
                        ImGui.PopStyleColor();
                    }
                    foreach (var z in p.Zones)
                    {
                        string azcfc = z.Value.ContentFinderCondition?.Value.Name?.ToString();
                        if (z.Value.PlaceName.Value.Name.ToString().Length == 0) continue;
                        var s = z.Key + " / " + z.Value.PlaceName.Value.Name + (string.IsNullOrEmpty(azcfc) ? "" : $" ({azcfc})");
                        if (!s.ToLower().Contains(zlockf)) continue;
                        if (zlockcur && !layout.ZoneLockH.Contains(z.Key)) continue;
                        if (layout.ZoneLockH.Contains(z.Key))
                        {
                            SImGuiEx.ColorButton(Colors.Red);
                        }
                        if (ImGui.SmallButton(s))
                        {
                            layout.ZoneLockH.Toggle(z.Key);
                        }
                        SImGuiEx.UncolorButton();
                    }
                    ImGui.EndCombo();
                }
                else
                {
                    if (colorZLock) ImGui.PopStyleColor();
                }
            }, out upperTextCursor);
            upperTextCursor += (ImGui.GetWindowContentRegionWidth() - WidthCombo * 3f - ImGui.GetStyle().ItemSpacing.X * 2f) / 2;

            var jprev = new List<string>();
            if (layout.JobLock == 0)
            {
                jprev.Add("All jobs");
            }
            else
            {
                foreach (var k in p.Jobs)
                {
                    if (Bitmask.IsBitSet(layout.JobLock, k.Key))
                    {
                        jprev.Add(k.Value);
                    }
                }
            }
            var colorJLock = Svc.ClientState?.LocalPlayer?.ClassJob != null
                && layout.JobLock != 0
                && !Bitmask.IsBitSet(layout.JobLock, (int)Svc.ClientState.LocalPlayer.ClassJob.Id)
                && Environment.TickCount64 % 1000 < 500;
            if (colorJLock) ImGui.PushStyleColor(ImGuiCol.Text, Colors.Red);
            SImGuiEx.GSameLine(delegate
            {
                ImGui.SetCursorPosX(ImGui.GetCursorPosX() + delta);
                ImGui.Text("Job lock");
                ImGui.SetCursorPosX(upperTextCursor);
                ImGui.SetNextItemWidth(WidthCombo);
                if (ImGui.BeginCombo("##joblock" + i, jprev.Count < 3 ? string.Join(", ", jprev) : jprev.Count + " jobs"))
                {
                    if (colorJLock) ImGui.PopStyleColor();
                    ImGui.InputTextWithHint("##joblockfltr" + i, "Filter", ref jobFilter, 100);
                    foreach (var k in p.Jobs)
                    {
                        if (!k.Key.ToString().Contains(jobFilter) && !k.Value.Contains(jobFilter)) continue;
                        if (k.Key == 0) continue;
                        var col = false;
                        if (Bitmask.IsBitSet(layout.JobLock, k.Key))
                        {
                            ImGui.PushStyleColor(ImGuiCol.Button, Colors.Red);
                            ImGui.PushStyleColor(ImGuiCol.ButtonHovered, Colors.Red);
                            col = true;
                        }
                        if (ImGui.SmallButton(k.Key + " / " + k.Value + "##selectjob" + i))
                        {
                            if (Bitmask.IsBitSet(layout.JobLock, k.Key))
                            {
                                Bitmask.ResetBit(ref layout.JobLock, k.Key);
                            }
                            else
                            {
                                Bitmask.SetBit(ref layout.JobLock, k.Key);
                            }
                        }
                        if (col) ImGui.PopStyleColor(2);
                    }
                    ImGui.EndCombo();
                }
                else
                {
                    if (colorJLock) ImGui.PopStyleColor();
                }
            }, out upperTextCursor, true);

            ImGui.Checkbox("Distance limit##dlimit" + i, ref layout.UseDistanceLimit);
            if (layout.UseDistanceLimit)
            {
                ImGui.SameLine();
                ImGui.SetNextItemWidth(150f);
                ImGui.SameLine();
                ImGui.Combo("##dlimittype" + i, ref layout.DistanceLimitType, new string[] { "Distance to current target", "Distance to element" }, 2);
                ImGui.SameLine();
                ImGui.SetNextItemWidth(50f);
                ImGui.DragFloat("##dlimit1" + i, ref layout.MinDistance, 0.1f);
                if (ImGui.IsItemHovered()) ImGui.SetTooltip("Including this value");
                ImGui.SameLine();
                ImGui.TextUnformatted("-");
                ImGui.SameLine();
                ImGui.SetNextItemWidth(50f);
                ImGui.DragFloat("##dlimit2" + i, ref layout.MaxDistance, 0.1f);
                if (ImGui.IsItemHovered()) ImGui.SetTooltip("Excluding this value");
                if (layout.DistanceLimitType == 0)
                {
                    ImGui.SameLine();
                    ImGui.TextUnformatted("Hitbox:");
                    ImGui.SameLine();
                    ImGui.Checkbox("+my##" + i, ref layout.DistanceLimitMyHitbox);
                    if (ImGui.IsItemHovered()) ImGui.SetTooltip("Add my hitbox value to distance calculation");
                    ImGui.SameLine();
                    ImGui.Checkbox("+target##" + i, ref layout.DistanceLimitTargetHitbox);
                    if (ImGui.IsItemHovered()) ImGui.SetTooltip("Add target's hitbox value to distance calculation");
                }
            }
            else
            {
                if (!layout.UseTriggers)
                {
                    ImGui.SameLine();
                }
            }

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
                        ImGui.TextUnformatted("Reset on:");
                        ImGui.SameLine();
                        ImGui.Checkbox("Combat exit##" + i + n, ref layout.Triggers[n].ResetOnCombatExit);
                        ImGui.SameLine();
                        ImGui.Checkbox("Territory change##" + i + n, ref layout.Triggers[n].ResetOnTChange);
                        ImGui.SameLine();
                        ImGui.TextUnformatted("State: " + layout.Triggers[n].FiredState);
                        if (layout.Triggers[n].Type == 0 || layout.Triggers[n].Type == 1)
                        {
                            ImGui.TextUnformatted("Time: ");
                            ImGui.SameLine();
                            ImGui.SetNextItemWidth(50f);
                            ImGui.DragFloat("##triggertime1" + i + n, ref layout.Triggers[n].TimeBegin, 0.1f, 0, 3599, "%.1f");
                            ImGui.SameLine();
                            ImGui.TextUnformatted(DateTimeOffset.FromUnixTimeMilliseconds((long)(layout.Triggers[n].TimeBegin * 1000)).ToString("mm:ss.f"));
                        }
                        else
                        {
                            ImGui.SetNextItemWidth(300f);
                            ImGui.InputTextWithHint("##textinput1" + n + i, "Case-insensitive message", ref layout.Triggers[n].Match, 1000);

                            ImGui.SameLine(); 
                            ImGui.TextUnformatted("Delay: ");
                            ImGui.SameLine(); 
                            ImGui.SetNextItemWidth(50f);
                            ImGui.DragFloat("##triggertime1" + i + n, ref layout.Triggers[n].MatchDelay, 0.1f, 0, 3599, "%.1f");
                            ImGui.SameLine();
                            ImGui.TextUnformatted(DateTimeOffset.FromUnixTimeMilliseconds((long)(layout.Triggers[n].MatchDelay*1000)).ToString("mm:ss.f"));
                            layout.Triggers[n].Match = layout.Triggers[n].Match.RemoveSymbols(Splatoon.InvalidSymbols);
                        }
                        ImGui.SameLine();
                        ImGui.TextUnformatted("Duration: ");
                        ImGui.SameLine();
                        ImGui.SetNextItemWidth(50f);
                        ImGui.DragFloat("##triggertime2" + i + n, ref layout.Triggers[n].Duration, 0.1f, 0, 3599, "%.1f");
                        ImGui.SameLine();
                        ImGui.TextUnformatted(layout.Triggers[n].Duration == 0 ? "Infinite" : DateTimeOffset.FromUnixTimeMilliseconds((long)(layout.Triggers[n].Duration * 1000)).ToString("mm:ss.f"));
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
                if (layout.Elements.ContainsKey(ename))
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
                    layout.Elements.Add(ename, el);
                    ename = "";
                }
            }
            ImGui.SameLine();
            if (ImGui.Button("Paste from clipboard##addelement" + i))
            {
                if (layout.Elements.ContainsKey(ename))
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
                        layout.Elements.Add(ename, el);
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
}
