using Dalamud.Game.ClientState.Objects.Types;
using Dalamud.Hooking;
using Dalamud.Utility.Signatures;
using ECommons;
using ECommons.DalamudServices;
using ECommons.ExcelServices.TerritoryEnumeration;
using ECommons.GameFunctions;
using ECommons.Hooks.ActionEffectTypes;
using ECommons.Logging;
using ECommons.MathHelpers;
using Splatoon.SplatoonScripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SplatoonScriptsOfficial.Tests
{
    public unsafe class GenericTest : SplatoonScript
    {
        public override HashSet<uint> ValidTerritories => new() {  };

        //__int64 __fastcall sub_1411BFB30(__int64 a1)
        delegate nint Func(nint a1);
        [Signature("40 56 48 83 EC 20 48 8B F1 E8 ?? ?? ?? ?? 83 BE", DetourName =nameof(Detour))]
        Hook<Func> Hook;

        //__int64 __fastcall Component::GUI::AtkUnitBase_FireCallback(__int64 *a1, int a2, __int64 a3, char a4)
        delegate nint Func2(nint* a1, int a2, nint a3, byte a4);
        [Signature("48 89 5C 24 ?? 48 89 6C 24 ?? 56 57 41 54 41 56 41 57 48 81 EC ?? ?? ?? ?? 48 8B 05 ?? ?? ?? ?? 48 33 C4 48 89 44 24 ?? BF", DetourName = nameof(Detour2))]
        Hook<Func2> Hook2;

        nint Detour2(nint* a1, int a2, nint a3, byte a4)
        {
            DuoLog.Information($"{(nint)a1:X16}, {a2:X8}");
            return Hook2.Original(a1, a2, a3, a4);
        }

        nint Detour(nint a1)
        {
            DuoLog.Information($"{a1:X16}");
            return Hook.Original(a1);
        }

        public override void OnEnable()
        {
            SignatureHelper.Initialise(this);
            Hook?.Enable();
            Hook2?.Enable();
        }

        public override void OnDisable()
        {
            Hook?.Disable();
            Hook?.Dispose();
            Hook2?.Disable();
            Hook2?.Dispose();
        }
    }
}
