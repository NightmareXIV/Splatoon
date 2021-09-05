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
using System.Web;
using System.Windows.Forms;

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
            var titleColored = false;
            if (p.isPvpZone)
            {
                ImGui.PushStyleColor(ImGuiCol.TitleBg, Colors.DarkRed);
                ImGui.PushStyleColor(ImGuiCol.TitleBgActive, Colors.DarkRed);
                titleColored = true;
            }
            if (ImGui.Begin("Splatoon", ref Open))
            {
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
            }
            ImGui.PopStyleVar();
            if (titleColored)
            {
                ImGui.PopStyleColor(2);
            }
            ImGui.EndTabBar();
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
            Clipboard.SetText(ImGui.GetIO().KeyAlt ? jsonraw : ImGui.GetIO().KeyCtrl ? HttpUtility.UrlEncode(json) : compressed.Length>base64.Length?base64:compressed);
        }

        private void HTTPExportToClipboard(Element el)
        {
            var l = JsonConvert.DeserializeObject<Element>(JsonConvert.SerializeObject(el)); ;
            l.Enabled = true;
            var json = JsonConvert.SerializeObject(l, Formatting.None, new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Ignore });
            var jsonraw = JsonConvert.SerializeObject(l, Formatting.Indented, new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Ignore });
            var compressed = json.Compress();
            var base64 = json.ToBase64UrlSafe();
            Clipboard.SetText(ImGui.GetIO().KeyAlt ? jsonraw : ImGui.GetIO().KeyCtrl ? HttpUtility.UrlEncode(json) : compressed.Length > base64.Length ? base64 : compressed);
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
                try
                {
                    json = Encoding.UTF8.GetString(Convert.FromBase64String(json));
                    p.Log("Import type: Base64");
                }
                catch (Exception e)
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
            catch (Exception e)
            {
                p.Log("Error importing: " + e.Message, true);
                p.Log(e.StackTrace);
            }
        }
    }
}
