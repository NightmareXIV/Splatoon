using Dalamud.Interface.Internal.Notifications;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Splatoon
{
    static class ImGuiEx //came here to laugh on how scuffed it is? let's do so together.
    {
        internal static float Measure(Action func)
        {
            var pos = ImGui.GetCursorPosX();
            func();
            ImGui.SameLine(0,0);
            var diff = ImGui.GetCursorPosX() - pos;
            ImGui.Dummy(Vector2.Zero);
            return diff;
        }

        public static void InputHex(string name, ref uint hexInt)
        {
            var text = $"{hexInt:X}";
            if(ImGui.InputText(name, ref text, 8))
            {
                if(uint.TryParse(text, NumberStyles.HexNumber, null, out var num))
                {
                    hexInt = num;
                }
            }
        }

        public static void TextCopy(string text)
        {
            ImGui.TextUnformatted(text);
            if (ImGui.IsItemHovered())
            {
                ImGui.SetMouseCursor(ImGuiMouseCursor.Hand);
            }
            if (ImGui.IsItemClicked(ImGuiMouseButton.Left))
            {
                ImGui.SetClipboardText(text);
                Svc.PluginInterface.UiBuilder.AddNotification("Text copied to clipboard", "Splatoon", NotificationType.Success);
            }
        }
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

        public static void GSameLine(Action a, out float cursorPosX, bool end = false)
        {
            var upperTextCursor = ImGui.GetCursorPos();
            a();
            if (!end)
            {
                ImGui.SameLine();
                upperTextCursor.X = ImGui.GetCursorPosX();
                ImGui.Text("");
                ImGui.SetCursorPos(upperTextCursor);
            }
            cursorPosX = upperTextCursor.X;
        }

        static public void EnumCombo<T>(string name, ref T refConfigField, string[] overrideNames = null) where T : IConvertible
        {
            var values = overrideNames ?? Enum.GetValues(typeof(T)).Cast<T>().Select(x => x.ToString().Replace("_", " ")).ToArray();
            var num = Convert.ToInt32(refConfigField);
            ImGui.Combo(name, ref num, values, values.Length);
            refConfigField = Enum.GetValues(typeof(T)).Cast<T>().ToArray()[num];
        }
    }
}
