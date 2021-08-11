using Dalamud.Game.ClientState.Actors.Types;
using Dalamud.Game.ClientState.Actors.Types.NonPlayer;
using Dalamud.Interface;
using ImGuiNET;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.ExceptionServices;
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
        internal readonly Splatoon p;
        public bool Open = false;
        string lname = "";
        string ename = "";
        string zlockf = "";
        bool zlockcur = false;
        string curEdit = null;
        bool enableDeletion = false;
        bool enableDeletionElement = false;
        bool WasOpen = false;
        string jobFilter = "";

        public CGui(Splatoon p)
        {
            this.p = p;
            p.pi.UiBuilder.OnBuildUi += Draw;
        }

        public void Dispose()
        {
            p.pi.UiBuilder.OnBuildUi -= Draw;
        }

        void UnsetS2W()
        {
            foreach (var l in p.Config.Layouts.Values)
            {
                foreach (var e in l.Elements.Values)
                {
                    e.screen2world = 0;
                }
            }
            p.S2WActive = false;
        }

        [HandleProcessCorruptedStateExceptions]
        void Draw()
        {
            if (p.S2WActive)
            {
                foreach (var l in p.Config.Layouts.Values)
                {
                    foreach (var e in l.Elements.Values)
                    {
                        if(e.screen2world != 0 && (!e.Enabled || !p.IsLayoutVisible(l)))
                        {
                            UnsetS2W();
                            break;
                        }
                    }
                }
            }
            if (p.S2WActive) return;
            if (!Open) 
            { 
                if(WasOpen)
                {
                    p.Config.Backup();
                    p.Config.Save();
                    WasOpen = false;
                    UnsetS2W();
                    p.Log("Configuration saved");
                }
                return;
            }
            WasOpen = true;
            ImGui.PushStyleVar(ImGuiStyleVar.WindowMinSize, new Vector2(700, 200));
            if (ImGui.Begin("Splatoon", ref Open))
            {
                /*if(p.CamAngleY > p.Config.maxcamY)
                {
                    ImGui.PushStyleColor(ImGuiCol.Text, Colors.Red);
                    ImGuiEx.TextCentered("Your camera settings prevent waymarks from displaying in your current camera position.");
                    ImGuiEx.TextCentered("Either lift your camera up or adjust camera settings in general settings below.");
                    ImGui.PopStyleColor();
                }*/
                if (ImGui.CollapsingHeader("General settings"))
                {

                    ImGuiEx.SizedText("Circle smoothness:", WidthLayout);
                    ImGui.SameLine();
                    ImGui.SetNextItemWidth(100f);
                    ImGui.DragInt("##circlesmoothness", ref p.Config.segments, 0.1f, 10, 150);
                    ImGui.SameLine();
                    ImGui.Text("(?)");
                    if (ImGui.IsItemHovered())
                        ImGui.SetTooltip("Higher - smoother circle, higher cpu usage");

                    ImGuiEx.SizedText("Drawing distance:", WidthLayout);
                    ImGui.SameLine();
                    ImGui.SetNextItemWidth(100f);
                    ImGui.DragFloat("##maxdistance", ref p.Config.maxdistance, 0.25f, 10f, 200f);
                    ImGui.SameLine();
                    ImGui.Text("(?)");
                    if (ImGui.IsItemHovered())
                        ImGui.SetTooltip("Only try to draw objects that are not \n" +
                            "further away from you than this value");

                    ImGuiEx.SizedText("Line segments:", WidthLayout);
                    ImGui.SameLine();
                    ImGui.SetNextItemWidth(100f);
                    ImGui.DragInt("##linesegments", ref p.Config.lineSegments, 0.1f, 10, 50);
                    ImGui.SameLine();
                    ImGui.Text("(?)");
                    if (ImGui.IsItemHovered())
                        ImGui.SetTooltip("Increase this if your lines stop drawing too far from the screen edges \n" +
                            "   or if line disappears when you are zoomed in and near it's edge. \n" +
                            "Increasing this setting SIGNIFICANTLY increases processing time.");
                    
                    /*ImGuiEx.SizedText("Draw only when Y camera rotation is lower than:", WidthLayout * 2);
                    ImGui.SameLine();
                    ImGui.SetNextItemWidth(150f);
                    ImGui.DragFloat("##camymax", ref p.Config.maxcamY, 0.005f, -1.48353f, 0.78540f, p.Config.maxcamY.ToString("0.#####"));
                    ImGui.SameLine();
                    if (ImGui.Button("Current: " + p.CamAngleY))
                    {
                        p.Config.maxcamY = p.CamAngleY;
                    }
                    ImGui.SameLine();
                    if (ImGui.Button("Default"))
                    {
                        p.Config.maxcamY = 0.05f;
                    }*/
                    if (ImGui.Button("Open debug window"))
                    {
                        p.DebugGui.Open = true;
                    }
                    ImGui.SameLine();
                    ImGui.Text("Contact developer:");
                    ImGui.SameLine();
                    if (ImGui.Button("Github"))
                    {
                        Process.Start("https://github.com/Eternita-S/Splatoon/issues");
                    }
                    ImGui.SameLine();
                    if (ImGui.Button("Discord"))
                    {
                        Clipboard.SetText("Eternita#4037");
                        p.pi.Framework.Gui.Chat.Print("[Splatoon] Contact me in discord: Eternita#4037. Discord ID copied into your clipboard.");
                    }

                }
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
                    else
                    {
                        var l = new Layout();
                        if(p.pi.ClientState != null) l.ZoneLockH.Add(p.pi.ClientState.TerritoryType);
                        p.Config.Layouts.Add(lname, l);
                        lname = "";
                    }
                }
                ImGui.SameLine();
                ImGui.Text("Import layout from:");
                ImGui.SameLine();
                if (ImGui.Button("clipboard"))
                {
                    try
                    {
                        ImportFromText(Clipboard.GetText());
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
                    if(curEdit == i)
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
                            ImGui.Checkbox("Prevent disabling with mass disabling commands##" + i, ref p.Config.Layouts[i].DisableDisabling);
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
                                ImGui.Text("-");
                                ImGui.SameLine();
                                ImGui.SetNextItemWidth(50f);
                                ImGui.DragInt("##bte" + i, ref p.Config.Layouts[i].BattleTimeEnd, 1f, p.Config.Layouts[i].BattleTimeBegin, 60 * 20);
                                ImGui.SameLine();
                                ImGui.Text(DateTimeOffset.FromUnixTimeSeconds(p.Config.Layouts[i].BattleTimeBegin).ToString("mm:ss") + " - " +
                                    DateTimeOffset.FromUnixTimeSeconds(p.Config.Layouts[i].BattleTimeEnd).ToString("mm:ss"));
                            }
                            if (p.Config.Layouts[i].Visibility == 3)
                            {
                                ImGuiEx.SizedText("Message trigger to hide:", WidthLayout);
                                ImGui.SameLine();
                                ImGui.SetNextItemWidth(WidthCombo);
                                ImGui.InputTextWithHint("##msghide" + i, "Case-insensitive (partial) message", ref p.Config.Layouts[i].MessageToWatchForEnd, 100);
                            }*/
                            var colorZLock = p.pi.ClientState?.TerritoryType != null
                                && p.Config.Layouts[i].ZoneLockH.Count != 0 
                                && !p.Config.Layouts[i].ZoneLockH.Contains(p.pi.ClientState.TerritoryType)
                                && DateTimeOffset.Now.ToUnixTimeMilliseconds() % 1000 < 500;
                            if (colorZLock) ImGui.PushStyleColor(ImGuiCol.Text, Colors.Red);
                            ImGuiEx.SizedText("Zone lock: ", WidthLayout);
                            ImGui.SameLine();
                            ImGui.SetNextItemWidth(WidthCombo);
                            p.Config.Layouts[i].ZoneLockH.RemoveWhere(el => !p.Zones.ContainsKey(el));
                            if (ImGui.BeginCombo("##zlk" + i, p.Config.Layouts[i].ZoneLockH.Count == 0 ? "All zones" :
                                p.Config.Layouts[i].ZoneLockH.Count == 1? p.Config.Layouts[i].ZoneLockH.First() + " / "
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
                                if(p.Config.Layouts[i].ZoneLockH.Contains(p.pi.ClientState.TerritoryType))
                                {
                                    ImGuiEx.ColorButton(Colors.Red);
                                }
                                if (ImGui.SmallButton("Current zone: " + p.pi.ClientState.TerritoryType + " / "
                                    + p.Zones[p.pi.ClientState.TerritoryType].PlaceName.Value.Name))
                                {
                                    Static.ToggleHashSet(ref p.Config.Layouts[i].ZoneLockH, p.pi.ClientState.TerritoryType);
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
                                        Static.ToggleHashSet(ref p.Config.Layouts[i].ZoneLockH, z.Key);
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
                            if(p.Config.Layouts[i].JobLock == 0)
                            {
                                jprev.Add("All jobs");
                            }
                            else
                            {
                                foreach(var k in p.Jobs)
                                {
                                    if(Bitmask.IsBitSet(p.Config.Layouts[i].JobLock, k.Key))
                                    {
                                        jprev.Add(k.Value);
                                    }
                                }
                            }
                            var colorJLock = p.pi.ClientState?.LocalPlayer?.ClassJob != null
                                && p.Config.Layouts[i].JobLock != 0
                                && !Bitmask.IsBitSet(p.Config.Layouts[i].JobLock, (int)p.pi.ClientState.LocalPlayer.ClassJob.Id)
                                && DateTimeOffset.Now.ToUnixTimeMilliseconds() % 1000 < 500;
                            if (colorJLock) ImGui.PushStyleColor(ImGuiCol.Text, Colors.Red);
                            ImGuiEx.SizedText("Job lock", WidthLayout);
                            ImGui.SameLine();
                            ImGui.SetNextItemWidth(WidthCombo);
                            if (ImGui.BeginCombo("##joblock"+i, jprev.Count<3?string.Join(", ", jprev): jprev.Count+" jobs"))
                            {
                                if (colorJLock) ImGui.PopStyleColor();
                                ImGui.InputTextWithHint("##joblockfltr"+i, "Filter", ref jobFilter, 100);
                                foreach(var k in p.Jobs)
                                {
                                    if (!k.Key.ToString().Contains(jobFilter) && !k.Value.Contains(jobFilter)) continue ;
                                    if (k.Key == 0) continue;
                                    var col = false;
                                    if(Bitmask.IsBitSet(p.Config.Layouts[i].JobLock, k.Key))
                                    {
                                        ImGui.PushStyleColor(ImGuiCol.Button, Colors.Red);
                                        ImGui.PushStyleColor(ImGuiCol.ButtonHovered, Colors.Red);
                                        col = true;
                                    }
                                    if (ImGui.SmallButton(k.Key + " / " + k.Value + "##selectjob" + i))
                                    {
                                        if(Bitmask.IsBitSet(p.Config.Layouts[i].JobLock, k.Key))
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
                                    el.refX = p.pi.ClientState.LocalPlayer.Position.X;
                                    el.refY = p.pi.ClientState.LocalPlayer.Position.Y;
                                    el.refZ = p.pi.ClientState.LocalPlayer.Position.Z;
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
                                    if (elcolored) { 
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
                                        ImGuiEx.SizedText("Element type:", WidthElement);
                                        ImGui.SameLine();
                                        ImGui.SetNextItemWidth(WidthCombo);
                                        ImGui.Combo("##elemselecttype" + i+k, ref el.type, Element.ElementTypes, Element.ElementTypes.Length);
                                        if (el.type == 0 || el.type == 2)
                                        {
                                            ImGuiEx.SizedText(el.type == 2? "Point A":"Reference position: ", WidthElement);
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
                                            if (ImGui.Button("My position##ref" + i + k))
                                            {
                                                el.refX = p.pi.ClientState.LocalPlayer.Position.X;
                                                el.refY = p.pi.ClientState.LocalPlayer.Position.Y;
                                                el.refZ = p.pi.ClientState.LocalPlayer.Position.Z;
                                            }
                                            ImGui.SameLine();
                                            if (ImGui.Button("0 0 0##ref" + i + k))
                                            {
                                                el.refX = 0;
                                                el.refY = 0;
                                                el.refZ = 0;
                                            }
                                            ImGui.SameLine();
                                            if(ImGui.Button("Screen2World##s2w1" + i + k))
                                            {
                                                if (p.IsLayoutVisible(p.Config.Layouts[i]) && el.Enabled/* && p.CamAngleY <= p.Config.maxcamY*/)
                                                {
                                                    UnsetS2W();
                                                    SetCursorTo(el.refX, el.refZ, el.refY);
                                                    el.screen2world = el.type == 0 ? 1 : 2;
                                                }
                                                else
                                                {
                                                    p.pi.Framework.Gui.Toast.ShowError("Unable to use for hidden element");
                                                }
                                            }
                                            ImGui.PopItemWidth();
                                        }
                                        else if(el.type == 1)
                                        {
                                            ImGuiEx.SizedText("Targeted actor: ", WidthElement);
                                            ImGui.SameLine();
                                            ImGui.SetNextItemWidth(WidthCombo);
                                            ImGui.Combo("##actortype" + i + k, ref el.refActorType, Element.ActorTypes, Element.ActorTypes.Length);
                                            if (el.refActorType == 0)
                                            {
                                                ImGui.SameLine();
                                                if (ImGui.Button("Copy settarget command##"+i+k))
                                                {
                                                    Clipboard.SetText("/splatoon settarget " + i + "~" + k);
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
                                                ImGui.Checkbox("Targetable only##"+i+k, ref el.onlyTargetable);
                                                if (p.pi.ClientState.Targets.CurrentTarget != null)
                                                {
                                                    ImGui.SameLine();
                                                    if(ImGui.Button("Target##btarget" + i + k)) el.refActorName = p.pi.ClientState.Targets.CurrentTarget.Name;
                                                }
                                            }
                                        }

                                        ImGuiEx.SizedText(el.type==2?"Point B":"Offset: ", WidthElement);
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
                                        if (el.type == 2)
                                        {
                                            ImGui.SameLine();
                                            if (ImGui.Button("My position##off" + i + k))
                                            {
                                                el.offX = p.pi.ClientState.LocalPlayer.Position.X;
                                                el.offY = p.pi.ClientState.LocalPlayer.Position.Y;
                                                el.offZ = p.pi.ClientState.LocalPlayer.Position.Z;
                                            }
                                        }
                                        ImGui.SameLine();
                                        if (ImGui.Button("0 0 0##off" + i + k))
                                        {
                                            el.offX = 0;
                                            el.offY = 0;
                                            el.offZ = 0;
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
                                                    p.pi.Framework.Gui.Toast.ShowError("Unable to use for hidden element");
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
                                            ImGui.Text("Thickness is set to 0: only text overlay will be drawn.");
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
                                                ImGui.Text("(?)");
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
                                if (elcolored)
                                {
                                    ImGui.PopStyleColor();
                                    elcolored = false;
                                }
                                var currentCursor = ImGui.GetCursorPos();
                                var text = Element.ElementTypes[el.type] + (el.type == 1 ? " ["+(el.refActorType == 0?el.refActorName:Element.ActorTypes[el.refActorType])+"]" : "");
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
            ImGui.PopStyleVar();
        }

        private void SetCursorTo(float refX, float refZ, float refY)
        {
            if (p.pi.Framework.Gui.WorldToScreen(new SharpDX.Vector3(refX, refZ, refY), out var screenPos))
            {
                var point = new Native.POINT() { X = (int)screenPos.X, Y = (int)screenPos.Y };
                //p.pi.Framework.Gui.Chat.Print(point.X + "/" + point.Y);
                if (Native.ClientToScreen(Process.GetCurrentProcess().MainWindowHandle, ref point))
                {
                    //p.pi.Framework.Gui.Chat.Print(point.X + "/" + point.Y);
                    Native.SetCursorPos(point.X, point.Y);
                }
            }
        }

        private void ImportFromText(string import)
        {
            try
            {
                var name = import.Split('~')[0];
                var json = import.Substring(name.Length + 1);
                if (p.Config.Layouts.ContainsKey(name))
                {
                    p.Log("Error: this name already exists", true);
                }
                else if (name.Length == 0)
                {
                    p.Log("Error: name not present", true);
                }
                else
                {
                    p.Config.Layouts.Add(name, JsonConvert.DeserializeObject<Layout>(json));
                    import = "";
                }
            }
            catch (Exception e)
            {
                p.Log("Error importing: " + e.Message, true);
                p.Log(e.StackTrace);
            }
        }
    }
}
