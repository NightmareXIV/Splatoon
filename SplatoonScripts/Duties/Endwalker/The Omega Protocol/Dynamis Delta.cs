using Dalamud.Game.ClientState.Objects.SubKinds;
using Dalamud.Game.ClientState.Objects.Types;
using ECommons;
using ECommons.Configuration;
using ECommons.DalamudServices;
using ECommons.GameFunctions;
using ECommons.Hooks.ActionEffectTypes;
using ECommons.Logging;
using ECommons.MathHelpers;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Splatoon.SplatoonScripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SplatoonScriptsOfficial.Duties.Endwalker.The_Omega_Protocol
{
    public unsafe class Dynamis_Delta : SplatoonScript
    {
        public override HashSet<uint> ValidTerritories => new() { 1122 };

        Config Conf => Controller.GetConfig<Config>();

        PlayerCharacter Player => Svc.ClientState.LocalPlayer;

        int Stage = 0;
        uint myTether;
        bool isMeClose;

        class Effects
        {
            public const uint NeatWorld = 3442;
            public const uint FarWorld = 3443;
            //  Remote Code Smell (3504), Remains = 16.2, Param = 0, Count = 0
            public const uint UpcomingBlueTether = 3504;
            //  Local Code Smell (3440), Remains = 16.2, Param = 0, Count = 0
            public const uint UpcomingGreenTether = 3440;
        }

        public override void OnSetup()
        {
            for(var i = 0; i < 8; i++)
            {
                Controller.RegisterElement($"Debug{i}", new(0) { Enabled = false});
            }
            Controller.RegisterElementFromCode("Bait", "{\"Name\":\"\",\"Enabled\":false,\"radius\":0.0,\"color\":3355508735,\"overlayBGColor\":4278190080,\"overlayTextColor\":4294967295,\"overlayFScale\":2.0,\"thicc\":5.0,\"overlayText\":\"BAIT\",\"tether\":true}");
            base.OnSetup();
        }

        public override void OnUpdate()
        {
            Off();
            if (Controller.Scene == 6)
            {
                var beetle = GetBeetle();
                var final = GetFinalOmega();
                if(beetle != null && final != null && (HasEffect(Effects.UpcomingGreenTether) || HasEffect(Effects.UpcomingBlueTether)))
                {
                    if (Stage == 0)
                    {
                        var p = FakeParty.Get().ToArray();
                        myTether = HasEffect(Effects.UpcomingGreenTether) ? Effects.UpcomingGreenTether : Effects.UpcomingBlueTether;
                        var sameTethers = p.Where(x => x.HasEffect(myTether)).OrderBy(x => GetAngleRelativeToObject(beetle, x, true)).ToArray();
                        var myPartner = (Player.Address.EqualsAny(sameTethers[0..1].Select(x => x.Address)) ? sameTethers[0..1] : sameTethers[2..3]).Where(x => x.Address != Player.Address).First();
                        var myMob = HasEffect(Effects.UpcomingGreenTether) ? final : beetle;
                        for (int i = 0; i < sameTethers.Length; i++)
                        {
                            if (Controller.TryGetElementByName($"Debug{i}", out var e))
                            {
                                e.Enabled = true;
                                e.SetRefPosition(sameTethers[i].Position);
                                e.overlayText = $"{GetAngleRelativeToObject(myMob, sameTethers[i], true)}" + (myPartner.Address == sameTethers[i].Address ? " Partner" : "");
                            }
                        }
                        var isMeClose = Vector3.Distance(myPartner.Position, new Vector3(100, 0, 100)) > Vector3.Distance(Player.Position, new Vector3(100, 0, 100));
                        InternalLog.Information($"Me close: {isMeClose}");
                        if(Svc.Objects.Any(x => x.DataId == 15710))
                        {
                            DuoLog.Information($"Snapshotting: you are blue " + (isMeClose ? "close" : "far"));
                            Stage = 1;
                        }
                    }
                    else if(Stage == 1)
                    {
                        var arms = GetArms().ToArray();
                        if (arms.Length == 6)
                        {
                            BattleChara myArm = null;
                            if(myTether == Effects.UpcomingBlueTether)
                            {
                                if (isMeClose)
                                {

                                }
                                else
                                {
                                    myArm = arms.OrderBy(x => Vector3.Distance(x.Position, beetle.Position)).ToArray()[0..1].OrderBy(x => Vector3.Distance(Player.Position, x.Position)).First();
                                }
                            }
                            else
                            {
                                if (isMeClose)
                                {
                                    myArm = arms.OrderBy(x => Vector3.Distance(x.Position, final.Position)).ToArray()[0..1].OrderBy(x => Vector3.Distance(Player.Position, x.Position)).First();
                                }
                                else
                                {
                                    myArm = arms.OrderBy(x => Vector3.Distance(x.Position, final.Position)).ToArray()[2..3].OrderBy(x => Vector3.Distance(Player.Position, x.Position)).First();
                                }
                            }
                            if(myArm != null && Controller.TryGetElementByName("Bait", out var e))
                            {
                                e.Enabled = true;
                                e.SetRefPosition(myArm.Position);
                            }
                        }
                    }
                }
            }
            else
            {
                Stage = 0;
            }
        }


        IEnumerable<BattleChara> GetArms()
        {
            foreach(var x in Svc.Objects)
            {
                if (x is BattleChara b && b.DataId.EqualsAny<uint>(15719, 15718)) yield return x as BattleChara;
            }
        }

        void Off()
        {
            Controller.GetRegisteredElements().Each(x => x.Value.Enabled = false);
        }

        BattleChara? GetBeetle() => Svc.Objects.FirstOrDefault(x => x is BattleChara c && c.Struct()->Character.ModelCharaId == 3771) as BattleChara;

        BattleChara? GetFinalOmega() => Svc.Objects.FirstOrDefault(x => x is BattleChara c && c.Struct()->Character.ModelCharaId == 3775) as BattleChara;

        bool HasEffect(uint id) => Player.StatusList.Any(x => x.StatusId == id);
        bool HasEffect(uint id, float remainsMin, float remainsMax) => Player.StatusList.Any(x => x.StatusId == id && x.RemainingTime.InRange(remainsMin,remainsMax));

        float GetAngleRelativeToObject(GameObject source, GameObject target, bool invert = false)
        {
            var angle = MathHelper.GetRelativeAngle(source.Position, target.Position);
            var angleRot = source.Rotation.RadToDeg();
            return (angle - angleRot + (invert?(360+180):360) ) % 360;
        }

        public class Config : IEzConfig
        {
            public bool Debug = false;
        }
    }

    public static class Dynamis_Delta_Extensions
    {
        public static bool HasEffect(this BattleChara obj, uint id) => obj.StatusList.Any(x => x.StatusId == id);
        public static bool HasEffect(this BattleChara obj, uint id, float remainsMin, float remainsMax) => obj.StatusList.Any(x => x.StatusId == id && x.RemainingTime.InRange(remainsMin, remainsMax));
    }
}
