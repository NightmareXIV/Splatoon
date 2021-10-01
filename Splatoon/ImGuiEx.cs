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
            if (width > s.X)
            {
                ImGui.SameLine();
                ImGui.Button("", new Vector2(width - s.X, 1f));
            }
            ImGui.PopStyleColor(3);
        }

        public static void TextCentered(string text)
        {
            /*ImGui.PushStyleColor(ImGuiCol.Button, Colors.Transparent);
            ImGui.PushStyleColor(ImGuiCol.ButtonHovered, Colors.Transparent);
            ImGui.PushStyleColor(ImGuiCol.ButtonActive, Colors.Transparent);*/
            var s = ImGui.CalcTextSize(text);
            //ImGui.Button("", new Vector2(ImGui.GetColumnWidth()/2f - s.X/2f, 1f));
            ImGui.SetCursorPosX(ImGui.GetColumnWidth() / 2f - s.X / 2f);
            //ImGui.PopStyleColor(3);
            //ImGui.SameLine();
            ImGui.Text(text);
        }

        static int StyleColors = 0;
        public static void ColorButton(uint color)
        {
            ImGui.PushStyleColor(ImGuiCol.Button, color);
            ImGui.PushStyleColor(ImGuiCol.ButtonHovered, color);
            ImGui.PushStyleColor(ImGuiCol.ButtonActive, color);
            StyleColors += 3;
        }

        public static void UncolorButton()
        {
            if (StyleColors == 0) return;
            ImGui.PopStyleColor(StyleColors);
            StyleColors = 0;
        }

        public static void DisplayColor(uint col)
        {
            var a = col.ToVector4();
            ImGui.ColorEdit4("", ref a, ImGuiColorEditFlags.NoInputs | ImGuiColorEditFlags.NoPicker);
        }
    }
}
