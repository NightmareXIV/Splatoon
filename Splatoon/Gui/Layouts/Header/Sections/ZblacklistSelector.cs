using Splatoon.Utils;

namespace Splatoon.ConfigGui.CGuiLayouts.LayoutDrawHeader.Subcommands
{
    internal static class ZblacklistSelector
    {
        internal static string zblacklistf = "";
        internal static bool zblacklistcur = false;
        internal static void DrawZblacklistSelector(this Layout layout)
        {
            var colorZBlacklist = Svc.ClientState?.TerritoryType != null
                && layout.ZoneBlacklistH.Count != 0
                && !layout.ZoneBlacklistH.Contains(Svc.ClientState.TerritoryType)
                && Environment.TickCount64 % 1000 < 500;
            if (colorZBlacklist) ImGui.PushStyleColor(ImGuiCol.Text, Colors.Red);
            layout.ZoneBlacklistH.RemoveWhere(el => !P.Zones.ContainsKey(el));
            ImGuiEx.SetNextItemFullWidth();
            if (ImGui.BeginCombo("##zblklst", layout.ZoneBlacklistH.Count == 0 ? "All zones" :
                layout.ZoneBlacklistH.Count == 1 ? layout.ZoneBlacklistH.First() + " / "
                + P.Zones[layout.ZoneBlacklistH.First()].PlaceName.Value.Name :
                layout.ZoneBlacklistH.Count + " zones"
                ))
            {
                if (colorZBlacklist) ImGui.PopStyleColor();
                ImGui.SetNextItemWidth(100f);
                ImGui.InputTextWithHint("##zblklst", "Blacklist", ref zblacklistf, 100);
                ImGui.SameLine();
                ImGui.Checkbox("Only selected", ref zblacklistcur);
                if (P.Zones.ContainsKey(Svc.ClientState.TerritoryType))
                {
                    ImGui.PushStyleColor(ImGuiCol.Text, Colors.Yellow);
                    if (layout.ZoneBlacklistH.Contains(Svc.ClientState.TerritoryType))
                    {
                        SImGuiEx.ColorButton(Colors.Red);
                    }
                    string zcfc = P.Zones[Svc.ClientState.TerritoryType].ContentFinderCondition?.Value.Name?.ToString();
                    if (P.Zones.ContainsKey(Svc.ClientState.TerritoryType) && ImGui.SmallButton("Current zone: " + Svc.ClientState.TerritoryType + " / "
                        + P.Zones[Svc.ClientState.TerritoryType].PlaceName.Value.Name +
                        (string.IsNullOrEmpty(zcfc) ? "" : $" ({zcfc})")))
                    {
                        layout.ZoneBlacklistH.Toggle(Svc.ClientState.TerritoryType);
                    }
                    SImGuiEx.UncolorButton();
                    ImGui.PopStyleColor();
                }
                foreach (var z in P.Zones)
                {
                    string azcfc = z.Value.ContentFinderCondition?.Value.Name?.ToString();
                    if (z.Value.PlaceName.Value.Name.ToString().Length == 0) continue;
                    var s = z.Key + " / " + z.Value.PlaceName.Value.Name + (string.IsNullOrEmpty(azcfc) ? "" : $" ({azcfc})");
                    if (!s.ToLower().Contains(zblacklistf)) continue;
                    if (zblacklistcur && !layout.ZoneBlacklistH.Contains(z.Key)) continue;
                    if (layout.ZoneBlacklistH.Contains(z.Key))
                    {
                        SImGuiEx.ColorButton(Colors.Red);
                    }
                    if (ImGui.SmallButton(s))
                    {
                        layout.ZoneBlacklistH.Toggle(z.Key);
                    }
                    SImGuiEx.UncolorButton();
                }
                ImGui.EndCombo();
            }
            else
            {
                if (colorZBlacklist) ImGui.PopStyleColor();
            }
        }
    }
}
