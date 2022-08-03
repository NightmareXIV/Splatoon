using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splatoon.ConfigGui.CGuiLayouts.LayoutDrawHeader.Subcommands
{
    internal static class ZlockSelector
    {
        internal static string zlockf = "";
        internal static bool zlockcur = false;
        internal static void DrawZlockSelector(this Layout layout)
        {
            var colorZLock = Svc.ClientState?.TerritoryType != null
                && layout.ZoneLockH.Count != 0
                && !layout.ZoneLockH.Contains(Svc.ClientState.TerritoryType)
                && Environment.TickCount64 % 1000 < 500;
            if (colorZLock) ImGui.PushStyleColor(ImGuiCol.Text, Colors.Red);
            layout.ZoneLockH.RemoveWhere(el => !P.Zones.ContainsKey(el));
            ImGuiEx.SetNextItemFullWidth();
            if (ImGui.BeginCombo("##zlk", layout.ZoneLockH.Count == 0 ? "All zones" :
                layout.ZoneLockH.Count == 1 ? layout.ZoneLockH.First() + " / "
                + P.Zones[layout.ZoneLockH.First()].PlaceName.Value.Name :
                layout.ZoneLockH.Count + " zones"
                ))
            {
                if (colorZLock) ImGui.PopStyleColor();
                ImGui.SetNextItemWidth(100f);
                ImGui.InputTextWithHint("##zfltr", "Filter", ref zlockf, 100);
                ImGui.SameLine();
                ImGui.Checkbox("Only selected", ref zlockcur);
                if (P.Zones.ContainsKey(Svc.ClientState.TerritoryType))
                {
                    ImGui.PushStyleColor(ImGuiCol.Text, Colors.Yellow);
                    if (layout.ZoneLockH.Contains(Svc.ClientState.TerritoryType))
                    {
                        SImGuiEx.ColorButton(Colors.Red);
                    }
                    string zcfc = P.Zones[Svc.ClientState.TerritoryType].ContentFinderCondition?.Value.Name?.ToString();
                    if (P.Zones.ContainsKey(Svc.ClientState.TerritoryType) && ImGui.SmallButton("Current zone: " + Svc.ClientState.TerritoryType + " / "
                        + P.Zones[Svc.ClientState.TerritoryType].PlaceName.Value.Name +
                        (string.IsNullOrEmpty(zcfc) ? "" : $" ({zcfc})")))
                    {
                        layout.ZoneLockH.Toggle(Svc.ClientState.TerritoryType);
                    }
                    SImGuiEx.UncolorButton();
                    ImGui.PopStyleColor();
                }
                foreach (var z in P.Zones)
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
        }
    }
}
