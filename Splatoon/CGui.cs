using Dalamud.Game.ClientState.Actors.Types;
using ImGuiNET;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Splatoon
{
    class CGui:IDisposable
    {
        const float WidthLayout = 150f;
        const float WidthElement = 150f;
        const float WidthCombo = 200f;
        readonly Splatoon p;
        public bool Open = false;
        string lname = "";
        string ename = "";
        string zlockf = "";
        string curEdit = null;
        bool enableDeletion = false;
        bool enableDeletionElement = false;
        bool WasOpen = false;

        public CGui(Splatoon p)
        {
            this.p = p;
            p._pi.UiBuilder.OnBuildUi += Draw;
        }

        public void Dispose()
        {
            p._pi.UiBuilder.OnBuildUi -= Draw;
        }

        void Draw()
        {
            if (!Open) 
            { 
                if(WasOpen)
                {
                    p.Config.Save();
                    WasOpen = false;
                    p.Log("Configuration saved");
                }
                return;
            }
            WasOpen = true;
            ImGui.PushStyleVar(ImGuiStyleVar.WindowMinSize, new Vector2(680, 200));
            if (ImGui.Begin("Splatoon", ref Open))
            {
                ImGui.SetNextItemWidth(350f);
                ImGui.InputTextWithHint("##lname", "Unique layout name", ref lname, 100);
                lname.Trim();
                ImGui.SameLine();
                if(ImGui.Button("Add layout"))
                {
                    if (p.Config.Layouts.ContainsKey(lname))
                    {
                        p.Log("Error: this name already exists");
                    }
                    else if (lname.Length == 0)
                    {
                        p.Log("Error: you must name layout");
                    }
                    else if(lname.Contains("~"))
                    {
                        p.Log("Name can't contain reserved characters: ~");
                    }
                    else
                    {
                        p.Config.Layouts.Add(lname, new Layout());
                        lname = "";
                    }
                }
                ImGui.SameLine();
                if (ImGui.Button("Import layout from clipboard"))
                {
                    try
                    {
                        var import = Clipboard.GetText();
                        var name = import.Split('~')[0];
                        var json = import.Substring(name.Length + 1);
                        if (p.Config.Layouts.ContainsKey(name))
                        {
                            p.Log("Error: this name already exists");
                        }
                        else if (name.Length == 0)
                        {
                            p.Log("Error: name not present");
                        }
                        else
                        {
                            p.Config.Layouts.Add(name, JsonConvert.DeserializeObject<Layout>(json));
                            import = "";
                        }
                    }
                    catch(Exception e)
                    {
                        p.Log("Error importing: "+e.Message);
                        p.Log(e.StackTrace);
                    }
                }
                ImGui.SameLine();
                if (ImGui.Button("Debug"))
                {
                    p.DebugGui.Open = true;
                }
                ImGui.Checkbox("Allow layout deletion", ref enableDeletion);
                ImGui.SameLine();
                ImGui.Checkbox("Allow elements deletion", ref enableDeletionElement);
                ImGui.BeginChild("##layoutlist");
                var open = false;
                foreach (var i in p.Config.Layouts.Keys.ToArray())
                {
                    var colored = false;
                    if(!p.Config.Layouts[i].Enabled)
                    {
                        colored = true;
                        ImGui.PushStyleColor(ImGuiCol.Text, Colors.Gray);
                    }
                    else if (p.Config.Layouts[i].DisableDisabling)
                    {
                        colored = true;
                        ImGui.PushStyleColor(ImGuiCol.Text, Colors.Orange);
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
                            ImGui.Checkbox("Prevent disabling with commands##" + i, ref p.Config.Layouts[i].DisableDisabling);
                            if (ImGui.Button("Export to clipboard"))
                            {
                                Clipboard.SetText(i + "~" + JsonConvert.SerializeObject(p.Config.Layouts[i], Formatting.None, new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Ignore }));
                            }
                            ImGui.SameLine();
                            if (ImGui.Button("Copy enable command"))
                            {
                                Clipboard.SetText("/splatoon enable " + i);
                            }
                            ImGui.SameLine();
                            if (ImGui.Button("Copy disable command"))
                            {
                                Clipboard.SetText("/splatoon disable " + i);
                            }
                            ImGuiEx.SizedText("Display conditions:", WidthLayout);
                            ImGui.SameLine();
                            ImGui.SetNextItemWidth(WidthCombo);
                            ImGui.Combo("##dcn" + i, ref p.Config.Layouts[i].DCond, Layout.DisplayConditions, Layout.DisplayConditions.Length);
                            ImGuiEx.SizedText("Visibility of layout:", WidthLayout);
                            ImGui.SameLine();
                            ImGui.SetNextItemWidth(WidthCombo);
                            ImGui.Combo("##vsb" + i, ref p.Config.Layouts[i].Visibility, Layout.VisibilityType, Layout.VisibilityType.Length);
                            if (p.Config.Layouts[i].Visibility == 1 || p.Config.Layouts[i].Visibility == 2)
                            {
                                ImGuiEx.SizedText("Visibility time:", WidthLayout);
                                ImGui.SameLine();
                                ImGui.SetNextItemWidth(50f);
                                ImGui.DragInt("##btbg" + i, ref p.Config.Layouts[i].BattleTimeBegin, 1f, 0, 60 * 20);
                                ImGui.SameLine();
                                ImGui.Text("-");
                                ImGui.SameLine();
                                ImGui.SetNextItemWidth(50f);
                                ImGui.DragInt("##bte" + i, ref p.Config.Layouts[i].BattleTimeEnd, 1f, p.Config.Layouts[i].BattleTimeBegin, 60 * 20);
                                ImGui.SameLine();
                                ImGui.Text(DateTimeOffset.FromUnixTimeSeconds(p.Config.Layouts[i].BattleTimeBegin).ToString("mm:ss") +" - "+
                                    DateTimeOffset.FromUnixTimeSeconds(p.Config.Layouts[i].BattleTimeEnd).ToString("mm:ss"));
                            }
                            if (p.Config.Layouts[i].Visibility == 2 || p.Config.Layouts[i].Visibility == 3)
                            {
                                ImGuiEx.SizedText("Message:", WidthLayout);
                                ImGui.SameLine();
                                ImGui.SetNextItemWidth(WidthCombo);
                                ImGui.InputTextWithHint("##acname" + i, "Case-insensitive (partial) message", ref p.Config.Layouts[i].MessageToWatch, 100);
                            }
                            ImGuiEx.SizedText("Zone lock: ", WidthLayout);
                            ImGui.SameLine();
                            ImGui.SetNextItemWidth(WidthCombo);
                            if (ImGui.BeginCombo("##zlk" + i, p.Config.Layouts[i].ZoneLock == 0 ? "All zones" : p.Config.Layouts[i].ZoneLock + " / "
                                + p.Zones[p.Config.Layouts[i].ZoneLock].PlaceName.Value.Name))
                            {
                                ImGui.InputTextWithHint("##zfltr" + i, "Filter", ref zlockf, 100);
                                if (ImGui.Selectable("All zones"))
                                {
                                    p.Config.Layouts[i].ZoneLock = 0;
                                }
                                ImGui.PushStyleColor(ImGuiCol.Text, 0xff00ffff);
                                if (ImGui.Selectable("Current zone: " + p._pi.ClientState.TerritoryType + " / "
                                    + p.Zones[p._pi.ClientState.TerritoryType].PlaceName.Value.Name))
                                {
                                    p.Config.Layouts[i].ZoneLock = p._pi.ClientState.TerritoryType;
                                }
                                ImGui.PopStyleColor();
                                foreach (var z in p.Zones)
                                {
                                    if (z.Value.PlaceName.Value.Name.Length == 0) continue;
                                    var s = z.Key + " / " + z.Value.PlaceName.Value.Name;
                                    if (!s.ToLower().Contains(zlockf)) continue;
                                    if (ImGui.Selectable(s))
                                    {
                                        p.Config.Layouts[i].ZoneLock = z.Key;
                                    }
                                }
                                ImGui.EndCombo();
                            }
                            ImGui.PushItemWidth(WidthCombo);
                            ImGui.InputTextWithHint("##elnameadd" + i, "Unique element name", ref ename, 100);
                            ImGui.PopItemWidth();
                            ImGui.SameLine();
                            if (ImGui.Button("Add element##addelement" + i))
                            {
                                if (p.Config.Layouts[i].Elements.ContainsKey(ename))
                                {
                                    p.Log("Error: this name already exists");
                                }
                                else if (ename.Length == 0)
                                {
                                    p.Log("Error: you must name layout");
                                }
                                else
                                {
                                    var el = new Element(0);
                                    el.refX = p._pi.ClientState.LocalPlayer.Position.X;
                                    el.refY = p._pi.ClientState.LocalPlayer.Position.Y;
                                    el.refZ = p._pi.ClientState.LocalPlayer.Position.Z;
                                    p.Config.Layouts[i].Elements.Add(ename, el);
                                    ename = "";
                                }
                            }
                            foreach (var k in p.Config.Layouts[i].Elements.Keys.ToArray())
                            {
                                var el = p.Config.Layouts[i].Elements[k];
                                if (ImGui.CollapsingHeader(i + " / " + k + "##elem" + i + k))
                                {
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
                                        ImGuiEx.SizedText("Element type:", WidthElement);
                                        ImGui.SameLine();
                                        ImGui.SetNextItemWidth(WidthCombo);
                                        ImGui.Combo("##elemselecttype" + i+k, ref el.type, Element.ElementTypes, Element.ElementTypes.Length);
                                        if (el.type == 0)
                                        {
                                            ImGuiEx.SizedText("Reference position: ", WidthElement);
                                            ImGui.SameLine();
                                            ImGui.PushItemWidth(60f);
                                            ImGui.Text("X:");
                                            ImGui.SameLine();
                                            ImGui.DragFloat("##refx" + i + k, ref el.refX, 0.02f, float.MinValue, float.MaxValue);
                                            ImGui.SameLine();
                                            ImGui.Text("Y:");
                                            ImGui.SameLine();
                                            ImGui.DragFloat("##refy" + i + k, ref el.refY, 0.02f, float.MinValue, float.MaxValue);
                                            ImGui.SameLine();
                                            ImGui.Text("Z:");
                                            ImGui.SameLine();
                                            ImGui.DragFloat("##refz" + i + k, ref el.refZ, 0.02f, float.MinValue, float.MaxValue);
                                            ImGui.SameLine();
                                            if (ImGui.Button("Set to my position##ref" + i + k))
                                            {
                                                el.refX = p._pi.ClientState.LocalPlayer.Position.X;
                                                el.refY = p._pi.ClientState.LocalPlayer.Position.Y;
                                                el.refZ = p._pi.ClientState.LocalPlayer.Position.Z;
                                            }
                                            ImGui.SameLine();
                                            if (ImGui.Button("Set to 0 0 0##ref" + i + k))
                                            {
                                                el.refX = 0;
                                                el.refY = 0;
                                                el.refZ = 0;
                                            }
                                            ImGui.PopItemWidth();
                                        }
                                        else if(el.type == 1)
                                        {
                                            ImGuiEx.SizedText("Targeted actor: ", WidthElement);
                                            ImGui.SameLine();
                                            ImGui.SetNextItemWidth(WidthCombo);
                                            ImGui.Combo("##actortype" + i + k, ref el.refActorType, Element.ActorTypes, Element.ActorTypes.Length);
                                            if(el.refActorType == 0)
                                            {
                                                ImGui.SameLine();
                                                ImGui.SetNextItemWidth(WidthCombo);
                                                ImGui.InputTextWithHint("##actorname" + i + k, "Case-insensitive (partial) name", ref el.refActorName, 100);
                                                if (p._pi.ClientState.Targets.CurrentTarget != null && p._pi.ClientState.Targets.CurrentTarget is PlayerCharacter)
                                                {
                                                    ImGui.SameLine();
                                                    if(ImGui.Button("Target##btarget" + i + k)) el.refActorName = p._pi.ClientState.Targets.CurrentTarget.Name;
                                                }
                                            }
                                        }

                                        ImGuiEx.SizedText("Offset: ", WidthElement);
                                        ImGui.SameLine();
                                        ImGui.PushItemWidth(60f);
                                        ImGui.Text("X:");
                                        ImGui.SameLine();
                                        ImGui.DragFloat("##offx" + i + k, ref el.offX, 0.02f, float.MinValue, float.MaxValue);
                                        ImGui.SameLine();
                                        ImGui.Text("Y:");
                                        ImGui.SameLine();
                                        ImGui.DragFloat("##offy" + i + k, ref el.offY, 0.02f, float.MinValue, float.MaxValue);
                                        ImGui.SameLine();
                                        ImGui.Text("Z:");
                                        ImGui.SameLine();
                                        ImGui.DragFloat("##offz" + i + k, ref el.offZ, 0.02f, float.MinValue, float.MaxValue);
                                        ImGui.SameLine();
                                        if (ImGui.Button("Set to 0 0 0##off" + i + k))
                                        {
                                            el.offX = 0;
                                            el.offY = 0;
                                            el.offZ = 0;
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
                                            ImGui.Text("Thickness is set to 0: only text overlay will be drawn.");
                                        }
                                        if (el.thicc > 0)
                                        {
                                            ImGuiEx.SizedText("Radius:", WidthElement);
                                            ImGui.SameLine();
                                            ImGui.SetNextItemWidth(60f);
                                            ImGui.DragFloat("##radius" + i + k, ref el.radius, 0.01f, 0, float.MaxValue);
                                            if (el.type == 1)
                                            {
                                                ImGui.SameLine();
                                                ImGui.Checkbox("+hitbox", ref el.includeHitbox);
                                            }
                                            ImGui.SameLine();
                                            ImGui.Text("Leave at 0 to draw a single dot");
                                        }
                                        ImGuiEx.SizedText("Overlay text:", WidthElement);
                                        ImGui.SameLine();
                                        ImGui.SetNextItemWidth(150f);
                                        ImGui.InputTextWithHint("##overlaytext" + i + k, "Text to display as overlay", ref el.overlayText, 30);
                                        if (el.overlayText.Length > 0)
                                        {
                                            ImGui.SameLine();
                                            ImGui.Text("Vertical offset:");
                                            ImGui.SameLine();
                                            ImGui.SetNextItemWidth(60f);
                                            ImGui.DragFloat("##vtextadj" + i + k, ref el.overlayVOffset, 0.02f);
                                            ImGui.SameLine();
                                            ImGui.Text("BG color:");
                                            ImGui.SameLine();
                                            var v4b = ImGui.ColorConvertU32ToFloat4(el.overlayBGColor);
                                            if (ImGui.ColorEdit4("##colorbuttonbg" + i + k, ref v4b, ImGuiColorEditFlags.NoInputs))
                                            {
                                                el.overlayBGColor = ImGui.ColorConvertFloat4ToU32(v4b);
                                            }
                                            ImGui.PopItemWidth();
                                            ImGui.SameLine();
                                            ImGui.Text("Text color:");
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
            ImGui.PopStyleVar();
        }
    }
}
