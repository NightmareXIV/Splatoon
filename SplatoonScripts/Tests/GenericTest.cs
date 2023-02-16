using Dalamud.Game.ClientState.Objects.SubKinds;
using Dalamud.Game.ClientState.Objects.Types;
using Dalamud.Hooking;
using Dalamud.Interface.Colors;
using Dalamud.Utility.Signatures;
using ECommons;
using ECommons.DalamudServices;
using ECommons.ExcelServices.TerritoryEnumeration;
using ECommons.GameFunctions;
using ECommons.Hooks.ActionEffectTypes;
using ECommons.ImGuiMethods;
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

        public override void OnSetup()
        {
            Controller.RegisterElementFromCode("Test", "{\"Name\":\"\",\"type\":1,\"radius\":4.0,\"refActorType\":1,\"Filled\":true}");
            base.OnSetup();
        }

        public override void OnUpdate()
        {
            Controller.GetElementByName("Test").color = GradientColor.Get(GradientColor.Get(0xFF00FF00.ToVector4(), 0xFFFF0000.ToVector4()), GradientColor.Get(0xFF0000FF.ToVector4(), 0xFF000000.ToVector4()), 4000).ToUint();
        }

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
