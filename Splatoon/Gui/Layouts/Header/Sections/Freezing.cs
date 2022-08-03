namespace Splatoon.ConfigGui.CGuiLayouts.LayoutDrawHeader.Subcommands
{
    internal static class Freezing
    {
        internal static void DrawFreezing(this Layout layout)
        {
            if (layout.Freezing)
            {
                ImGuiEx.Text("Freeze for:");
                ImGui.SameLine();
                ImGui.SetNextItemWidth(50f);
                ImGui.DragFloat("##freezeTime", ref layout.FreezeFor, 0.1f, 0, 99999, $"{layout.FreezeFor:F1}");
                ImGui.SameLine();
                ImGuiEx.Text("Refreeze interval:");
                ImGui.SameLine();
                ImGui.SetNextItemWidth(50f);
                ImGui.DragFloat("##freezeInt", ref layout.IntervalBetweenFreezes, 0.1f, 0, 99999, $"{layout.IntervalBetweenFreezes:F1}");
                ImGuiEx.Text("Reset on:");
                ImGui.SameLine();
                ImGui.Checkbox("Combat end", ref layout.FreezeResetCombat);
                ImGui.SameLine();
                ImGui.Checkbox("Zone change", ref layout.FreezeResetTerr);
            }
        }
    }
}
