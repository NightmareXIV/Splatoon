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
        void LayoutDrawElement(string i, string k)
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
                    if (ImGui.Button("Copy style##" + i + k))
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
                    if(ImGui.Combo("##elemselecttype" + i + k, ref el.type, Element.ElementTypes, Element.ElementTypes.Length))
                    {
                        if((el.type == 2 || el.type == 3) && el.radius == 0.35f)
                        {
                            el.radius = 0;
                        }
                    }
                    if (el.type == 1 || el.type == 3)
                    {
                        ImGui.SameLine();
                        ImGui.Checkbox("Account for rotation##rota" + i + k, ref el.includeRotation);
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
                            if (Svc.Targets.Target != null)
                            {
                                ImGui.SameLine();
                                if (ImGui.Button("Target##btarget" + i + k)) el.refActorName = Svc.Targets.Target.Name.ToString();
                            }
                            ImGuiEx.SizedText("", WidthElement);
                            ImGui.SameLine();
                            ImGui.Text("Targetability: ");
                            ImGui.SameLine();
                            ImGui.SetNextItemWidth(100f);
                            if(ImGui.BeginCombo($"##TargetabilityCombo{i+k}", el.onlyTargetable ? "Targetable" : (el.onlyUnTargetable ? "Untargetable" : "Any")))
                            {
                                if (ImGui.Selectable("Any"))
                                {
                                    el.onlyTargetable = false;
                                    el.onlyUnTargetable = false;
                                }
                                if (ImGui.Selectable("Targetable only"))
                                {
                                    el.onlyTargetable = true;
                                    el.onlyUnTargetable = false;
                                }
                                if (ImGui.Selectable("Untargetable only"))
                                {
                                    el.onlyTargetable = false;
                                    el.onlyUnTargetable = true;
                                }
                                ImGui.EndCombo();
                            }
                            ImGui.SameLine();
                            ImGui.Checkbox("Visible characters only##" + i + k, ref el.onlyVisible);
                            if (ImGui.IsItemHovered())
                            {
                                ImGui.SetTooltip("Setting this checkbox will also restrict search to characters ONLY. \n(character - is a player, companion or friendly/hostile NPC that can fight and have HP)");
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
                                    Svc.PluginInterface.UiBuilder.AddNotification("Unable to use for hidden element", "Splatoon", NotificationType.Error);
                                }
                            }
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
                    if ((el.type == 1 || el.type == 3) && el.includeRotation)
                    {
                        ImGui.SameLine();
                        ImGui.Text("Angle: " + RadToDeg(AngleBetweenVectors(0, 0, 10, 0, el.type == 1 ? 0 : el.refX, el.type == 1 ? 0 : el.refY, el.offX, el.offY)));
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
                                Svc.PluginInterface.UiBuilder.AddNotification("Unable to use for hidden element", "Splatoon", NotificationType.Error);
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
                    if (el.thicc > 0 || (el.type == 3 && el.includeRotation))
                    {
                        if (el.type != 2 && !(el.type == 3 && !el.includeRotation))
                        {
                            ImGuiEx.SizedText("Radius:", WidthElement);
                            ImGui.SameLine();
                            ImGui.SetNextItemWidth(60f);
                            ImGui.DragFloat("##radius" + i + k, ref el.radius, 0.01f, 0, float.MaxValue);
                            if (ImGui.IsItemHovered())
                                ImGui.SetTooltip("Leave at 0 to draw single dot");
                            if (el.type == 1 || (el.type == 3 && el.includeRotation))
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
                        }
                        if (el.type != 2 && el.type != 3)
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
