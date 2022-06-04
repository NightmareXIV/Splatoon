﻿using Dalamud.Interface.Internal.Notifications;

namespace Splatoon
{
    partial class CGui
    {
        bool autoscrollLog = true;
        float s2wx, s2wy, s2wz, s2wrx, s2wry;
        bool s2wb = false;

        void DisplayDebug()
        {
            ImGui.BeginChild("##splatoonmaindbg");
            ImGui.Checkbox("Disable line fix", ref p.DisableLineFix);
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
                    s2wb = p.MemoryManager.WorldToScreen(new Vector3(s2wx, s2wz, s2wy), out Vector2 pos);
                    s2wrx = pos.X;
                    s2wry = pos.Y;
                });
            }
            ImGui.TextColored(ImGui.ColorConvertU32ToFloat4(s2wb ? Colors.Green : Colors.Red), "X:" + s2wrx + "\nY:" + s2wry);
            ImGui.Separator();
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
                    ImGuiEx.Text($"{a.ObjectId:X8}");
                    ImGui.SameLine();
                    ImGui.SetCursorPosX(300f);
                    ImGuiEx.Text($"{a.DataId:X8}");
                    ImGui.SameLine();
                    ImGui.SetCursorPosX(400f);
                    ImGuiEx.Text($"{p.MemoryManager.GetIsTargetable((a is Character ch) ? ch : a)}");
                    ImGui.SameLine();
                    ImGui.SetCursorPosX(500f);
                    ImGuiEx.Text($"{((a is Character chr) ? p.MemoryManager.GetIsVisible(chr) : "Not a char")}");
                    ImGui.SameLine();
                    ImGui.SetCursorPosX(600f);
                    ImGuiEx.Text(a is Character chr2 ? $"{p.MemoryManager.GetModelId(chr2):X8}" : "Not a char");
                });
            }
            ImGui.EndChild();
        }
    }
}
