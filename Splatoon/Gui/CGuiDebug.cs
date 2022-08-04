using ECommons.GameFunctions;
using Splatoon.Memory;
using Splatoon.Utils;

namespace Splatoon
{
    unsafe partial class CGui
    {
        bool autoscrollLog = true;
        float s2wx, s2wy, s2wz, s2wrx, s2wry;
        bool s2wb = false;

        void DisplayDebug()
        {
            ImGui.BeginChild("##splatoonmaindbg");
            ImGui.Checkbox("Disable line fix", ref p.DisableLineFix);
            ImGuiEx.Text($"Line segments: {P.CurrentLineSegments}");
            var t = Environment.TickCount64 - p.CombatStarted;
            ImGuiEx.Text("CombatStarted = " + t);
            ImGui.SetNextItemWidth(60f);
            ImGui.DragInt($"Message concurrency", ref p.dequeueConcurrency, float.Epsilon);
            ImGui.Separator();
            if (Svc.ClientState.LocalPlayer != null)
            {
                var mypos = GetPlayerPositionXZY();
                ImGuiEx.Text("My pos XYZ: \n" + mypos.X + "\n" + mypos.Y + "\n" + mypos.Z);
                ImGuiEx.Text("Rotation: " + Svc.ClientState.LocalPlayer.Rotation);
                var tar = Svc.Targets.Target;
                if (tar != null)
                {
                    ImGuiEx.Text("Target pos XYZ: \n" + tar.GetPositionXZY().X + "\n" + tar.GetPositionXZY().Y + "\n" + tar.GetPositionXZY().Z);
                    ImGuiEx.Text("Rotation: " + tar.Rotation.ToString());
                    ImGuiEx.Text("Hitbox: " + tar.HitboxRadius.ToString());
                }
            }
            ImGui.Separator();
            ImGuiEx.Text("Query WorldToScreen");
            ImGui.PushItemWidth(50f);
            ImGui.DragFloat("##qs2wx", ref s2wx);
            ImGui.SameLine();
            ImGui.DragFloat("##qs2wy", ref s2wy);
            ImGui.SameLine();
            ImGui.DragFloat("##qs2wz", ref s2wz);
            ImGui.PopItemWidth();
            if (ImGui.Button("To my pos"))
            {
                var mypos = GetPlayerPositionXZY();
                s2wx = mypos.X;
                s2wy = mypos.Y;
                s2wz = mypos.Z;
            }
            ImGui.SameLine();
            if (ImGui.Button("Query"))
            {
                Safe(delegate
                {
                    s2wb = Svc.GameGui.WorldToScreen(new Vector3(s2wx, s2wz, s2wy), out Vector2 pos);
                    s2wrx = pos.X;
                    s2wry = pos.Y;
                });
            }
            ImGui.TextColored(ImGui.ColorConvertU32ToFloat4(s2wb ? Colors.Green : Colors.Red), "X:" + s2wrx + "\nY:" + s2wry);
            ImGui.Separator();
            if (Svc.ClientState.LocalPlayer != null)
            {
                ImGuiEx.Text($"Player+1 distance: {Vector3.Distance(Svc.ClientState.LocalPlayer.Position, Svc.ClientState.LocalPlayer.Position + new Vector3(1, 0, 0))}");
                ImGuiEx.Text($"Player+1+1 distance: {Vector3.Distance(Svc.ClientState.LocalPlayer.Position + new Vector3(1, 0, 0), Svc.ClientState.LocalPlayer.Position + new Vector3(2, 0, 0))}");
                Svc.GameGui.WorldToScreen(Svc.ClientState.LocalPlayer.Position, out var v1);
                Svc.GameGui.WorldToScreen(Svc.ClientState.LocalPlayer.Position + new Vector3(1, 0, 0), out var v2);
                Svc.GameGui.WorldToScreen(Svc.ClientState.LocalPlayer.Position + new Vector3(2, 0, 0), out var v3);
                Svc.GameGui.WorldToScreen(Svc.ClientState.LocalPlayer.Position + new Vector3(3, 0, 0), out var v4);
                Svc.GameGui.WorldToScreen(Svc.ClientState.LocalPlayer.Position + new Vector3(4, 0, 0), out var v5);
                ImGuiEx.Text($"Screen distance: {Vector2.Distance(v1, v2)}, {Vector2.Distance(v2, v3)}, {Vector2.Distance(v3, v4)}, {Vector2.Distance(v4, v5)}");
                ImGui.Separator();
            }
            ImGuiEx.Text("Camera angle X:" + p.CamAngleX);
            ImGuiEx.Text("Camera angle Y:" + p.CamAngleY);
            ImGuiEx.Text("Camera zoom:" + p.CamZoom);
            ImGui.Separator();
            ImGuiEx.Text("Object table:");
            ImGuiEx.Text("Name");
            ImGui.SameLine();
            ImGui.SetCursorPosX(200f);
            ImGuiEx.Text("Object ID");
            ImGui.SameLine();
            ImGui.SetCursorPosX(300f);
            ImGuiEx.Text($"Data ID");
            ImGui.SameLine();
            ImGui.SetCursorPosX(400f);
            ImGuiEx.Text($"Targetable");
            ImGui.SameLine();
            ImGui.SetCursorPosX(500f);
            ImGuiEx.Text($"Visible");
            ImGui.SameLine();
            ImGui.SetCursorPosX(600f);
            ImGuiEx.Text($"Model ID");
            foreach (var a in Svc.Objects)
            {
                Safe(delegate
                {
                    ImGuiEx.Text(a.Name.ToString());
                    ImGui.SameLine();
                    ImGui.SetCursorPosX(200f);
                    ImGuiEx.Text($"{a.ObjectId.Format()}");
                    ImGui.SameLine();
                    ImGui.SetCursorPosX(300f);
                    ImGuiEx.Text($"{a.DataId.Format()}");
                    ImGui.SameLine();
                    ImGui.SetCursorPosX(400f);
                    ImGuiEx.Text($"{a.Struct()->GetIsTargetable()}");
                    ImGui.SameLine();
                    ImGui.SetCursorPosX(500f);
                    ImGuiEx.Text($"{((a is Character chr) ? chr.IsCharacterVisible() : "Not a char")}");
                    ImGui.SameLine();
                    ImGui.SetCursorPosX(600f);
                    ImGuiEx.Text(a is Character chr2 ? $"{chr2.Struct()->ModelCharaId.Format()}" : "Not a char");
                });
            }
            ImGui.Separator();
            if (ImGui.CollapsingHeader("NameNpcID"))
            {
                foreach(var x in p.NameNpcIDs)
                {
                    ImGuiEx.Text($"{x.Key} = {x.Value}");
                }
            }
            if (ImGui.CollapsingHeader("CastInfos"))
            {
                foreach(var x in AttachedInfo.CastInfos)
                {
                    ImGuiEx.Text($"{x.Key} = {x.Value.ID}, {x.Value.StartTime}");
                }
            }
            ImGui.EndChild();
        }
    }
}
