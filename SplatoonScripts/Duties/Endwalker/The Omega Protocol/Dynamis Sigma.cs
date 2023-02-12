using Dalamud.Game.ClientState.Objects.SubKinds;
using Dalamud.Game.ClientState.Objects.Types;
using ECommons;
using ECommons.Configuration;
using ECommons.DalamudServices;
using ECommons.Hooks.ActionEffectTypes;
using ECommons.Logging;
using ECommons.MathHelpers;
using ECommons.Reflection;
using FFXIVClientStructs.FFXIV.Component.GUI;
using Splatoon.SplatoonScripting;
using Splatoon.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SplatoonScriptsOfficial.Duties.Endwalker.The_Omega_Protocol
{
    public class Dynamis_Sigma : SplatoonScript
    {
        public override HashSet<uint> ValidTerritories => new() { 1122 };

        public const uint TowerSingle = 2013245;
        public const uint TowerDual = 2013246;
        public const uint TowerAny = 2013244;

        public const uint GlitchFar = 3428;
        public const uint GlitchClose = 3427;

        Vector3 OmegaPos = Vector3.Zero;

        GameObject[] GetTowers() => Svc.Objects.Where(x => x.DataId.EqualsAny<uint>(TowerSingle, TowerDual)).ToArray();

        public override void OnSetup()
        {
            for (int i = 0; i < 6; i++)
            {
                Controller.RegisterElementFromCode($"{i}", "{\"Enabled\":false,\"Name\":\"\",\"radius\":2.5,\"Donut\":0.5,\"color\":4278255615,\"overlayBGColor\":4110417920,\"overlayTextColor\":4278255615,\"overlayFScale\":2.0,\"overlayPlaceholders\":true,\"thicc\":4.0,\"overlayText\":\"L1\\\\nL2\",\"refActorDataID\":2013244,\"refActorComparisonType\":3}");
            }
            base.OnSetup();
        }

        public override void OnUpdate()
        {
            Off();
            if (Controller.Scene == 6)
            {
                if (GetTowers().Length.EqualsAny(5, 6))
                {
                    var towers = GetTowers().OrderBy(x => GetTowerAngle(x, IsInverted())).ToArray();
                    Queue<string> enumeration = Svc.ClientState.LocalPlayer.StatusList.Any(x => x.StatusId == GlitchFar)? new(new string[] { "R2", "R3", "R4", "L4", "L3", "L2", "L1", "R1" }) : new(new string[]{ "R1", "R2", "R3", "R4", "L4", "L3", "L2", "L1" });
                    for (int i = 0; i < towers.Length; i++)
                    {
                        if (towers[i].DataId == TowerSingle) 
                        {
                            SetTowerAs(i, towers[i], enumeration.Dequeue());
                        }
                        else
                        {
                            SetTowerAs(i, towers[i], enumeration.Dequeue(), enumeration.Dequeue());
                        }
                    }
                }
            }
        }

        public override void OnMessage(string Message)
        {
            if (Controller.Scene != 6) return;
            if(Message.EqualsAny("The right arm unit uses Hyper Pulse."))
            {
                OmegaPos = Svc.Objects.FirstOrDefault(x => x.DataId == 15720)?.Position ?? Vector3.Zero;
                PluginLog.Information($"Omega position captured: {OmegaPos}");
            }
        }

        void Off()
        {
            Controller.GetRegisteredElements().Each(x => x.Value.Enabled = false);
        }

        void SetTowerAs(int tower, GameObject obj, params string[] s)
        {
            if(Controller.TryGetElementByName($"{tower}", out var t))
            {
                Array.Sort(s);
                t.Enabled = true;
                t.SetRefPosition(obj.Position);
                t.overlayText = s.Join("\n");
            }
            else
            {
                DuoLog.Error($"Could not obtain element {tower}");
            }
        }

        float GetTowerAngle(GameObject t, bool inverted = false)
        {
            var z = new Vector3(100, 0, 100);
            var angle = (MathHelper.GetRelativeAngle(z, t.Position) + (inverted?179:-1) + 360 - MathHelper.GetRelativeAngle(z, OmegaPos)) % 360;
            return angle;
        }

        bool IsInverted()
        {
            if(Svc.ClientState.LocalPlayer.StatusList.Any(x => x.StatusId == GlitchFar))
            {
                return !GetTowers().Any(x => GetTowerAngle(x) > 360-3);
            }
            else
            {
                return !GetTowers().Any(x => GetTowerAngle(x).InRange(90, 90+45));
            }
        }

        public class Config : IEzConfig
        {
            
        }
    }
}
