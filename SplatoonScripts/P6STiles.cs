using ECommons.DalamudServices;
using Splatoon.SplatoonScripting;
using System.Collections.Generic;

namespace NightmareXIVSplatoonScripts
{
    public class P6STiles : SplatoonScript
    {
        public override HashSet<uint> ValidTerritories => new() { 5,15,25,990 };
        public override void OnSetup()
        {
            Svc.Chat.Print("Hello!");
        }

        public override void OnEnable()
        {
            Svc.Chat.Print("Enabled");
        }

        public override void OnDisable()
        {
            Svc.Chat.Print("Disabled");
        }
    }
}