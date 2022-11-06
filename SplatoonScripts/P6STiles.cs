using Splatoon.SplatoonScripting;
using System.Collections.Generic;

namespace NightmareXIVSplatoonScripts
{
    public class P6STiles : SplatoonScript
    {
        public override string Name => "P6S Tiles";

        public override HashSet<uint> ValidTerritories => new() { 1084 };

        public override string? Author => "NightmareXIV";
        public override uint Version => 1;
        public override string? Description => "Displays unsafe tiles during Polyominoid Sigma in P6S";
        
    }
}