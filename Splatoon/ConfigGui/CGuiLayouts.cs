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
                try
                {
                    ImportFromText(ImGui.GetClipboardText());
                }
                catch (Exception e)
                {
                    p.Log(e.Message + "\n" + e.StackTrace);
                }
            }
            /*ImGui.SameLine();
            if (ImGui.Button("file"))
            {
                try
                {
                    using (OpenFileDialog openFileDialog = new OpenFileDialog())
                    {
                        openFileDialog.Filter = "json files (*.json)|*.json";
                        openFileDialog.FilterIndex = 0;
                        openFileDialog.RestoreDirectory = true;
                        openFileDialog.Title = "Select file to import";

                        if (openFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            //Read the contents of the file into a stream
                            var fileStream = openFileDialog.OpenFile();

                            using (StreamReader reader = new StreamReader(fileStream))
                            {
                                ImportFromText(reader.ReadToEnd());
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    p.Log(e.Message + "\n" + e.StackTrace);
                }
            }*/

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
                        ImGui.Checkbox("Enabled##" + i, ref p.Config.Layouts[i].Enabled);
                        ImGui.SameLine();
                        ImGui.Checkbox("Prevent controlling with web api##" + i, ref p.Config.Layouts[i].DisableDisabling);
                        ImGui.SameLine();
                        ImGui.Checkbox("Disable in duty##" + i, ref p.Config.Layouts[i].DisableInDuty);
                        if (ImGui.Button("Export to clipboard"))
                        {
                            ImGui.SetClipboardText(i + "~" + JsonConvert.SerializeObject(p.Config.Layouts[i], Formatting.None, new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Ignore }));
                        }
                        ImGui.SameLine();
                        if (ImGui.Button("Copy enable command"))
                        {
                            ImGui.SetClipboardText("/splatoon enable " + i);
                        }
                        ImGui.SameLine();
                        if (ImGui.Button("Copy disable command"))
                        {
                            ImGui.SetClipboardText("/splatoon disable " + i);
                        }
                        ImGui.SameLine();
                        if (ImGui.Button("Copy as HTTP param##" + i))
                        {
                            HTTPExportToClipboard(p.Config.Layouts[i]);
                        }
                        if (ImGui.IsItemHovered())
                        {
                            ImGui.SetTooltip("Hold ALT to copy raw JSON (for usage with post body or you'll have to urlencode it yourself)\nHold CTRL and click to copy urlencoded raw");
                        }
                        ImGuiEx.SizedText("Display conditions:", WidthLayout);
                        ImGui.SameLine();
                        ImGui.SetNextItemWidth(WidthCombo);
                        ImGui.Combo("##dcn" + i, ref p.Config.Layouts[i].DCond, Layout.DisplayConditions, Layout.DisplayConditions.Length);
                        /*ImGuiEx.SizedText("Visibility of layout:", WidthLayout);
                        ImGui.SameLine();
                        ImGui.SetNextItemWidth(WidthCombo);
                        ImGui.Combo("##vsb" + i, ref p.Config.Layouts[i].Visibility, Layout.VisibilityType, Layout.VisibilityType.Length);
                        if (ImGui.IsItemHovered())
                        {
                            ImGui.SetTooltip("Not yet implemented"); 
                        }
                        p.Config.Layouts[i].Visibility = 0;
                        if (p.Config.Layouts[i].Visibility > 0)
                        {
                            ImGui.SameLine();
                            ImGui.Checkbox("Auto-hide on leaving combat##"+i, ref p.Config.Layouts[i].AutoHideOutCombat);
                        }
                        if (p.Config.Layouts[i].Visibility == 2 || p.Config.Layouts[i].Visibility == 3)
                        {
                            ImGuiEx.SizedText("Message trigger to show:", WidthLayout);
                            ImGui.SameLine();
                            ImGui.SetNextItemWidth(WidthCombo);
                            ImGui.InputTextWithHint("##msgshow" + i, "Case-insensitive (partial) message", ref p.Config.Layouts[i].MessageToWatch, 100);
                        }
                        if (p.Config.Layouts[i].Visibility == 1 || p.Config.Layouts[i].Visibility == 2)
                        {
                            ImGuiEx.SizedText("Visibility time:", WidthLayout);
                            ImGui.SameLine();
                            ImGui.SetNextItemWidth(50f);
                            ImGui.DragInt("##btbg" + i, ref p.Config.Layouts[i].BattleTimeBegin, 1f, 0, 60 * 20);
                            ImGui.SameLine();
                            ImGui.TextUnformatted("-");
                            ImGui.SameLine();
                            ImGui.SetNextItemWidth(50f);
                            ImGui.DragInt("##bte" + i, ref p.Config.Layouts[i].BattleTimeEnd, 1f, p.Config.Layouts[i].BattleTimeBegin, 60 * 20);
                            ImGui.SameLine();
                            ImGui.TextUnformatted(DateTimeOffset.FromUnixTimeSeconds(p.Config.Layouts[i].BattleTimeBegin).ToString("mm:ss") + " - " +
                                DateTimeOffset.FromUnixTimeSeconds(p.Config.Layouts[i].BattleTimeEnd).ToString("mm:ss"));
                        }
                        if (p.Config.Layouts[i].Visibility == 3)
                        {
                            ImGuiEx.SizedText("Message trigger to hide:", WidthLayout);
                            ImGui.SameLine();
                            ImGui.SetNextItemWidth(WidthCombo);
                            ImGui.InputTextWithHint("##msghide" + i, "Case-insensitive (partial) message", ref p.Config.Layouts[i].MessageToWatchForEnd, 100);
                        }*/
                        var colorZLock = Svc.ClientState?.TerritoryType != null
                            && p.Config.Layouts[i].ZoneLockH.Count != 0
                            && !p.Config.Layouts[i].ZoneLockH.Contains(Svc.ClientState.TerritoryType)
                            && Environment.TickCount64 % 1000 < 500;
                        if (colorZLock) ImGui.PushStyleColor(ImGuiCol.Text, Colors.Red);
                        ImGuiEx.SizedText("Zone lock: ", WidthLayout);
                        ImGui.SameLine();
                        ImGui.SetNextItemWidth(WidthCombo);
                        p.Config.Layouts[i].ZoneLockH.RemoveWhere(el => !p.Zones.ContainsKey(el));
                        if (ImGui.BeginCombo("##zlk" + i, p.Config.Layouts[i].ZoneLockH.Count == 0 ? "All zones" :
                            p.Config.Layouts[i].ZoneLockH.Count == 1 ? p.Config.Layouts[i].ZoneLockH.First() + " / "
                            + p.Zones[p.Config.Layouts[i].ZoneLockH.First()].PlaceName.Value.Name :
                            p.Config.Layouts[i].ZoneLockH.Count + " zones"
                            ))
                        {
                            if (colorZLock) ImGui.PopStyleColor();
                            ImGui.SetNextItemWidth(100f);
                            ImGui.InputTextWithHint("##zfltr" + i, "Filter", ref zlockf, 100);
                            ImGui.SameLine();
                            ImGui.Checkbox("Only selected", ref zlockcur);
                            ImGui.PushStyleColor(ImGuiCol.Text, Colors.Yellow);
                            if (p.Config.Layouts[i].ZoneLockH.Contains(Svc.ClientState.TerritoryType))
                            {
                                ImGuiEx.ColorButton(Colors.Red);
                            }
                            if (ImGui.SmallButton("Current zone: " + Svc.ClientState.TerritoryType + " / "
                                + p.Zones[Svc.ClientState.TerritoryType].PlaceName.Value.Name))
                            {
                                p.Config.Layouts[i].ZoneLockH.Toggle(Svc.ClientState.TerritoryType);
                            }
                            ImGuiEx.UncolorButton();
                            ImGui.PopStyleColor();
                            foreach (var z in p.Zones)
                            {
                                if (z.Value.PlaceName.Value.Name.ToString().Length == 0) continue;
                                var s = z.Key + " / " + z.Value.PlaceName.Value.Name;
                                if (!s.ToLower().Contains(zlockf)) continue;
                                if (zlockcur && !p.Config.Layouts[i].ZoneLockH.Contains(z.Key)) continue;
                                if (p.Config.Layouts[i].ZoneLockH.Contains(z.Key))
                                {
                                    ImGuiEx.ColorButton(Colors.Red);
                                }
                                if (ImGui.SmallButton(s))
                                {
                                    p.Config.Layouts[i].ZoneLockH.Toggle(z.Key);
                                }
                                ImGuiEx.UncolorButton();
                            }
                            ImGui.EndCombo();
                        }
                        else
                        {
                            if (colorZLock) ImGui.PopStyleColor();
                        }

                        var jprev = new List<string>();
                        if (p.Config.Layouts[i].JobLock == 0)
                        {
                            jprev.Add("All jobs");
                        }
                        else
                        {
                            foreach (var k in p.Jobs)
                            {
                                if (Bitmask.IsBitSet(p.Config.Layouts[i].JobLock, k.Key))
                                {
                                    jprev.Add(k.Value);
                                }
                            }
                        }
                        var colorJLock = Svc.ClientState?.LocalPlayer?.ClassJob != null
                            && p.Config.Layouts[i].JobLock != 0
                            && !Bitmask.IsBitSet(p.Config.Layouts[i].JobLock, (int)Svc.ClientState.LocalPlayer.ClassJob.Id)
                            && Environment.TickCount64 % 1000 < 500;
                        if (colorJLock) ImGui.PushStyleColor(ImGuiCol.Text, Colors.Red);
                        ImGuiEx.SizedText("Job lock", WidthLayout);
                        ImGui.SameLine();
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
                                if (Bitmask.IsBitSet(p.Config.Layouts[i].JobLock, k.Key))
                                {
                                    ImGui.PushStyleColor(ImGuiCol.Button, Colors.Red);
                                    ImGui.PushStyleColor(ImGuiCol.ButtonHovered, Colors.Red);
                                    col = true;
                                }
                                if (ImGui.SmallButton(k.Key + " / " + k.Value + "##selectjob" + i))
                                {
                                    if (Bitmask.IsBitSet(p.Config.Layouts[i].JobLock, k.Key))
                                    {
                                        Bitmask.ResetBit(ref p.Config.Layouts[i].JobLock, k.Key);
                                    }
                                    else
                                    {
                                        Bitmask.SetBit(ref p.Config.Layouts[i].JobLock, k.Key);
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

                        ImGuiEx.SizedText("Distance limit:", WidthLayout);
                        ImGui.SameLine();
                        ImGui.Checkbox("##dlimit" + i, ref p.Config.Layouts[i].UseDistanceLimit);
                        if (p.Config.Layouts[i].UseDistanceLimit)
                        {
                            ImGui.SameLine();
                            ImGui.SetNextItemWidth(150f);
                            ImGui.SameLine();
                            ImGui.Combo("##dlimittype" + i, ref p.Config.Layouts[i].DistanceLimitType, new string[] { "Distance to current target", "Distance to element" }, 2);
                            ImGui.SameLine();
                            ImGui.SetNextItemWidth(50f);
                            ImGui.DragFloat("##dlimit1" + i, ref p.Config.Layouts[i].MinDistance, 0.1f);
                            if (ImGui.IsItemHovered()) ImGui.SetTooltip("Including this value");
                            ImGui.SameLine();
                            ImGui.TextUnformatted("-");
                            ImGui.SameLine();
                            ImGui.SetNextItemWidth(50f);
                            ImGui.DragFloat("##dlimit2" + i, ref p.Config.Layouts[i].MaxDistance, 0.1f);
                            if (ImGui.IsItemHovered()) ImGui.SetTooltip("Excluding this value");
                            if (p.Config.Layouts[i].DistanceLimitType == 0)
                            {
                                ImGui.SameLine();
                                ImGui.TextUnformatted("Hitbox:");
                                ImGui.SameLine();
                                ImGui.Checkbox("+my##" + i, ref p.Config.Layouts[i].DistanceLimitMyHitbox);
                                if (ImGui.IsItemHovered()) ImGui.SetTooltip("Add my hitbox value to distance calculation");
                                ImGui.SameLine();
                                ImGui.Checkbox("+target##" + i, ref p.Config.Layouts[i].DistanceLimitTargetHitbox);
                                if (ImGui.IsItemHovered()) ImGui.SetTooltip("Add target's hitbox value to distance calculation");
                            }
                        }

                        if (p.Config.Layouts[i].UseTriggers)
                        {
                            ImGui.Checkbox("##usetrigger" + i, ref p.Config.Layouts[i].UseTriggers);
                            ImGui.SameLine();
                            ImGui.PushStyleColor(ImGuiCol.Header, Colors.Transparent);
                            if(ImGui.CollapsingHeader("Trigger settings##" + i))
                            {
                                if(ImGui.Button("Add new trigger##" + i))
                                {
                                    p.Config.Layouts[i].Triggers.Add(new Trigger());
                                }
                                var deleteTrigger = -1;
                                for(var n = 0; n < p.Config.Layouts[i].Triggers.Count; n++)
                                {
                                    if(ImGui.Button("[X]##"+n+i) && ImGui.GetIO().KeyCtrl)
                                    {
                                        deleteTrigger = n;
                                    }
                                    if (ImGui.IsItemHovered())
                                    {
                                        ImGui.SetTooltip("Hold CTRL + left click to delete");
                                    }
                                    ImGui.SameLine();
                                    ImGui.SetNextItemWidth(WidthCombo);
                                    ImGui.Combo("##trigger" + i + n, ref p.Config.Layouts[i].Triggers[n].Type, Trigger.Types, Trigger.Types.Length);
                                    ImGui.SameLine();
                                    ImGui.TextUnformatted("Reset on:");
                                    ImGui.SameLine();
                                    ImGui.Checkbox("Combat exit##" + i + n, ref p.Config.Layouts[i].Triggers[n].ResetOnCombatExit);
                                    ImGui.SameLine();
                                    ImGui.Checkbox("Territory change##" + i + n, ref p.Config.Layouts[i].Triggers[n].ResetOnTChange);
                                    ImGui.SameLine();
                                    ImGui.TextUnformatted("State: " + p.Config.Layouts[i].Triggers[n].FiredState);
                                    if (p.Config.Layouts[i].Triggers[n].Type == 0 || p.Config.Layouts[i].Triggers[n].Type == 1)
                                    {
                                        ImGui.TextUnformatted("Time: ");
                                        ImGui.SameLine();
                                        ImGui.SetNextItemWidth(50f);
                                        ImGui.DragInt("##triggertime1" + i + n, ref p.Config.Layouts[i].Triggers[n].TimeBegin, 0.2f, 0, 3599);
                                        ImGui.SameLine();
                                        ImGui.TextUnformatted(DateTimeOffset.FromUnixTimeSeconds(p.Config.Layouts[i].Triggers[n].TimeBegin).ToString("mm:ss"));
                                    }
                                    else
                                    {
                                        ImGui.SetNextItemWidth(400f);
                                        ImGui.InputTextWithHint("##textinput1"+n+i, "Case-insensitive message", ref p.Config.Layouts[i].Triggers[n].Match, 1000);
                                    }
                                    ImGui.SameLine();
                                    ImGui.TextUnformatted("Duration: ");
                                    ImGui.SameLine();
                                    ImGui.SetNextItemWidth(50f);
                                    ImGui.DragInt("##triggertime2" + i + n, ref p.Config.Layouts[i].Triggers[n].Duration, 0.2f, 0, 3599);
                                    ImGui.SameLine();
                                    ImGui.TextUnformatted(p.Config.Layouts[i].Triggers[n].Duration == 0 ? "Infinite" : DateTimeOffset.FromUnixTimeSeconds(p.Config.Layouts[i].Triggers[n].Duration).ToString("mm:ss"));
                                    ImGui.Separator();
                                }
                                if(deleteTrigger != -1)
                                {
                                    try
                                    {
                                        p.Config.Layouts[i].Triggers.RemoveAt(deleteTrigger);
                                    }
                                    catch(Exception e)
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
                            ImGui.Checkbox("Use trigger system##usetrigger" + i, ref p.Config.Layouts[i].UseTriggers);
                        }

                        ImGui.TextColored(ImGui.ColorConvertU32ToFloat4(Colors.Green), "Add elements to the layout to create markers:");
                        ImGui.SameLine();
                        ImGui.PushItemWidth(WidthCombo);
                        ImGui.InputTextWithHint("##elnameadd" + i, "Unique element name", ref ename, 100);
                        ImGui.PopItemWidth();
                        ImGui.SameLine();
                        if (ImGui.Button("Add element##addelement" + i))
                        {
                            if (p.Config.Layouts[i].Elements.ContainsKey(ename))
                            {
                                p.Log("Error: this name already exists", true);
                            }
                            else if (ename.Length == 0)
                            {
                                p.Log("Error: you must name layout", true);
                            }
                            else
                            {
                                var el = new Element(0);
                                el.refX = GetPlayerPositionXZY().X;
                                el.refY = GetPlayerPositionXZY().Y;
                                el.refZ = GetPlayerPositionXZY().Z;
                                p.Config.Layouts[i].Elements.Add(ename, el);
                                ename = "";
                            }
                        }
                        foreach (var k in p.Config.Layouts[i].Elements.Keys.ToArray())
                        {
                            var cursor = ImGui.GetCursorPos();
                            var el = p.Config.Layouts[i].Elements[k];
                            var elcolored = false;
                            if (!el.Enabled)
                            {
                                ImGui.PushStyleColor(ImGuiCol.Text, Colors.Gray);
                                elcolored = true;
                            }
                            if (ImGui.CollapsingHeader(i + " / " + k + "##elem" + i + k))
                            {
                                if (elcolored)
                                {
                                    ImGui.PopStyleColor();
                                    elcolored = false;
                                }
                                if (enableDeletionElement)
                                {
                                    ImGui.PushStyleColor(ImGuiCol.Button, Colors.Orange);
                                    if (ImGui.Button("Delete##elemdel" + i + k))
                                    {
                                        p.Config.Layouts[i].Elements.Remove(k);
                                    }
                                    ImGui.PopStyleColor();
                                    ImGui.SameLine();
                                }
                                if (p.Config.Layouts[i].Elements.ContainsKey(k))
                                {
                                    ImGui.Checkbox("Enabled##" + i + k, ref el.Enabled);
                                    ImGui.SameLine();
                                    if (ImGui.Button("Copy as HTTP param##" + i + k))
                                    {
                                        HTTPExportToClipboard(el);
                                    }

                                    if (ImGui.IsItemHovered())
                                    {
                                        ImGui.SetTooltip("Hold ALT to copy raw JSON (for usage with post body or you'll have to urlencode it yourself)\nHold CTRL and click to copy urlencoded raw");
                                    }
                                    ImGui.SameLine();
                                    if (ImGui.Button("Copy style##"+i+k))
                                    {
                                        p.Clipboard = JsonConvert.DeserializeObject<Element>(JsonConvert.SerializeObject(el));
                                    }
                                    if (p.Clipboard != null)
                                    {
                                        ImGui.SameLine();
                                        if (ImGui.Button("Paste style##" + i + k))
                                        {
                                            el.color = p.Clipboard.color;
                                            el.overlayBGColor = p.Clipboard.overlayBGColor;
                                            el.overlayTextColor = p.Clipboard.overlayTextColor;
                                            el.tether = p.Clipboard.tether;
                                            el.thicc = p.Clipboard.thicc;
                                            el.overlayVOffset = p.Clipboard.overlayVOffset;
                                            if (ImGui.GetIO().KeyCtrl)
                                            {
                                                el.radius = p.Clipboard.radius;
                                                el.includeHitbox = p.Clipboard.includeHitbox;
                                                el.includeOwnHitbox = p.Clipboard.includeOwnHitbox;
                                                el.includeRotation = p.Clipboard.includeRotation;
                                                el.onlyTargetable = p.Clipboard.onlyTargetable;
                                            }
                                            if (ImGui.GetIO().KeyShift && el.type != 2)
                                            {
                                                el.refX = p.Clipboard.refX;
                                                el.refY = p.Clipboard.refY;
                                                el.refZ = p.Clipboard.refZ;
                                            }
                                        }
                                        if (ImGui.IsItemHovered())
                                        {
                                            ImGui.BeginTooltip();
                                            ImGui.TextUnformatted("Copied style:");
                                            ImGui.TextUnformatted($"Color: 0x{p.Clipboard.color:X8}");
                                            ImGui.SameLine();
                                            ImGuiEx.DisplayColor(p.Clipboard.color);
                                            ImGui.TextUnformatted($"Overlay BG color: 0x{p.Clipboard.overlayBGColor:X8}");
                                            ImGui.SameLine();
                                            ImGuiEx.DisplayColor(p.Clipboard.overlayBGColor);
                                            ImGui.TextUnformatted($"Overlay text color: 0x{p.Clipboard.overlayTextColor:X8}");
                                            ImGui.SameLine();
                                            ImGuiEx.DisplayColor(p.Clipboard.overlayTextColor);
                                            ImGui.TextUnformatted($"Overlay vertical offset: {p.Clipboard.overlayVOffset}");
                                            ImGui.TextUnformatted($"Thickness: {p.Clipboard.thicc}");
                                            ImGui.TextUnformatted($"Tether: {p.Clipboard.tether}");
                                            ImGui.Separator();
                                            ImGui.TextColored((ImGui.GetIO().KeyCtrl ? Colors.Green : Colors.Gray).ToVector4(),
                                                "Holding CTRL when clicking will also paste:");
                                            ImGui.TextUnformatted($"Radius: {p.Clipboard.radius}");
                                            ImGui.TextUnformatted($"Include target hitbox: {p.Clipboard.includeHitbox}");
                                            ImGui.TextUnformatted($"Include own hitbox: {p.Clipboard.includeOwnHitbox}");
                                            ImGui.TextUnformatted($"Include rotation: {p.Clipboard.includeRotation}");
                                            ImGui.TextUnformatted($"Only targetable: {p.Clipboard.onlyTargetable}");
                                            ImGui.Separator();
                                            ImGui.TextColored((ImGui.GetIO().KeyShift ? Colors.Green : Colors.Gray).ToVector4(),
                                                "Holding SHIFT when clicking will also paste:");
                                            ImGui.TextUnformatted($"X offset: {p.Clipboard.offX}");
                                            ImGui.TextUnformatted($"Y offset: {p.Clipboard.offY}");
                                            ImGui.TextUnformatted($"Z offset: {p.Clipboard.offZ}");

                                            ImGui.EndTooltip();
                                        }
                                    }


                                    ImGuiEx.SizedText("Element type:", WidthElement);
                                    ImGui.SameLine();
                                    ImGui.SetNextItemWidth(WidthCombo);
                                    ImGui.Combo("##elemselecttype" + i + k, ref el.type, Element.ElementTypes, Element.ElementTypes.Length);
                                    if (el.type == 1 || el.type == 3)
                                    {
                                        ImGuiEx.SizedText("Targeted actor: ", WidthElement);
                                        ImGui.SameLine();
                                        ImGui.SetNextItemWidth(WidthCombo);
                                        ImGui.Combo("##actortype" + i + k, ref el.refActorType, Element.ActorTypes, Element.ActorTypes.Length);
                                        if (el.refActorType == 0)
                                        {
                                            ImGui.SameLine();
                                            if (ImGui.Button("Copy settarget command##" + i + k))
                                            {
                                                ImGui.SetClipboardText("/splatoon settarget " + i + "~" + k);
                                            }
                                            if (ImGui.IsItemHovered())
                                            {
                                                ImGui.SetTooltip("This command allows you to quickly change\n" +
                                                    "search name to your active target's name.\n" +
                                                    "You can use it with macro.");
                                            }
                                            ImGuiEx.SizedText("", WidthElement);
                                            ImGui.SameLine();
                                            ImGui.SetNextItemWidth(WidthCombo);
                                            ImGui.InputTextWithHint("##actorname" + i + k, "Case-insensitive (partial) name", ref el.refActorName, 100);
                                            if (ImGui.IsItemHovered())
                                            {
                                                ImGui.SetTooltip("Keep in mind that searching actor by name is\n" +
                                                    "relatively resource expensive operation. \n" +
                                                    "Try to keep amount of these down to reasonable number.");
                                            }
                                            ImGui.SameLine();
                                            ImGui.Checkbox("Targetable only##" + i + k, ref el.onlyTargetable);
                                            if (Svc.Targets.Target != null)
                                            {
                                                ImGui.SameLine();
                                                if (ImGui.Button("Target##btarget" + i + k)) el.refActorName = Svc.Targets.Target.Name.ToString();
                                            }
                                        }
                                    }

                                    if (el.type == 0 || el.type == 2 || el.type == 3)
                                    {
                                        ImGuiEx.SizedText((el.type == 2 || el.type == 3) ? "Point A" : "Reference position: ", WidthElement);
                                        ImGui.SameLine();
                                        ImGui.PushItemWidth(60f);
                                        ImGui.TextUnformatted("X:");
                                        ImGui.SameLine();
                                        ImGui.DragFloat("##refx" + i + k, ref el.refX, 0.02f, float.MinValue, float.MaxValue);
                                        ImGui.SameLine();
                                        ImGui.TextUnformatted("Y:");
                                        ImGui.SameLine();
                                        ImGui.DragFloat("##refy" + i + k, ref el.refY, 0.02f, float.MinValue, float.MaxValue);
                                        ImGui.SameLine();
                                        ImGui.TextUnformatted("Z:");
                                        ImGui.SameLine();
                                        ImGui.DragFloat("##refz" + i + k, ref el.refZ, 0.02f, float.MinValue, float.MaxValue);
                                        ImGui.SameLine();
                                        if (ImGui.Button("0 0 0##ref" + i + k))
                                        {
                                            el.refX = 0;
                                            el.refY = 0;
                                            el.refZ = 0;
                                        }
                                        if (el.type != 3)
                                        {
                                            ImGui.SameLine();
                                            if (ImGui.Button("My position##ref" + i + k))
                                            {
                                                el.refX = GetPlayerPositionXZY().X;
                                                el.refY = GetPlayerPositionXZY().Y;
                                                el.refZ = GetPlayerPositionXZY().Z;
                                            }
                                            ImGui.SameLine();
                                            if (ImGui.Button("Screen2World##s2w1" + i + k))
                                            {
                                                if (p.IsLayoutVisible(p.Config.Layouts[i]) && el.Enabled/* && p.CamAngleY <= p.Config.maxcamY*/)
                                                {
                                                    UnsetS2W();
                                                    SetCursorTo(el.refX, el.refZ, el.refY);
                                                    el.screen2world = el.type == 0 ? 1 : 2;
                                                }
                                                else
                                                {
                                                    Svc.Toasts.ShowError("Unable to use for hidden element");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            ImGui.SameLine();
                                            ImGui.Text("Angle: " + RadToDeg(AngleBetweenVectors(0, 0, 10, 0, el.refX, el.refY, el.offX, el.offY)));
                                        }
                                        ImGui.PopItemWidth();
                                    }
                                    

                                    ImGuiEx.SizedText((el.type == 2 || el.type == 3) ? "Point B" : "Offset: ", WidthElement);
                                    ImGui.SameLine();
                                    ImGui.PushItemWidth(60f);
                                    ImGui.TextUnformatted("X:");
                                    ImGui.SameLine();
                                    ImGui.DragFloat("##offx" + i + k, ref el.offX, 0.02f, float.MinValue, float.MaxValue);
                                    ImGui.SameLine();
                                    ImGui.TextUnformatted("Y:");
                                    ImGui.SameLine();
                                    ImGui.DragFloat("##offy" + i + k, ref el.offY, 0.02f, float.MinValue, float.MaxValue);
                                    ImGui.SameLine();
                                    ImGui.TextUnformatted("Z:");
                                    ImGui.SameLine();
                                    ImGui.DragFloat("##offz" + i + k, ref el.offZ, 0.02f, float.MinValue, float.MaxValue);
                                    ImGui.SameLine();
                                    if (ImGui.Button("0 0 0##off" + i + k))
                                    {
                                        el.offX = 0;
                                        el.offY = 0;
                                        el.offZ = 0;
                                    }
                                    if (el.type == 2)
                                    {
                                        ImGui.SameLine();
                                        if (ImGui.Button("My position##off" + i + k))
                                        {
                                            el.offX = GetPlayerPositionXZY().X;
                                            el.offY = GetPlayerPositionXZY().Y;
                                            el.offZ = GetPlayerPositionXZY().Z;
                                        }
                                    }
                                    //ImGui.SameLine();
                                    //ImGui.Checkbox("Actor relative##rota"+i+k, ref el.includeRotation);
                                    if (el.type == 2)
                                    {
                                        ImGui.SameLine();
                                        if (ImGui.Button("Screen2World##s2w2" + i + k))
                                        {
                                            if (p.IsLayoutVisible(p.Config.Layouts[i]) && el.Enabled/* && p.CamAngleY <= p.Config.maxcamY*/)
                                            {
                                                UnsetS2W();
                                                SetCursorTo(el.offX, el.offZ, el.offY);
                                                el.screen2world = 3;
                                            }
                                            else
                                            {
                                                Svc.Toasts.ShowError("Unable to use for hidden element");
                                            }
                                        }
                                    }

                                    ImGuiEx.SizedText("Line thickness:", WidthElement);
                                    ImGui.SameLine();
                                    ImGui.DragFloat("##thicc" + i + k, ref el.thicc, 0.1f, 0f, float.MaxValue);
                                    ImGui.PopItemWidth();
                                    if (el.thicc > 0)
                                    {
                                        ImGui.SameLine();
                                        var v4 = ImGui.ColorConvertU32ToFloat4(el.color);
                                        if (ImGui.ColorEdit4("##colorbutton" + i + k, ref v4, ImGuiColorEditFlags.NoInputs))
                                        {
                                            el.color = ImGui.ColorConvertFloat4ToU32(v4);
                                        }
                                        ImGui.PopItemWidth();
                                    }
                                    else
                                    {
                                        ImGui.SameLine();
                                        ImGui.TextUnformatted("Thickness is set to 0: only text overlay will be drawn.");
                                    }
                                    if (el.thicc > 0)
                                    {
                                        ImGuiEx.SizedText("Radius:", WidthElement);
                                        ImGui.SameLine();
                                        ImGui.SetNextItemWidth(60f);
                                        ImGui.DragFloat("##radius" + i + k, ref el.radius, 0.01f, 0, float.MaxValue);
                                        if (ImGui.IsItemHovered())
                                            ImGui.SetTooltip("Leave at 0 to draw single dot");
                                        if (el.type == 1)
                                        {
                                            if (el.refActorType != 1)
                                            {
                                                ImGui.SameLine();
                                                ImGui.Checkbox("+target hitbox##" + i + k, ref el.includeHitbox);
                                            }
                                            ImGui.SameLine();
                                            ImGui.Checkbox("+your hitbox##" + i + k, ref el.includeOwnHitbox);
                                            ImGui.SameLine();
                                            ImGui.TextUnformatted("(?)");
                                            if (ImGui.IsItemHovered())
                                            {
                                                ImGui.SetTooltip("When the game tells you that ability A has distance D,\n" +
                                                    "in fact it means that you are allowed to execute\n" +
                                                    "ability A if distance between edge of your hitbox\n" +
                                                    "and enemy's hitbox is less or equal than distance D,\n" +
                                                    "that is for targeted abilities.\n" +
                                                    "If an ability is AoE, such check is performed between\n" +
                                                    "middle point of your character and edge of enemy's hitbox.\n\n" +
                                                    "Summary: if you are trying to make targeted ability indicator -\n" +
                                                    "enable both \"+your hitbox\" and \"+target hitbox\".\n" +
                                                    "If you are trying to make AoE ability indicator - \n" +
                                                    "enable only \"+target hitbox\" to make indicators valid.");
                                            }
                                        }
                                        if (el.type != 2)
                                        {
                                            ImGuiEx.SizedText("Tether:", WidthElement);
                                            ImGui.SameLine();
                                            ImGui.Checkbox("Enable##TetherEnable" + i + k, ref el.tether);
                                        }
                                    }
                                    ImGuiEx.SizedText("Overlay text:", WidthElement);
                                    ImGui.SameLine();
                                    ImGui.SetNextItemWidth(150f);
                                    ImGui.InputTextWithHint("##overlaytext" + i + k, "Text to display as overlay", ref el.overlayText, 30);
                                    if (el.overlayText.Length > 0)
                                    {
                                        ImGui.SameLine();
                                        ImGui.TextUnformatted("Vertical offset:");
                                        ImGui.SameLine();
                                        ImGui.SetNextItemWidth(60f);
                                        ImGui.DragFloat("##vtextadj" + i + k, ref el.overlayVOffset, 0.02f);
                                        ImGui.SameLine();
                                        ImGui.TextUnformatted("BG color:");
                                        ImGui.SameLine();
                                        var v4b = ImGui.ColorConvertU32ToFloat4(el.overlayBGColor);
                                        if (ImGui.ColorEdit4("##colorbuttonbg" + i + k, ref v4b, ImGuiColorEditFlags.NoInputs))
                                        {
                                            el.overlayBGColor = ImGui.ColorConvertFloat4ToU32(v4b);
                                        }
                                        ImGui.PopItemWidth();
                                        ImGui.SameLine();
                                        ImGui.TextUnformatted("Text color:");
                                        ImGui.SameLine();
                                        var v4t = ImGui.ColorConvertU32ToFloat4(el.overlayTextColor);
                                        if (ImGui.ColorEdit4("##colorbuttonfg" + i + k, ref v4t, ImGuiColorEditFlags.NoInputs))
                                        {
                                            el.overlayTextColor = ImGui.ColorConvertFloat4ToU32(v4t);
                                        }
                                        ImGui.PopItemWidth();
                                    }
                                }
                            }
                            if (elcolored)
                            {
                                ImGui.PopStyleColor();
                                elcolored = false;
                            }
                            var currentCursor = ImGui.GetCursorPos();
                            var text = Element.ElementTypes[el.type] + (el.type == 1 ? " [" + (el.refActorType == 0 ? el.refActorName : Element.ActorTypes[el.refActorType]) + "]" : "");
                            var textSize = ImGui.CalcTextSize(text);
                            ImGui.SetCursorPosX(ImGui.GetColumnWidth() - textSize.X - ImGui.GetStyle().ItemInnerSpacing.X);
                            ImGui.SetCursorPosY(cursor.Y + ImGui.GetStyle().ItemInnerSpacing.Y / 2);
                            ImGui.TextUnformatted(text);
                            ImGui.SetCursorPos(currentCursor);
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
