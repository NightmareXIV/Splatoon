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
        internal static void Draw()
        {
            if(Svc.Targets.Target != null && Svc.ClientState.LocalPlayer != null)
            {
                DrawGameObject(Svc.Targets.Target);
            }
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
            if(obj is Character c)
            {
                ImGuiEx.TextCopy("- Character -");
                ImGuiEx.TextCopy($"HP: {c.CurrentHp} / {c.MaxHp}");
                ImGuiEx.TextCopy($"Name NPC ID: {c.NameId}");
                ImGuiEx.TextWrappedCopy($"Customize: {c.Customize.Select(x => $"{x:X2}").Join(" ")}");
                ImGuiEx.TextCopy($"ModelCharaId: {c.Struct()->ModelCharaId}");
                ImGuiEx.TextCopy($"ModelCharaId_2: {c.Struct()->ModelCharaId_2}");
                /*ImGuiEx.TextCopy($"VfxData: {CharacterFunctions.GetVFXId(c.Struct()->VfxData)}");
                ImGuiEx.TextCopy($"VfxData2: {CharacterFunctions.GetVFXId(c.Struct()->VfxData2)}");
                if (c.Struct()->VfxData != null)
                {
                    var d = MemoryHelper.ReadRaw((IntPtr)c.Struct()->VfxData, 464);
                    ImGuiEx.TextWrappedCopy($"VFXdata: {(IntPtr)c.Struct()->VfxData:X16} \n{d.Select(x => $"{x:X2}").Join(" ")}");
                    var data = Separate(d, new byte[] { 0 }).ToList();
                    data.RemoveAll(x => x.Length == 0);
                    for (var i = 0; i < data.Count; i++)
                    {
                        ImGuiEx.TextCopy($"   {Encoding.UTF8.GetString(data[i])}");
                    }
                }
                if (c.Struct()->VfxData2 != null)
                {
                    var d = MemoryHelper.ReadRaw((IntPtr)c.Struct()->VfxData2, 464);
                    ImGuiEx.TextWrappedCopy($"VFXdata2 {(IntPtr)c.Struct()->VfxData2:X16}: \n{d.Select(x => $"{x:X2}").Join(" ")}");
                    var data = Separate(d, new byte[] { 0 }).ToList();
                    data.RemoveAll(x => x.Length == 0);
                    for (var i = 0; i < data.Count; i++)
                    {
                        ImGuiEx.TextCopy($"   {Encoding.UTF8.GetString(data[i])}");
                    }
                }*/
                ImGuiEx.Text("VFX (>60 sec)");
                if(c.TryGetVfx(out var fx))
                {
                    foreach(var x in fx)
                    {
                        ImGuiEx.TextCopy($"{x.Key}, Age = {x.Value.Age}");
                    }
                }
            }
            if(obj is BattleChara b)
            {
                ImGuiEx.TextCopy("- Battle chara -");
                ImGuiEx.TextCopy($"Casting: {b.IsCasting}, Action ID = {b.CastActionId.Format()}, Type = {b.CastActionType}, Cast time: {b.CurrentCastTime}/{b.TotalCastTime}");
                ImGuiEx.TextCopy($"Status:");
                foreach(var x in b.StatusList)
                {
                    ImGuiEx.TextCopy($"  {x.GameData.Name} ({x.StatusId}), Remains = {x.RemainingTime}, Param = {x.Param}, Count = {x.StackCount}");
                }
            }
        }
    }
}
