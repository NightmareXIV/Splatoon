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
    public class ObjectEffectTest : SplatoonScript
    {
        public override HashSet<uint> ValidTerritories => new();

        delegate long ProcessObjectEffect(long a1, ushort a2, ushort a3, long a4);
        [Signature("40 53 55 56 57 48 81 EC ?? ?? ?? ?? 48 8B 05 ?? ?? ?? ?? 48 33 C4 48 89 84 24 ?? ?? ?? ?? 0F B7 FA", DetourName = nameof(ProcessObjectEffectDetour))]
        Hook<ProcessObjectEffect> ProcessObjectEffectHook;

        public override void OnEnable()
        {
            SignatureHelper.Initialise(this);
            ProcessObjectEffectHook.Enable();
            PluginLog.Information($"ProcessObjectEffectHook.Address: {ProcessObjectEffectHook.Address:X16}");
        }

        public override void OnDisable()
        {
            ProcessObjectEffectHook.Disable();
            ProcessObjectEffectHook.Dispose();
        }

        long ProcessObjectEffectDetour(long a1, ushort a2, ushort a3, long a4)
        {
            var ret = ProcessObjectEffectHook.Original(a1, a2, a3, a4);
            PluginLog.Information($"ObjectEffect: {a1:X16}, {a2:X4}, {a3:X4}, {a4:X16}, ret: {ret}");
            return ret;
        }
    }
}
