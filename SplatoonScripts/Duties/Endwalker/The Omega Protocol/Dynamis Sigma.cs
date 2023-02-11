using ECommons;
using ECommons.DalamudServices;
using ECommons.Logging;
using Splatoon.SplatoonScripting;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
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

        public const uint GlitchFar = 0;
        public const uint GlitchClose = 0;

        public override void OnSetup()
        {
            for (int i = 1; i <= 5; i++)
            {
                Controller.RegisterElementFromCode($"{i}", "{\"Enabled\":false,\"Name\":\"\",\"radius\":2.5,\"Donut\":0.5,\"color\":4278255615,\"overlayBGColor\":4110417920,\"overlayTextColor\":4278255615,\"overlayFScale\":2.0,\"overlayPlaceholders\":true,\"thicc\":4.0,\"overlayText\":\"L1\\\\nL2\",\"refActorDataID\":2013244,\"refActorComparisonType\":3}");
            }
            base.OnSetup();
        }

        public override void OnUpdate()
        {
            if(Controller.Scene == 6)
            {
                var towers = Svc.Objects.Where(x => x.DataId.EqualsAny<uint>(TowerSingle, TowerDual));
                if(towers.Count() == 5)
                {

                }
            }
            else
            {
                Off();
            }
        }

        void Off()
        {
            Controller.GetRegisteredElements().Each(x => x.Value.Enabled = false);
        }

        void SetTowerAs(int tower, params string[] s)
        {
            if(Controller.TryGetElementByName($"T{tower}", out var t))
            {
                t.overlayText = s.Join("\n");
            }
            else
            {
                DuoLog.Error($"Could not obtain element {tower}");
            }
        }
    }
}
