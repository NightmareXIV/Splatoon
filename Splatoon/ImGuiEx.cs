using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Splatoon
{
    class ImGuiEx //came here to laugh on how scuffed it is? let's do so together.
    {
        public static void SizedText(string text, float width)
        {
            ImGui.PushStyleColor(ImGuiCol.Button, 0x00000000);
            ImGui.PushStyleColor(ImGuiCol.ButtonHovered, 0x00000000);
            ImGui.PushStyleColor(ImGuiCol.ButtonActive, 0x00000000);
            var s = ImGui.CalcTextSize(text);
            ImGui.Text(text);
            ImGui.SameLine();
            if(width > s.X) ImGui.Button("", new Vector2(width-s.X, 1f));
            ImGui.PopStyleColor(3);
        }
    }
}
