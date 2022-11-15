using Splatoon.SplatoonScripting;
using System.Collections.Generic;

namespace NightmareXIVSplatoonScripts
{
    public class P6STiles : SplatoonScript
    {
        public override HashSet<uint> ValidTerritories => new() { 5,15,25 };
    }
}