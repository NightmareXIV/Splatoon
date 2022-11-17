using ECommons.DalamudServices;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Splatoon.SplatoonScripting;
using System.Collections.Generic;

namespace SplatoonScripts.Duties.Endwalker
{
    public class P6STiles : SplatoonScript
    {
        public override HashSet<uint> ValidTerritories => new() { 5,15,25,990 };

        public override Metadata? Metadata => new(1, "NightmareXIV", null, "https://github.com/NightmareXIV")
        {
            UpdateURL = "https://github.com/NightmareXIV/Splatoon/raw/master/SplatoonScripts/Duties/Endwalker/"
        };

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