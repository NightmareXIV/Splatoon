using Dalamud.Game.ClientState.Objects.SubKinds;
using Dalamud.Game.ClientState.Objects.Types;
using Dalamud.Interface.Colors;
using ECommons;
using ECommons.DalamudServices;
using ECommons.ImGuiMethods;
using ECommons.Logging;
using ECommons.MathHelpers;
using ImGuiNET;
using Splatoon.Memory;
using Splatoon.SplatoonScripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SplatoonScriptsOfficial.Duties.Endwalker.The_Omega_Protocol
{
    public class Party_Synergy : SplatoonScript
    {
        public override HashSet<uint> ValidTerritories => new() { 1122 };
        const string StackVFX = "vfx/lockon/eff/com_share2i.avfx";
        const string ChainVFX = "vfx/lockon/eff/z3oz_firechain_0";

        public override void OnVFXSpawn(uint target, string vfxPath)
        {
            //Dequeued message: VFX vfx/lockon/eff/com_share2i.avfx
            if (vfxPath == StackVFX && Svc.ClientState.LocalPlayer.StatusList.Any(x => x.StatusId.EqualsAny<uint>(3427, 3428)))
            {
                var stackers = AttachedInfo.VFXInfos.Where(x => x.Value.Any(z => z.Key == StackVFX && z.Value.Age < 1000)).Select(x => x.Key).Select(x => Svc.Objects.FirstOrDefault(z => z.Address == x)).ToArray();
                var opticalUnit = Svc.Objects.FirstOrDefault(x => x is Character c && c.NameId == 7640);
                if(stackers.Length == 2 && opticalUnit != null)
                {
                    var mid = MathHelper.GetRelativeAngle(new(100, 100), opticalUnit.Position.ToVector2());
                    var a1 = MathHelper.GetRelativeAngle(stackers[0].Position, opticalUnit.Position);
                    var a2 = MathHelper.GetRelativeAngle(stackers[1].Position, opticalUnit.Position);
                    //DuoLog.Information($"Angles: {a1}, {a2}");
                    if((a1 > mid && a2 > mid) || (a1 < mid && a2 < mid))
                    {
                        //DuoLog.Information($"Swap!");
                        var swapper = stackers.OrderBy(x => Vector3.Distance(opticalUnit.Position, x.Position)).ToArray()[1];
                        var swappersVfx = AttachedInfo.VFXInfos[swapper.Address].FirstOrDefault(x => x.Key.Contains(ChainVFX) && x.Value.AgeF < 60).Key;
                        //DuoLog.Information($"Swapper: {swapper} Swapper's vfx: {swappersVfx}");
                        var secondSwapper = AttachedInfo.VFXInfos.Where(x => x.Key != swapper.Address && x.Value.Any(z => z.Key.Contains(swappersVfx) && z.Value.AgeF < 60)).Select(x => x.Key).Select(x => Svc.Objects.FirstOrDefault(z => z.Address == x)).FirstOrDefault();
                        //DuoLog.Information($"Second swapper: {secondSwapper}");
                        DuoLog.Warning($"{swapper.Name} and {secondSwapper?.Name} swap!");
                        if (Svc.ClientState.LocalPlayer.Address.EqualsAny(swapper.Address, secondSwapper.Address))
                        {
                            new TimedMiddleOverlayWindow("swaponYOU", 5000, () =>
                            {
                                ImGui.SetWindowFontScale(2f);
                                ImGuiEx.Text(ImGuiColors.DalamudRed, $"Stack swap position!\n{swapper.Name} <-> {secondSwapper?.Name}");
                            }, 300);
                        }
                    }
                    else
                    {
                        DuoLog.Information($"No swap");
                    }
                }
            }
        }

        public override void OnSettingsDraw()
        {
            var opticalUnit = Svc.Objects.FirstOrDefault(x => x is Character c && c.NameId == 7640);
            if(opticalUnit != null)
            {
                var mid = MathHelper.GetRelativeAngle(new(100, 100), opticalUnit.Position.ToVector2());
                ImGuiEx.Text($"Mid: {mid}");
                foreach(var x in Svc.Objects)
                {
                    if(x is PlayerCharacter pc) 
                    {
                        var pos = MathHelper.GetRelativeAngle(pc.Position.ToVector2(), opticalUnit.Position.ToVector2());
                        ImGuiEx.Text($"{pc.Name} {pos} {(pos > mid?"left":"right")}");
                    }
                }
            }
        }

    }
}
