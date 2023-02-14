using Dalamud.Game.ClientState.Objects.SubKinds;
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
using FFXIVClientStructs.FFXIV.Client.Graphics.Environment;
using ImGuiNET;
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

        int value = 0;

        public override void OnSettingsDraw()
        {
            ImGui.InputInt("", ref value);
            if (ImGui.Button("Apply"))
            {
                var x = (nint)EnvManager.Instance();
                x += 36;
                *(byte*)x = (byte)value;
            }
        }
    }
}
