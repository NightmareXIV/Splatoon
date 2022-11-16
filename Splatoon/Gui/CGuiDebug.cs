using Dalamud;
using ECommons;
using ECommons.GameFunctions;
using ECommons.MathHelpers;
using ECommons.Reflection;
using Splatoon.Memory;
using Splatoon.SplatoonScripting;
using Splatoon.Utils;
using System.Reflection;
using System.Runtime.Loader;

namespace Splatoon
{
    unsafe partial class CGui
    {
        bool autoscrollLog = true;
        float s2wx, s2wy, s2wz, s2wrx, s2wry;
        bool s2wb = false;
        string[] Placeholders = new string[] { "<1>", "<2>", "<3>", "<4>", "<5>", "<6>", "<7>", "<8>", "<d1>", "<d2>", "<d3>", "<d4>", "<t1>", "<t2>", "<h1>", "<h2>", "<me>", "<t>", "<mo>", "<t2t>" };

        void DisplayDebug()
        {
            ImGui.BeginChild("##splatoonmaindbg");
            ImGui.Checkbox("Disable line fix", ref p.DisableLineFix);
            ImGuiEx.Text($"Line segments: {P.CurrentLineSegments}");
            {
                var t = Environment.TickCount64 - p.CombatStarted;
                ImGuiEx.Text("CombatStarted = " + t);
            }
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

            if (ImGui.CollapsingHeader("Object table"))
            {
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
            }
            if (ImGui.CollapsingHeader("NameNpcID"))
            {
                foreach(var x in NameNpcIDs)
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
            if (ImGui.CollapsingHeader("Placeholders"))
            {
                foreach(var x in Placeholders)
                {
                    ImGuiEx.Text($"{x}");
                    ImGui.SameLine();
                    ImGui.SetCursorPosX(60f);
                    var ph = FakePronoun.Resolve(x);
                    if (ph != null)
                    {
                        var obj = Svc.Objects.CreateObjectReference((IntPtr)ph);
                        ImGuiEx.Text($"{obj}");
                    }
                    else
                    {
                        ImGuiEx.Text("null");
                    }
                }
            }
            if (ImGui.CollapsingHeader("Distance"))
            {
                if (Svc.Targets.Target.NotNull(out var t))
                {
                    ImGuiEx.Text($"Distance c2c 3d: {Vector3.Distance(Svc.ClientState.LocalPlayer.Position, t.Position)}");
                    ImGuiEx.Text($"Distance c2c 2d: {Vector2.Distance(Svc.ClientState.LocalPlayer.Position.ToVector2(), t.Position.ToVector2())}");
                    ImGuiEx.Text($"Distance h2h 3d: {Vector3.Distance(Svc.ClientState.LocalPlayer.Position, t.Position) - Svc.ClientState.LocalPlayer.HitboxRadius - t.HitboxRadius}");
                    ImGuiEx.Text($"Distance h2h 2d: {Vector2.Distance(Svc.ClientState.LocalPlayer.Position.ToVector2(), t.Position.ToVector2()) - Svc.ClientState.LocalPlayer.HitboxRadius - t.HitboxRadius}");
                    ImGuiEx.Text($"Distance c2h 3d: {Vector3.Distance(Svc.ClientState.LocalPlayer.Position, t.Position) - Svc.ClientState.LocalPlayer.HitboxRadius}");
                    ImGuiEx.Text($"Distance c2h 2d: {Vector2.Distance(Svc.ClientState.LocalPlayer.Position.ToVector2(), t.Position.ToVector2()) - Svc.ClientState.LocalPlayer.HitboxRadius}");
                }
            }
            if (ImGui.Button("Compile clipboard"))
            {
                var txt = ImGui.GetClipboardText();
                ScriptingProcessor.CompileAndLoad(txt);
            }
            if(ImGui.Button("Get assemblies"))
            {
                var main = AppDomain.CurrentDomain.GetAssemblies();
                PluginLog.Information($"{main.Select(x => x.FullName).Join("\n")}");
            }
            if(ImGui.Button("Get local plugin"))
            {
                DuoLog.Information($"Success: {DalamudReflector.TryGetLocalPlugin(out var instance, out var type)}");
                if(type != null)
                {
                    DuoLog.Information($"{type.FullName}");
                    var loader = type.GetField("loader", ReflectionHelper.AllFlags).GetValue(instance);
                    DuoLog.Information($"{loader.GetType().FullName}");
                    var context = loader.GetFoP<AssemblyLoadContext>("context");
                    var txt = ImGui.GetClipboardText();
                    ScriptingProcessor.CompileAndLoad(txt);
                }
            }
            ImGui.EndChild();
            
        }
    }
}
