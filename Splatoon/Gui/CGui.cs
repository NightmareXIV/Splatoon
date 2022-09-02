﻿using Dalamud.Interface.Colors;
using Newtonsoft.Json;
using PInvoke;
using Splatoon.ConfigGui;
using Splatoon.Gui;
using Splatoon.Utils;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Web;

namespace Splatoon
{
    // Master class
    unsafe partial class CGui:IDisposable
    {
        Dictionary<uint, string> ActionNames;
        Dictionary<uint, string> BuffNames;
        const float WidthLayout = 150f;
        internal static float WidthElement
        {
            get
            {
                return 130f.Scale();
            }
        }
        internal const float WidthCombo = 200f;
        internal readonly Splatoon p;
        public bool Open = false;
        bool WasOpen = false;
        float RightWidth = 0;
        internal static GCHandle imGuiPayloadPtr;

        public CGui(Splatoon p)
        {
            this.p = p;
            Svc.PluginInterface.UiBuilder.Draw += Draw;
            ActionNames = Svc.Data.GetExcelSheet<Lumina.Excel.GeneratedSheets.Action>().ToDictionary(x => x.RowId, x => $"{x.RowId} | {x.Name}");
            BuffNames = Svc.Data.GetExcelSheet<Lumina.Excel.GeneratedSheets.Status>().ToDictionary(x => x.RowId, x => $"{x.RowId} | {x.Name}");
        }

        public void Dispose()
        {
            if (imGuiPayloadPtr.IsAllocated)
            {
                imGuiPayloadPtr.Free();
            }
            Svc.PluginInterface.UiBuilder.Draw -= Draw;
        }
                
        void Draw()
        {
            if (p.s2wInfo != null) return;
            if (!Open) 
            { 
                if(WasOpen)
                {
                    p.Config.Save();
                    WasOpen = false;
                    Notify.Success("Configuration saved");
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
            var ctspan = TimeSpan.FromMilliseconds(Environment.TickCount64 - p.CombatStarted);
            var title = $"Splatoon v{p.loader.splatoonVersion} | {(p.Zones.TryGetValue(Svc.ClientState.TerritoryType, out var t) ? p.Zones[Svc.ClientState.TerritoryType].PlaceName.Value.Name : "Terr: " + Svc.ClientState.TerritoryType)} | {(p.CombatStarted == 0?"Not in combat":$"Combat: {ctspan.Minutes:D2}{(ctspan.Milliseconds < 500?":":" ")}{ctspan.Seconds:D2} ({(int)ctspan.TotalSeconds}.{(ctspan.Milliseconds / 100):D1}s)")} | Phase: {p.Phase} | Layouts: {p.LayoutAmount} | Elements: {p.ElementAmount} | {GetPlayerPositionXZY().X:F1}, {GetPlayerPositionXZY().Y:F1}###Splatoon";
            if (ImGui.Begin(title, ref Open))
            {
                try
                {
                    if (ChlogGui.ChlogVersion > p.Config.ChlogReadVer)
                    {
                        ImGuiEx.Text("You may not change configuration until you have read changelog and closed window.");
                        if (ImGui.Button("Open changelog now"))
                        {
                            p.ChangelogGui.openLoggedOut = true;
                        }
                    }
                    else
                    {
                        var curCursor = ImGui.GetCursorPos();
                        ImGui.SetCursorPosX(ImGui.GetContentRegionAvail().X - RightWidth);
                        RightWidth = ImGuiEx.Measure(delegate
                        {
                            ImGui.SetNextItemWidth(100f);
                            if (ImGui.BeginCombo("##phaseSelector", $"Phase {p.Phase}"))
                            {
                                if (ImGui.Selectable("Phase 1 (doorboss)")) p.Phase = 1;
                                if (ImGui.Selectable("Phase 2 (post-doorboss)")) p.Phase = 2;
                                ImGuiEx.Text("Manual phase selection:");
                                ImGui.SameLine();
                                ImGui.SetNextItemWidth(30f);
                                ImGui.DragInt("##mPSel", ref p.Phase, 0.1f, 1, 9);
                                ImGui.EndCombo();
                            }
                        });
                        ImGui.SetCursorPos(curCursor);

                        ImGuiEx.EzTabBar("SplatoonSettings",
                            ("General", DisplayGeneralSettings, null, true),
                            ("Layouts", DislayLayouts, Colors.Green.ToVector4(), true),
                            ("Logger", DisplayLogger, null, true),
                            ("Explorer", Explorer.Draw, null, true),
                            ("Debug", DisplayDebug, null, true),
                            ("Log", DisplayLog, null, true),
                            ("Dynamic", DisplayDynamicElements, null, true),
                            ("Profiling", DisplayProfiling, null, true),
                            ("Contribute", Contribute.Draw, ImGuiColors.ParsedGold, true)

                            );
                    }
                }
                catch(Exception ex)
                {
                    ex.Log();
                    ImGuiEx.Text(ImGuiColors.DalamudRed, $"Error: {ex.Message}\n{ex.StackTrace}");
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
            foreach (var e in l.ElementsL) e.Enabled = true;
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
                var point = new POINT() { x = (int)screenPos.X, y = (int)screenPos.Y };
                //Chat.Print(point.X + "/" + point.Y);
                if (User32.ClientToScreen(Process.GetCurrentProcess().MainWindowHandle, ref point))
                {
                    //Chat.Print(point.X + "/" + point.Y);
                    User32.SetCursorPos(point.x, point.y);
                }
            }
        }
    }
}
