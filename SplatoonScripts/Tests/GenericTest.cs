using Dalamud.Hooking;
using Dalamud.Utility.Signatures;
using ECommons.DalamudServices;
using ECommons.Logging;
using Splatoon.SplatoonScripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplatoonScriptsOfficial.Tests
{
    public class GenericTest : SplatoonScript
    {
        public override HashSet<uint> ValidTerritories => new();

        delegate long Test(long a1, long a2, uint a3, uint a4, uint a5, int a6, int a7);
        [Signature("48 89 5C 24 ?? 57 48 83 EC 30 41 8B D9", DetourName = nameof(TestDetour))]
        Hook<Test> TestHook;

        public override void OnEnable()
        {
            SignatureHelper.Initialise(this);
            TestHook?.Enable();
        }

        public override void OnDisable()
        {
            TestHook?.Disable();
            TestHook?.Dispose();
        }

        long TestDetour(long a1, long a2, uint a3, uint a4, uint a5, int a6, int a7)
        {
            Svc.Chat.Print($"{a3:X}, {a4:X}, {a5:X}, {a6:X}, {a7:X}");
            return TestHook.Original(a1, a2,a3,a4,a5,a6,a7);
        }
    }
}
