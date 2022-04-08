using Dalamud.Interface;
using Dalamud.Interface.Internal.Notifications;
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
using System.Web;

namespace Splatoon
{
    // Master class
    partial class CGui:IDisposable
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
            Svc.PluginInterface.UiBuilder.Draw += Draw;
        }

        public void Dispose()
        {
            Svc.PluginInterface.UiBuilder.Draw -= Draw;
        }

        [HandleProcessCorruptedStateExceptions]
        void Draw()
        {
            if (p.s2wInfo != null) return;
            if (!Open) 
            { 
                if(WasOpen)
                {
                    p.Config.Save();
                    WasOpen = false;
                    Notify("Configuration saved", NotificationType.Success);
                    if(p.Config.verboselog) p.Log("Configuration saved");
                }
                return;
            }
            else
            {
                if (!WasOpen)
                {
                    p.Config.Backup();
                }
                if(p.s2wInfo == null && Svc.PluginInterface.UiBuilder.FrameCount % 600 == 0)
                {
                    p.Config.Save();
                    p.Log("Configuration autosaved");
                }
            }
            WasOpen = true;
            ImGui.PushStyleVar(ImGuiStyleVar.WindowMinSize, new Vector2(700, 200));
            var titleColored = false;
            if (ImGui.Begin("Splatoon", ref Open))
            {
                if(p.MemoryManager.ErrorCode != 0)
                {
                    var cursor = ImGui.GetCursorPos();
                    var text = "Failsafe mode";
                    ImGui.SetCursorPosX(ImGui.GetContentRegionAvail().X - ImGui.CalcTextSize(text).X);
                    ImGui.TextColored((Environment.TickCount % 1000 < 500 ? Colors.Red : Colors.Yellow).ToVector4(), text);
                    ImGui.SetCursorPos(cursor);
                }
                ImGui.BeginTabBar("SplatoonSettings");
                if (ImGui.BeginTabItem("General settings"))
                {
                    DisplayGeneralSettings();
                    ImGui.EndTabItem();
                }
                ImGui.PushStyleColor(ImGuiCol.Text, Colors.Green);
                if (ImGui.BeginTabItem("Layouts"))
                {
                    ImGui.PopStyleColor();
                    DislayLayouts();
                    ImGui.EndTabItem();
                }
                else
                {
                    ImGui.PopStyleColor();
                }

                if (ImGui.BeginTabItem("Object logger"))
                {
                    DisplayLogger();
                    ImGui.EndTabItem();
                }
                if (ImGui.BeginTabItem("Debug"))
                {
                    DisplayDebug();
                    ImGui.EndTabItem();
                }
                if (ImGui.BeginTabItem("Log"))
                {
                    DisplayLog();
                    ImGui.EndTabItem();
                }
                if (ImGui.BeginTabItem("Dynamic elements"))
                {
                    DisplayDynamicElements();
                    ImGui.EndTabItem();
                }
                if (ImGui.BeginTabItem("Profiling"))
                {
                    DisplayProfiling();
                    ImGui.EndTabItem();
                }
            }
            ImGui.PopStyleVar();
            if (titleColored)
            {
                ImGui.PopStyleColor(2);
            }
            ImGui.EndTabBar();
            ImGui.End();
        }

        private void HTTPExportToClipboard(Layout el)
        {
            var l = JsonConvert.DeserializeObject<Layout>(JsonConvert.SerializeObject(el));
            l.Enabled = true;
            foreach (var e in l.Elements.Values) e.Enabled = true;
            var json = "~" + JsonConvert.SerializeObject(l, Formatting.None, new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Ignore });
            var jsonraw = "~" + JsonConvert.SerializeObject(l, Formatting.Indented, new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Ignore });
            var compressed = json.Compress();
            var base64 = json.ToBase64UrlSafe();
            ImGui.SetClipboardText(ImGui.GetIO().KeyAlt ? jsonraw : ImGui.GetIO().KeyCtrl ? HttpUtility.UrlEncode(json) : compressed.Length>base64.Length?base64:compressed);
        }

        private void HTTPExportToClipboard(Element el)
        {
            var l = JsonConvert.DeserializeObject<Element>(JsonConvert.SerializeObject(el)); ;
            l.Enabled = true;
            var json = JsonConvert.SerializeObject(l, Formatting.None, new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Ignore });
            var jsonraw = JsonConvert.SerializeObject(l, Formatting.Indented, new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Ignore });
            var compressed = json.Compress();
            var base64 = json.ToBase64UrlSafe();
            ImGui.SetClipboardText(ImGui.GetIO().KeyAlt ? jsonraw : ImGui.GetIO().KeyCtrl ? HttpUtility.UrlEncode(json) : compressed.Length > base64.Length ? base64 : compressed);
        }

        private void SetCursorTo(float refX, float refZ, float refY)
        {
            if (Svc.GameGui.WorldToScreen(new Vector3(refX, refZ, refY), out var screenPos))
            {
                var point = new Native.POINT() { X = (int)screenPos.X, Y = (int)screenPos.Y };
                //Chat.Print(point.X + "/" + point.Y);
                if (Native.ClientToScreen(Process.GetCurrentProcess().MainWindowHandle, ref point))
                {
                    //Chat.Print(point.X + "/" + point.Y);
                    Native.SetCursorPos(point.X, point.Y);
                }
            }
        }

        private void ImportFromText(string import)
        {
            try
            {
                if (import.Contains('~'))
                {
                    var name = import.Split('~')[0];
                    var json = import.Substring(name.Length + 1);
                    try
                    {
                        json = Encoding.UTF8.GetString(Convert.FromBase64String(json));
                        p.Log("Import type: Base64");
                    }
                    catch (Exception)
                    {
                        p.Log("Import type: JSON", true);
                    }
                    if (p.Config.Layouts.ContainsKey(name))
                    {
                        p.Log("Error: this name already exists", true);
                    }
                    else if (name.Length == 0)
                    {
                        p.Log("Error: name not present", true);
                    }
                    else if (name.Contains(","))
                    {
                        p.Log("Name can't contain reserved characters: ,", true);
                    }
                    else
                    {
                        p.Config.Layouts.Add(name, JsonConvert.DeserializeObject<Layout>(json));
                        import = "";
                    }
                }
                else
                {
                    p.Log("Import type: Legacy/Paisley Park/Waymark preset plugin", true);
                    var lp = JsonConvert.DeserializeObject<LegacyPreset>(import);
                    if (lp.Name == null || lp.Name == "") lp.Name = DateTimeOffset.Now.ToLocalTime().ToString().Replace(",", ".");
                    if(lp.A == null && lp.B == null && lp.C == null && lp.D == null &&
                        lp.One == null && lp.Two == null && lp.Three == null && lp.Four == null)
                    {
                        p.Log("Error importing: invalid data", true);
                    }
                    else if (p.Config.Layouts.ContainsKey("Legacy preset: " + lp.Name))
                    {
                        p.Log("Error: this name already exists", true);
                    }
                    else if (lp.Name.Contains(",") || lp.Name.Contains("~"))
                    {
                        p.Log("Name can't contain reserved characters: , and ~", true);
                    }
                    else
                    {
                        Layout l = new Layout()
                        {
                            ZoneLockH = new HashSet<ushort>() { Svc.ClientState.TerritoryType }
                        };
                        if (lp.A != null && lp.A.Active) l.Elements.Add("A", lp.A.ToElement("A", 0xff00ff00));
                        if (lp.B != null && lp.B.Active) l.Elements.Add("B", lp.B.ToElement("B", 0xff00ffff));
                        if (lp.C != null && lp.C.Active) l.Elements.Add("C", lp.C.ToElement("C", 0xffffff00));
                        if (lp.D != null && lp.D.Active) l.Elements.Add("D", lp.D.ToElement("D", 0xffff00ff));
                        if (lp.One != null && lp.One.Active) l.Elements.Add("1", lp.One.ToElement("1", 0xff00ff00));
                        if (lp.Two != null && lp.Two.Active) l.Elements.Add("2", lp.Two.ToElement("2", 0xff00ffff));
                        if (lp.Three != null && lp.Three.Active) l.Elements.Add("3", lp.Three.ToElement("3", 0xffffff00));
                        if (lp.Four != null && lp.Four.Active) l.Elements.Add("4", lp.Four.ToElement("4", 0xffff00ff));
                        p.Config.Layouts.Add("Legacy preset: " + lp.Name, l);
                        import = "";
                    }
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
