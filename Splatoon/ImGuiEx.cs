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
            ImGui.PushStyleColor(ImGuiCol.Button, Colors.Transparent);
            ImGui.PushStyleColor(ImGuiCol.ButtonHovered, Colors.Transparent);
            ImGui.PushStyleColor(ImGuiCol.ButtonActive, Colors.Transparent);
            var s = ImGui.CalcTextSize(text);
            ImGui.Text(text);
            ImGui.SameLine();
            if(width > s.X) ImGui.Button("", new Vector2(width-s.X, 1f));
            ImGui.PopStyleColor(3);
        }

        public static void TextCentered(string text)
        {

            ImGui.PushStyleColor(ImGuiCol.Button, Colors.Transparent);
            ImGui.PushStyleColor(ImGuiCol.ButtonHovered, Colors.Transparent);
            ImGui.PushStyleColor(ImGuiCol.ButtonActive, Colors.Transparent);
            var s = ImGui.CalcTextSize(text);
            ImGui.Button("", new Vector2(ImGui.GetColumnWidth()/2 - s.X/2, 1f));
            ImGui.PopStyleColor(3);
            ImGui.SameLine();
            ImGui.Text(text);
        }
    }
}
