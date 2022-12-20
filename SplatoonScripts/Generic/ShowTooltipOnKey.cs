using Dalamud.Interface.Colors;
using Dalamud.Interface;
using ECommons.ImGuiMethods;
using ECommons.Logging;
using ECommons.MathHelpers;
using ImGuiNET;
using PInvoke;
using Splatoon.SplatoonScripting;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ECommons.Configuration;
using FFXIVClientStructs.FFXIV.Client.UI.Misc;
using ECommons;
using FFXIVClientStructs.FFXIV.Component.GUI;
using Dalamud.Utility.Signatures;
using Dalamud.Hooking;

namespace SplatoonScriptsOfficial.Generic
{
    public unsafe class ShowTooltipOnKey : SplatoonScript
    {
        public override HashSet<uint> ValidTerritories => new();
        public override Metadata? Metadata => new(1, "NightmareXIV");

        bool keyState = false;
        Config Conf = null!;

        delegate long AtkTooltipManager_ShowNodeTooltip(long a1, byte a2, uint a3);
        [Signature("48 89 5C 24 ?? 48 89 74 24 ?? 57 48 83 EC 20 41 8B F8 0F B6 F2 48 8B D9 E8 ?? ?? ?? ?? F6 80 ?? ?? ?? ?? ?? 74 0F 44 8B C7 40 0F B6 D6 48 8B CB E8 ?? ?? ?? ?? 48 8B 5C 24 ?? 48 8B 74 24 ?? 48 83 C4 20 5F C3 CC CC CC CC CC CC CC CC CC CC CC 48 89 5C 24", DetourName = nameof(AtkTooltipManager_ShowNodeTooltipDetour), Fallibility = Fallibility.Fallible)]
        Hook<AtkTooltipManager_ShowNodeTooltip> AddonItemDetail_Show = null!;

        long AtkTooltipManager_ShowNodeTooltipDetour(long a1, byte a2, uint a3)
        {
            var ret = AddonItemDetail_Show.Original(a1, a2, a3);
            if (!Bitmask.IsBitSet(User32.GetKeyState((int)Conf.Key), 15))
            {
                ((AtkUnitBase*)a1)->Hide(false);
            }
            return ret;
        }

        public override void OnEnable()
        {
            SignatureHelper.Initialise(this);
            AddonItemDetail_Show?.Enable();
        }

        public override void OnDisable()
        {
            AddonItemDetail_Show?.Disable();
            AddonItemDetail_Show?.Dispose();
        }

        public override void OnSetup()
        {
            Conf = this.Controller.GetConfig<Config>();
        }

        public override void OnSettingsDraw()
        {
            ImGui.SetNextItemWidth(200f);
            if (ImGui.BeginCombo("##inputKey", $"{Conf.Key}"))
            {
                var block = false;
                if (ImGui.Selectable("Cancel"))
                {
                }
                if (ImGui.IsItemHovered()) block = true;
                if (ImGui.Selectable("Clear"))
                {
                    Conf.Key = Keys.None;
                }
                if (ImGui.IsItemHovered()) block = true;
                if (!block)
                {
                    ImGuiEx.Text(GradientColor.Get(ImGuiColors.ParsedGreen, ImGuiColors.DalamudRed), "Now press new key...");
                    foreach (var x in Enum.GetValues<Keys>())
                    {
                        if (Bitmask.IsBitSet(User32.GetKeyState((int)x), 15))
                        {
                            ImGui.CloseCurrentPopup();
                            Conf.Key = x;
                            break;
                        }
                    }
                }
                ImGui.EndCombo();
            }
            if (Conf.Key != Keys.None)
            {
                ImGui.SameLine();
                if (ImGuiEx.IconButton(FontAwesomeIcon.Trash))
                {
                    Conf.Key = Keys.None;
                }
            }
        }

        class Config : IEzConfig
        {
            public Keys Key = Keys.ControlKey;
        }
    }
}
