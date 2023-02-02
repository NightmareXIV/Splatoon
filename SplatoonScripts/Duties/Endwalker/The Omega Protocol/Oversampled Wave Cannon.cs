using Dalamud.Game.ClientState.Objects.SubKinds;
using Dalamud.Game.ClientState.Objects.Types;
using Dalamud.Logging;
using ECommons;
using ECommons.Configuration;
using ECommons.DalamudServices;
using ECommons.GameFunctions;
using ECommons.ImGuiMethods;
using ECommons.Logging;
using ECommons.MathHelpers;
using ImGuiNET;
using Splatoon.SplatoonScripting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplatoonScriptsOfficial.Duties.Endwalker.The_Omega_Protocol
{
    public class Oversampled_Wave_Cannon : SplatoonScript
    {
        public override HashSet<uint> ValidTerritories => new(1122);
        public override Metadata? Metadata => new(1, "NightmareXIV");
        Config Conf => Controller.GetConfig<Config>();

        public override void OnSetup()
        {
            Controller.RegisterElementFromCode("FreeN", "{\"Name\":\"FreeN\",\"Enabled\":false,\"refX\":100.0,\"refY\":85.0,\"radius\":4.0,\"overlayBGColor\":4278190080,\"overlayTextColor\":4294967295,\"thicc\":5.0,\"overlayText\":\"Get hit once\",\"tether\":true}");
            Controller.RegisterElementFromCode("FreeS", "{\"Name\":\"FreeS\",\"Enabled\":false,\"refX\":100.0,\"refY\":115.0,\"radius\":4.0,\"overlayBGColor\":4278190080,\"overlayTextColor\":4294967295,\"thicc\":5.0,\"overlayText\":\"Get hit once\",\"tether\":true}");
            Controller.RegisterElementFromCode("", "{\"Name\":\"FreeW\",\"Enabled\":false,\"refX\":85.0,\"refY\":100.0,\"radius\":4.0,\"overlayBGColor\":4278190080,\"overlayTextColor\":4294967295,\"thicc\":5.0,\"overlayText\":\"Get hit once\",\"tether\":true}");
            Controller.RegisterElementFromCode("", "{\"Name\":\"FreeE\",\"Enabled\":false,\"refX\":115.0,\"refY\":100.0,\"radius\":4.0,\"overlayBGColor\":4278190080,\"overlayTextColor\":4294967295,\"thicc\":5.0,\"overlayText\":\"Get hit once\",\"tether\":true}");

            Controller.RegisterElementFromCode("EastM1", "{\"Name\":\"EastM1\",\"Enabled\":false,\"refX\":110.13328,\"refY\":90.989174,\"refZ\":-5.456968E-12,\"radius\":1.0,\"overlayBGColor\":4278190080,\"overlayTextColor\":4294967295,\"thicc\":5.0,\"overlayText\":\"Inner edge\",\"tether\":true}");
            Controller.RegisterElementFromCode("EastM2", "{\"Name\":\"EastM2\",\"Enabled\":false,\"refX\":110.057434,\"refY\":108.96221,\"refZ\":-5.456968E-12,\"radius\":1.0,\"overlayBGColor\":4278190080,\"overlayTextColor\":4294967295,\"thicc\":5.0,\"overlayText\":\"Inner edge\",\"tether\":true}");
            Controller.RegisterElementFromCode("EastM3", "{\"Name\":\"EastM3\",\"Enabled\":false,\"refX\":90.37988,\"refY\":109.85926,\"refZ\":-9.536798E-07,\"radius\":1.0,\"overlayBGColor\":4278190080,\"overlayTextColor\":4294967295,\"thicc\":5.0,\"overlayText\":\"IN MARKER\",\"tether\":true}");

            Controller.RegisterElementFromCode("WestM1", "{\"Name\":\"WestM1\",\"Enabled\":false,\"refX\":89.933,\"refY\":90.989174,\"refZ\":-5.456968E-12,\"radius\":1.0,\"overlayBGColor\":4278190080,\"overlayTextColor\":4294967295,\"thicc\":5.0,\"overlayText\":\"Inner edge\",\"tether\":true}");
            Controller.RegisterElementFromCode("WestM2", "{\"Name\":\"WestM2\",\"Enabled\":false,\"refX\":89.937,\"refY\":108.96221,\"refZ\":-5.456968E-12,\"radius\":1.0,\"overlayBGColor\":4278190080,\"overlayTextColor\":4294967295,\"thicc\":5.0,\"overlayText\":\"Inner edge\",\"tether\":true}");
            Controller.RegisterElementFromCode("WestM3", "{\"Name\":\"WestM3\",\"Enabled\":false,\"refX\":110.04,\"refY\":109.85926,\"refZ\":-9.536798E-07,\"radius\":1.0,\"overlayBGColor\":4278190080,\"overlayTextColor\":4294967295,\"thicc\":5.0,\"overlayText\":\"IN MARKER\",\"tether\":true}");
        }

        public override void OnUpdate()
        {
            if (IsMechanicRunning(out var direction))
            {
                var prio = ObtainMyPriority();
                if(prio.Priority != 0)
                {
                    if (prio.IsMonitor)
                    {

                    }
                }
            }
            else
            {

            }
        }

        public override void OnSettingsDraw()
        {
            ImGuiEx.Text($"Priority list:");
            ImGui.SameLine();
            if (ImGui.SmallButton("Test"))
            {
                if (TryGetPriorityList(out var list))
                {
                    DuoLog.Information($"Success: priority list {list.Print()}");
                }
                else
                {
                    DuoLog.Warning($"Could not get priority list");
                }
            }
            var toRem = -1;
            for (int i = 0; i < Conf.Priorities.Count; i++)
            {
                if (DrawPrioList(i))
                {
                    toRem = i;
                }
            }
            if(toRem != -1)
            {
                Conf.Priorities.RemoveAt(toRem);
            }
            if(ImGui.Button("Create new priority list"))
            {
                Conf.Priorities.Add(new string[] { "", "", "", "", "", "", "", "" });
            }
            if (ImGui.CollapsingHeader("Debug"))
            {
                var pr = ObtainMyPriority();
                ImGuiEx.Text($"My priority: {pr.Priority}, IsMonitor = {pr.IsMonitor}");
                if(IsMechanicRunning(out var dir))
                {
                    ImGuiEx.Text($"Mechanic is running, direction {dir}");
                }
            }
        }

        bool IsMechanicRunning(out CardinalDirection mechanicStep)
        {
            var caster = Svc.Objects.FirstOrDefault(x => x is BattleChara b && b.CastActionId.EqualsAny<uint>(31595, 31596)) as BattleChara;
            if(caster != null)
            {
                mechanicStep = caster.CastActionId == 31595 ? CardinalDirection.East : CardinalDirection.West;
                return true;
            }
            mechanicStep = default;
            return false;
        }

        (int Priority, bool IsMonitor) ObtainMyPriority()
        {
            if(TryGetPriorityList(out var list))
            {
                var isMonitor = Svc.ClientState.LocalPlayer.HasMonitor();
                if (isMonitor)
                {
                    var prio = 1;
                    foreach(var x in list.Where(z => GetPlayer(z)?.HasMonitor() == true))
                    {
                        if(x == Svc.ClientState.LocalPlayer.Name.ToString())
                        {
                            return (prio, true);
                        }
                        else
                        {
                            prio++;
                        }
                    }
                }
                else
                {
                    var prio = 1;
                    foreach (var x in list.Where(z => GetPlayer(z)?.HasMonitor() == false))
                    {
                        if (x == Svc.ClientState.LocalPlayer.Name.ToString())
                        {
                            return (prio, false);
                        }
                        else
                        {
                            prio++;
                        }
                    }
                }
            }
            return (0, false);
        }

        PlayerCharacter? GetPlayer(string name)
        {
            return FakeParty.Get().FirstOrDefault(x => x.Name.ToString() == name);
        }

        bool DrawPrioList(int num)
        {
            var prio = Conf.Priorities[num];
            ImGuiEx.Text($"Priority list {num+1}");
            ImGui.PushID($"prio{num}");
            for (int i = 0; i < prio.Length; i++)
            {
                ImGui.PushID($"prio{num}element{i}");
                ImGui.SetNextItemWidth(200);
                ImGui.InputText($"Player {i + 1}", ref prio[i], 50);
                ImGui.SameLine();
                ImGui.SetNextItemWidth(150);
                if (ImGui.BeginCombo("##partysel", "Select from party"))
                {
                    foreach(var x in FakeParty.Get())
                    {
                        if (ImGui.Selectable(x.Name.ToString()))
                        {
                            prio[i] = x.Name.ToString();
                        }
                    }
                    ImGui.EndCombo();
                }
                ImGui.PopID();
            }
            if(ImGui.Button("Delete this list (ctrl+click)") && ImGui.GetIO().KeyCtrl)
            {
                return true;
            }
            ImGui.PopID();
            return false;
        }

        bool TryGetPriorityList([NotNullWhen(true)]out string[]? values)
        {
            foreach(var p in Conf.Priorities)
            {
                var valid = true;
                var l = FakeParty.Get().Select(x => x.Name.ToString()).ToHashSet();
                foreach(var x in p)
                {
                    if (!l.Remove(x))
                    {
                        valid = false;
                        break;
                    }
                }
                if (valid)
                {
                    values = p;
                    return true;
                }
            }
            values = default;
            return false;
        }

        public class Config : IEzConfig
        {
            public List<string[]> Priorities = new();
        }
    }

    public static class OWCExtensions
    {
        public static bool HasMonitor(this PlayerCharacter c)
        {
            return c.StatusList.Any(x => x.StatusId.EqualsAny<uint>(3453, 3452));
        }
    }
}
