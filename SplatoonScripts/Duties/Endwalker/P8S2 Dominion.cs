using Dalamud.Game.ClientState.Objects.Types;
using ECommons.DalamudServices;
using ECommons.Logging;
using ECommons.MathHelpers;
using Splatoon.SplatoonScripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplatoonScriptsOfficial.Duties.Endwalker
{
    public class P8S2_Dominion : SplatoonScript
    {
        public override HashSet<uint> ValidTerritories => new() { 1088 };

        public override void OnSetup()
        {
            this.Controller.TryRegisterElement("Tower1", new(0) { Enabled = false, overlayText = "tower 1" });
            this.Controller.TryRegisterElement("Tower2", new(0) { Enabled = false, overlayText = "tower 2" });
            this.Controller.TryRegisterElement("Tower3", new(0) { Enabled = false, overlayText = "tower 3" });
            this.Controller.TryRegisterElement("Tower4", new(0) { Enabled = false, overlayText = "tower 4" });
        }

        public override void OnUpdate()
        {
            //tower cast: 31196
            //debuff:   Earth Resistance Down II (3372)
            if(Svc.Objects.Any(x => x is BattleChara c && c.IsCasting && c.CastActionId == 31196) && 
                !Svc.ClientState.LocalPlayer!.StatusList.Any(x => x.StatusId == 3372 && x.RemainingTime > 3f))
            {
                int i = 1;
                foreach(var x in GetEarliestTowers())
                {
                    var angle = (int)(MathHelper.GetRelativeAngle(new(100, 0, 100), x.Position) + 180) % 360;
                    if(this.Controller.TryGetElementByName($"Tower{i}", out var e))
                    {
                        e.Enabled = true;
                        e.refX = x.Position.X;
                        e.refY = x.Position.Z;
                        e.refZ = x.Position.Y;
                        e.overlayText = $"{angle}";
                    }
                    else
                    {
                        DuoLog.Warning($"{i} tower");
                    }
                    i++;
                }
            }
            else
            {
                { if (this.Controller.TryGetElementByName($"Tower1", out var e)) e.Enabled = false; }
                { if (this.Controller.TryGetElementByName($"Tower2", out var e)) e.Enabled = false; }
                { if (this.Controller.TryGetElementByName($"Tower3", out var e)) e.Enabled = false; }
                { if (this.Controller.TryGetElementByName($"Tower4", out var e)) e.Enabled = false; }
            }
        }

        IEnumerable<GameObject> GetEarliestTowers()
        {
            return Svc.Objects.Where(x => x is BattleChara b && b.IsCasting && b.CastActionId == 31196).Cast<BattleChara>().OrderBy(x => x.CurrentCastTime).Take(4);
        }
    }
}
