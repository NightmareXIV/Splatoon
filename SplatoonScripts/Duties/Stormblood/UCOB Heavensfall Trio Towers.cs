using Dalamud.Game.ClientState.Objects.Types;
using ECommons.DalamudServices;
using Splatoon.SplatoonScripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplatoonScriptsOfficial.Duties.Stormblood;

public class UCOB_Heavensfall_Trio_Towers : SplatoonScript
{
    public override HashSet<uint> ValidTerritories => new() { 733 };

    public override Metadata? Metadata => new(1, "NightmareXIV");

    public override void OnUpdate()
    {
        
    }

    IEnumerable<BattleChara> FindTowers()
    {
        return Svc.Objects.Where(x => x is BattleChara c && c.IsCasting && c.CastActionId == 9951).Cast<BattleChara>();
    }
}
