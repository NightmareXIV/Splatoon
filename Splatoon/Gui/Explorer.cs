using Dalamud.Interface.Colors;
using Dalamud.Memory;
using ECommons.GameFunctions;
using ECommons.MathHelpers;
using Splatoon.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splatoon.Gui
{
    internal unsafe static class Explorer
    {
        internal static IntPtr Ptr = IntPtr.Zero;
        internal static void Draw()
        {
            ImGui.BeginChild("##exch");
            var x = Svc.Objects.FirstOrDefault(x => x.Address == Ptr);
            ImGuiEx.Text(ImGuiColors.DalamudOrange, "Beta");
            if(ImGui.BeginCombo("##selector", $"{(Ptr == IntPtr.Zero?"Target":$"{(x == null?$"{Ptr:X16} - invalid ptr" : $"{x}")}")}"))
            {
                if (ImGui.Selectable("Target"))
                {
                    Ptr = IntPtr.Zero;
                }
                foreach(var o in Svc.Objects)
                {
                    if (ImGui.Selectable($"{o}"))
                    {
                        Ptr = o.Address;
                    }
                }
                ImGui.EndCombo();
            }
            if (Ptr == IntPtr.Zero)
            {
                if (Svc.Targets.Target != null && Svc.ClientState.LocalPlayer != null)
                {
                    DrawGameObject(Svc.Targets.Target);
                }
            }
            else
            {
                if(x != null)
                {
                    DrawGameObject(x);
                }
            }
            ImGui.EndChild();
        }

        internal static void DrawGameObject(GameObject obj)
        {
            ImGuiEx.TextCopy($"GameObject {obj}");
            ImGuiEx.TextCopy($"ObjectKind: {obj.ObjectKind}");
            ImGuiEx.TextCopy($"Position: {obj.Position.X} {obj.Position.Y} {obj.Position.Z}");
            ImGuiEx.TextCopy($"Rotation: {obj.Rotation}");
            ImGuiEx.TextCopy($"Vector3 distance: {Vector3.Distance(obj.Position, Svc.ClientState.LocalPlayer.Position)}");
            ImGuiEx.TextCopy($"Vector2 distance: {Vector2.Distance(obj.Position.ToVector2(), Svc.ClientState.LocalPlayer.Position.ToVector2())}");
            ImGuiEx.TextCopy($"Object ID long: {((long)obj.Struct()->GetObjectID()).Format()}");
            ImGuiEx.TextCopy($"Object ID: {obj.ObjectId.Format()}");
            ImGuiEx.TextCopy($"Data ID: {obj.DataId.Format()}");
            ImGuiEx.TextCopy($"Owner ID: {obj.OwnerId.Format()}");
            ImGuiEx.TextCopy($"NPC ID: {obj.Struct()->GetNpcID()}");
            ImGuiEx.TextCopy($"Dead: {obj.Struct()->IsDead()}");
            ImGuiEx.TextCopy($"Hitbox radius: {obj.HitboxRadius}");
            ImGuiEx.TextCopy($"Targetable: {obj.Struct()->GetIsTargetable()}");
            ImGuiEx.TextCopy($"Nameplate: {ObjectFunctions.GetNameplateColor(obj.Address)}");
            ImGuiEx.TextCopy($"Is hostile: {ObjectFunctions.IsHostile(obj)}");
            if (obj is Character c)
            {
                ImGuiEx.TextCopy("---------- Character ----------");
                ImGuiEx.TextCopy($"HP: {c.CurrentHp} / {c.MaxHp}");
                ImGuiEx.TextCopy($"Name NPC ID: {c.NameId}");
                ImGuiEx.TextWrappedCopy($"Customize: {c.Customize.Select(x => $"{x:X2}").Join(" ")}");
                ImGuiEx.TextCopy($"ModelCharaId: {c.Struct()->ModelCharaId}");
                ImGuiEx.TextCopy($"ModelCharaId_2: {c.Struct()->ModelCharaId_2}");
                ImGuiEx.TextCopy($"Visible: {c.IsCharacterVisible()}");
                ImGuiEx.Text("VFX");
                if(c.TryGetVfx(out var fx))
                {
                    foreach(var x in fx)
                    {
                        ImGuiEx.TextCopy($"{x.Key}, Age = {x.Value.AgeF:F1}");
                    }
                }
            }
            if(obj is BattleChara b)
            {
                ImGuiEx.TextCopy("---------- Battle chara ----------");
                ImGuiEx.TextCopy($"Casting: {b.IsCasting}, Action ID = {b.CastActionId.Format()}, Type = {b.CastActionType}, Cast time: {b.CurrentCastTime:F1}/{b.TotalCastTime:F1}");
                if (AttachedInfo.CastInfos.TryGetValue(b.Address, out var info)) 
                {
                    ImGuiEx.TextCopy($"Overcast: ID={info.ID}, StartTime={info.StartTime}, Age={info.AgeF:F1}");
                }
                ImGuiEx.TextCopy($"Status:");
                foreach(var x in b.StatusList)
                {
                    ImGuiEx.TextCopy($"  {x.GameData.Name} ({x.StatusId.Format()}), Remains = {x.RemainingTime:F1}, Param = {x.Param}, Count = {x.StackCount}");
                }
            }
        }
    }
}
