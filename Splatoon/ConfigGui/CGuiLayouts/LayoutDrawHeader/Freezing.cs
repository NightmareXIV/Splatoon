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
                ImGui.SetNextItemWidth(50f);
                ImGuiEx.Text("Freeze for:");
                ImGui.SameLine();
                ImGui.DragFloat("##freezeTime", ref layout.FreezeFor, 0.1f, 0, 99999, $"{layout.FreezeFor:F1}");
            }
        }
    }
}
