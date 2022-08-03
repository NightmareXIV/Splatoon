namespace Splatoon
{
    internal partial class CGui
    {
        internal static bool AddEmptyLayout(out Layout l)
        {
            if (NewLayoytName.Contains("~"))
            {
                Notify.Error("Name can't contain reserved characters: ~");
            }
            else if (NewLayoytName.Contains(","))
            {
                Notify.Error("Name can't contain reserved characters: ,");
            }
            else
            {
                l = new Layout()
                {
                    Name = CGui.NewLayoytName
                };
                if (Svc.ClientState != null) l.ZoneLockH.Add(Svc.ClientState.TerritoryType);
                P.Config.LayoutsL.Add(l);
                CGui.NewLayoytName = "";
                return true;
            }
            l = default;
            return false;
        }

        static void DrawRotationSelector(Element el, string i, string k)
        {
            ImGui.SameLine();
            ImGuiEx.Text("Add angle:");
            ImGui.SameLine();
            var angleDegrees = el.AdditionalRotation.RadiansToDegrees();
            ImGui.SameLine();
            ImGui.SetNextItemWidth(50f);
            ImGui.DragFloat("##ExtraAngle" + i + k, ref angleDegrees, 0.1f, 0f, 360f);
            if (ImGui.IsItemHovered()) ImGui.SetTooltip("Hold shift for faster changing;\ndouble-click to enter manually.");
            if (angleDegrees < 0f || angleDegrees > 360f) angleDegrees = 0f;
            el.AdditionalRotation = angleDegrees.DegreesToRadians();
            if (el.type != 1)
            {
                ImGui.SameLine();
                ImGui.Checkbox("Face me##" + i + k, ref el.FaceMe);
            }
        }

        void DrawVector3Selector(string lbl, Point3 point3, Layout l, Element e, bool text = true)
        {
                SImGuiEx.SizedText(text?lbl:"", WidthElement);
                ImGui.SameLine();
            ImGui.SetNextItemWidth(200f);
            var vec = point3.ToVector3();
            if(ImGui.DragFloat3($"##{lbl}", ref vec))
            {
                point3.X = vec.X;
                point3.Y = vec.Y;
                point3.Z = vec.Z;
            }
            ImGui.SameLine();

            if (ImGui.Button($"0 0 0##{lbl}"))
            {
                point3.X = 0;
                point3.Y = 0;
                point3.Z = 0;
            }
            ImGui.SameLine();
            if (ImGui.Button($"My position##{lbl}"))
            {
                point3.X = GetPlayerPositionXZY().X;
                point3.Y = GetPlayerPositionXZY().Y;
                point3.Z = GetPlayerPositionXZY().Z;
            }
            ImGui.SameLine();
            if (ImGui.Button($"Screen2World##{lbl}"))
            {
                if (p.IsLayoutVisible(l) && e.Enabled)
                {
                    SetCursorTo(point3.X, point3.Z, point3.Y);
                    p.BeginS2W(point3, "X", "Y", "Z");
                }
                else
                {
                    Notify.Error("Unable to use for hidden element");
                }
            }
        }
    }
}
