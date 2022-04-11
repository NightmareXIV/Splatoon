using Dalamud.Interface.Internal.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splatoon
{
    internal partial class CGui
    {
        void DrawVector3Selector(string lbl, ref Point3 vec3, Layout l, Element e)
        {
            ImGuiEx.SizedText(lbl, WidthElement);
            ImGui.SameLine();
            ImGui.SetNextItemWidth(200f);
            var vec = vec3.ToVector3();
            if(ImGui.DragFloat3($"##{lbl}", ref vec))
            {
                vec3 = vec.ToPoint3();
            }
            ImGui.SameLine();

            if (ImGui.Button($"0 0 0##{lbl}"))
            {
                vec3 = new();
            }
            ImGui.SameLine();
            if (ImGui.Button($"My position##{lbl}"))
            {
                vec3.X = GetPlayerPositionXZY().X;
                vec3.Y = GetPlayerPositionXZY().Y;
                vec3.Z = GetPlayerPositionXZY().Z;
            }
            ImGui.SameLine();
            if (ImGui.Button($"Screen2World##{lbl}"))
            {
                if (p.IsLayoutVisible(l) && e.Enabled)
                {
                    SetCursorTo(vec3.X, vec3.Z, vec3.Y);
                    p.BeginS2W(vec3, "X", "Y", "Z");
                }
                else
                {
                    Notify("Unable to use for hidden element", NotificationType.Error);
                }
            }
        }
    }
}
