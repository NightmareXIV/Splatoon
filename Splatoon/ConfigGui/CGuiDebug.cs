using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var t = DateTimeOffset.Now.ToUnixTimeSeconds() - p.CombatStarted;
            ImGui.Text("CombatStarted = " + t);
            ImGui.Separator();
            if (p.pi.ClientState.LocalPlayer != null)
            {
                var mypos = p.GetPlayerPosition();
                ImGui.Text("My pos XYZ: \n" + mypos.X + "\n" + mypos.Y + "\n" + mypos.Z);
                var tar = p.pi.ClientState.Targets.CurrentTarget;
                if (tar != null)
                {
                    ImGui.Text("Target pos XYZ: \n" + tar.Position.X + "\n" + tar.Position.Y + "\n" + tar.Position.Z);
                }
            }
            ImGui.Separator();
            ImGui.Text("Query WorldToScreen");
            ImGui.PushItemWidth(50f);
            ImGui.DragFloat("##qs2wx", ref s2wx);
            ImGui.SameLine();
            ImGui.DragFloat("##qs2wy", ref s2wy);
            ImGui.SameLine();
            ImGui.DragFloat("##qs2wz", ref s2wz);
            ImGui.PopItemWidth();
            if (ImGui.Button("To my pos"))
            {
                var mypos = p.GetPlayerPosition();
                s2wx = mypos.X;
                s2wy = mypos.Y;
                s2wz = mypos.Z;
            }
            ImGui.SameLine();
            if (ImGui.Button("Query"))
            {
                s2wb = p.pi.Framework.Gui.WorldToScreen(new SharpDX.Vector3(s2wx, s2wz, s2wy), out SharpDX.Vector2 pos);
                s2wrx = pos.X;
                s2wry = pos.Y;
            }
            ImGui.TextColored(ImGui.ColorConvertU32ToFloat4(s2wb ? Colors.Green : Colors.Red), "X:" + s2wrx + "\nY:" + s2wry);
            ImGui.Separator();
            ImGui.Text("Camera angle X:" + p.CamAngleX);
            ImGui.Text("Camera angle Y:" + p.CamAngleY);
            ImGui.Separator();
            ImGui.Text("Actors:");
            foreach (var a in p.pi.ClientState.Actors)
            {
                try
                {
                    ImGui.Text(a.Name);
                }
                catch (Exception e)
                {
                    ImGui.Text(e.Message);
                }
            }
            ImGui.EndChild();
        }
    }
}
