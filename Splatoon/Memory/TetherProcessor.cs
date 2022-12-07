using Dalamud.Hooking;
using Dalamud.Utility.Signatures;
using Splatoon.SplatoonScripting;
using Character = FFXIVClientStructs.FFXIV.Client.Game.Character.Character;

namespace Splatoon.Memory;

internal unsafe class TetherProcessor
{
    delegate long ProcessTether(Character* a1, byte a2, byte a3, long targetOID, byte a5);
    [Signature("48 89 5C 24 ?? 48 89 6C 24 ?? 48 89 74 24 ?? 57 41 54 41 55 41 56 41 57 48 83 EC 20 0F B6 6C 24", DetourName = nameof(ProcessTetherDetour), Fallibility = Fallibility.Fallible)]
    Hook<ProcessTether> ProcessTetherHook = null;

    internal TetherProcessor()
    {
        SignatureHelper.Initialise(this);
        Enable();
    }

    internal void Enable()
    {
        if (ProcessTetherHook?.IsEnabled == false) ProcessTetherHook?.Enable();
    }

    internal void Disable()
    {
        if (ProcessTetherHook?.IsEnabled == true) ProcessTetherHook?.Disable();
    }

    internal void Dispose()
    {
        Disable();
        ProcessTetherHook?.Dispose();
    }

    long ProcessTetherDetour(Character* a1, byte a2, byte a3, long targetOID, byte a5)
    {
        try
        {
            if(targetOID == 0xE0000000)
            {
                ScriptingProcessor.OnTetherRemoval(a1->GameObject.ObjectID, a2, a3, a5);
            }
            else
            {
                ScriptingProcessor.OnTetherCreate(a1->GameObject.ObjectID, (uint)targetOID, a2, a3, a5);
            }
        }
        catch (Exception e)
        {
            e.Log();
        }
        return ProcessTetherHook.Original(a1, a2, a3, targetOID, a5);
    }
}
