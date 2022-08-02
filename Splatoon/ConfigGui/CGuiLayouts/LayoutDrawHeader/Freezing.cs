using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splatoon.ConfigGui.CGuiLayouts.LayoutDrawHeader
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
            }
        }
    }
}
