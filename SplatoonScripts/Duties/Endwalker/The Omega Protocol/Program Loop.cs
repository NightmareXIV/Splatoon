using Dalamud.Game.ClientState.Objects.SubKinds;
using Dalamud.Game.ClientState.Objects.Types;
using Dalamud.Logging;
using ECommons;
using ECommons.Configuration;
using ECommons.Hooks;
using ECommons.ImGuiMethods;
using ECommons.Logging;
using ImGuiNET;
using Splatoon.SplatoonScripting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using PluginLog = ECommons.Logging.PluginLog;

namespace SplatoonScriptsOfficial.Duties.Endwalker.The_Omega_Protocol
{
    public class Program_Loop : SplatoonScript
    {
        public override HashSet<uint> ValidTerritories => new() { 1122 };

        public override Metadata? Metadata => new(1, "NightmareXIV");

        Config Conf => Controller.GetConfig<Config>();

        HashSet<uint> TetheredPlayers = new();

        public override void OnSetup()
        {
            SetupElements();
        }

        void SetupElements()
        {
            Controller.TryRegisterElement("Tether1", new(1)
            {
                color = Conf.TetherAOECol.ToUint(),
                refActorComparisonType = 2,
                onlyTargetable = true,
                Filled = true,
                Enabled = false,
                radius = 16f
            }, true);
            Controller.TryRegisterElement("Tether2", new(1)
            {
                color = Conf.TetherAOECol.ToUint(),
                refActorComparisonType = 2,
                onlyTargetable = true,
                Filled = true,
                Enabled = false,
                radius = 16f
            }, true);
        }

        public override void OnTetherCreate(uint source, uint target, byte data2, byte data3, byte data5)
        {
            if (IsOmega(target, out _))
            {
                TetheredPlayers.Add(source);
                UpdateTethers();
            }
        }

        public override void OnTetherRemoval(uint source, byte data2, byte data3, byte data5)
        {
            TetheredPlayers.Remove(source);
            UpdateTethers();
        }

        void UpdateTethers()
        {
            var items = TetheredPlayers.ToArray();
            if(items.Length >= 2)
            {
                PluginLog.Information($"Displaying");
                {
                    if (Controller.TryGetElementByName("Tether1", out var e))
                    {
                        e.Enabled = true;
                        e.refActorObjectID = items[0];
                    }
                }
                {
                    if (Controller.TryGetElementByName("Tether2", out var e))
                    {
                        e.Enabled = true;
                        e.refActorObjectID = items[1];
                    }
                }
            }
            else
            {
                PluginLog.Information($"Hiding");
                {
                    if (Controller.TryGetElementByName("Tether1", out var e))
                    {
                        e.Enabled = false;
                    }
                }
                {
                    if (Controller.TryGetElementByName("Tether2", out var e))
                    {
                        e.Enabled = false;
                    }
                }
            }
        }

        public override void OnDirectorUpdate(DirectorUpdateCategory category)
        {
            if(category.EqualsAny(DirectorUpdateCategory.Commence, DirectorUpdateCategory.Recommence, DirectorUpdateCategory.Wipe))
            {
                TetheredPlayers.Clear();
                UpdateTethers();
            }
        }

        bool IsOmega(uint oid, [NotNullWhen(true)]out BattleChara? omega)
        {
            if(oid.TryGetObject(out var obj) && obj is BattleChara o && o.NameId == 7695)
            {
                omega = o;
                return true;
            }
            omega = null;
            return false;
        }

        public override void OnSettingsDraw()
        {
            if(ImGui.ColorEdit4("Tether's AOE color", ref Conf.TetherAOECol, ImGuiColorEditFlags.NoInputs))
            {
                SetupElements();
            }
            if (ImGui.CollapsingHeader("Debug"))
            {
                foreach(var x in TetheredPlayers)
                {
                    ImGuiEx.Text($"Player: {x} {x.GetObject()}");
                }
            }
        }

        public class Config : IEzConfig
        {
            public Vector4 TetherAOECol = new(0f, 0f, 1f, 0.3f);
        }
    }
}
