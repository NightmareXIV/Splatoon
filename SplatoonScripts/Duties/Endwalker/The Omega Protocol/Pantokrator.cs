using Dalamud.Game.ClientState.Objects.SubKinds;
using Dalamud.Game.ClientState.Objects.Types;
using ECommons.DalamudServices;
using ECommons.GameFunctions;
using ECommons.MathHelpers;
using Splatoon.SplatoonScripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SplatoonScriptsOfficial.Duties.Endwalker.The_Omega_Protocol
{
    public class Pantokrator : SplatoonScript
    {
        public override HashSet<uint> ValidTerritories => new() { 1122 };
        BattleChara? Omega => Svc.Objects.FirstOrDefault(x => x is BattleChara o && o.NameId == 7695 && o.IsTargetable()) as BattleChara;

        //  Condensed Wave Cannon Kyrios (3508), Remains = 9.6, Param = 0, Count = 0
        const uint Laser = 3508;
        //  Guided Missile Kyrios Incoming (3497), Remains = 21.6, Param = 0, Count = 0
        const uint Rocket = 3497;
        const uint FirstInLine = 3004;

        public override void OnSetup()
        {
            Controller.RegisterElement("Laser1", new(2) { Enabled = false, color = 0xDAFFFF00, radius = 4f, refX = 100f, refY = 100f });
            Controller.RegisterElement("Laser2", new(2) { Enabled = false, color = 0xDAFFFF00, radius = 4f, refX = 100f, refY = 100f });
        }

        public override void OnUpdate()
        {
            if (!Omega) return;
            var lasers = Svc.Objects.Where(x => x is PlayerCharacter pc && pc.StatusList.Any(z => z.StatusId == Laser && z.RemainingTime <= 6f)).ToArray();
            var rockets = Svc.Objects.Where(x => x is PlayerCharacter pc && pc.StatusList.Any(z => z.StatusId == Rocket && (z.RemainingTime <= 6f || pc.StatusList.Any(c => c.StatusId == FirstInLine)))).ToArray();
            if(lasers.Length == 2)
            {
                Controller.GetElementByName("Laser1").Enabled = true;
                Controller.GetElementByName("Laser2").Enabled = true;
                var angle = MathHelper.GetRelativeAngle(new(100f, 100f), lasers[0].Position.ToVector2());
                var point = RotatePoint(100f, 100f, angle, new())
            }
            else
            {
                Controller.GetElementByName("Laser1").Enabled = false;
                Controller.GetElementByName("Laser2").Enabled = false;
            }
        }

        public static Vector3 RotatePoint(float cx, float cy, float angle, Vector3 p)
        {
            if (angle == 0f) return p;
            var s = (float)Math.Sin(angle);
            var c = (float)Math.Cos(angle);

            // translate point back to origin:
            p.X -= cx;
            p.Y -= cy;

            // rotate point
            float xnew = p.X * c - p.Y * s;
            float ynew = p.X * s + p.Y * c;

            // translate point back:
            p.X = xnew + cx;
            p.Y = ynew + cy;
            return p;
        }
    }
}
