using Dalamud.Game.ClientState.Objects.SubKinds;
using Dalamud.Game.ClientState.Objects.Types;
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
using System.Linq;
using System.Numerics;

namespace SplatoonScriptsOfficial.Duties.Endwalker
{
    public class P8S2_Dominion : SplatoonScript
    {
        public override HashSet<uint> ValidTerritories => new() { 1088 };
        int Stage = 0;
        List<uint> FirstPlayers = new();
        List<uint> SecondPlayers = new();

        public override void OnSetup()
        {
            this.Controller.TryRegisterElement("MyTower", new(0) { Enabled = false, thicc=10, tether=true, radius = 0 });
        }

        public override void OnMessage(string Message)
        {
            if (Message.Contains("(11402>31193)"))
            {
                Stage = 1;
                DuoLog.Information($"Stage 1: Cast");
            }
        }
        public override void OnUpdate()
        {
            //tower cast: 31196
            //debuff:   Earth Resistance Down II (3372)

            if (Svc.ClientState.LocalPlayer == null) return;

            if (Stage == 1)
            {
                var playersSecondTowers = Svc.Objects.Where(x => x is PlayerCharacter pc && pc.StatusList.Any(x => x.StatusId == 3372 && x.RemainingTime > 6f));
                if (playersSecondTowers.Count() == 4)
                {
                    Stage = 2;
                    DuoLog.Information($"Stage 2: First towers");
                    FirstPlayers = Svc.Objects.Where(x => x is PlayerCharacter pc && !pc.StatusList.Any(x => x.StatusId == 3372) && IsRoleMatching(pc)).Select(x => x.ObjectId).ToList();
                    SecondPlayers = playersSecondTowers.Where(x => x is PlayerCharacter pc && IsRoleMatching(pc)).Select(x => x.ObjectId).ToList();
                    DuoLog.Information($"First towers: {FirstPlayers.Select(x => x.GetObject()?.Name).Print()}\nSecond towers: {SecondPlayers.Select(x => x.GetObject()?.Name).Print()}");
                }
            }
            else if(Stage == 2)
            {
                var towers = GetTowers();
                if(towers.Count() == 4)
                {
                    Stage = 3;
                    DuoLog.Information($"Stage 3: Second towers");
                    if (Controller.TryGetElementByName("MyTower", out var e)) e.Enabled = false;
                    Process(towers.OrderBy(GetAngle).ToArray(), FirstPlayers);
                }
                else if(!towers.Any())
                {
                    Reset();
                }
            }
            else if(Stage == 3)
            {
                var towers = GetTowers();
                if (towers.Count() == 8)
                {
                    Stage = 4;
                    DuoLog.Information($"Stage 4: Final");
                    if (Controller.TryGetElementByName("MyTower", out var e)) e.Enabled = false;
                    towers = GetEarliestTowers();
                    Process(towers.OrderBy(GetAngle).ToArray(), SecondPlayers);
                }
                if (!towers.Any())
                {
                    Reset();
                }
            }
            else if(Stage == 4)
            {
                if (!GetTowers().Any())
                {
                    Reset();
                }
            }
        }

        void Reset()
        {
            if (Controller.TryGetElementByName("MyTower", out var e)) e.Enabled = false;
            Stage = 0;
            DuoLog.Information($"Reset");
        }

        void Process(BattleChara[] towers, List<uint> players)
        {
            if (players.Contains(Svc.ClientState.LocalPlayer!.ObjectId) && Controller.TryGetElementByName("MyTower", out var e))
            {
                var prio = GetPriority().Where(x => players.Select(z => z.GetObject()!.Name.ToString()).Contains(x)).ToArray();
                if (prio.Length == 2)
                {
                    if (prio[0] == Svc.ClientState.LocalPlayer.Name.ToString())
                    {
                        e.Enabled = true;
                        var pos = ((Svc.ClientState.LocalPlayer.GetRole() == CombatRole.DPS) != this.Controller.GetConfig<Config>().Reverse) ? 2 : 0;
                        e.refX = towers[pos].Position.X;
                        e.refY = towers[pos].Position.Z;
                        e.refZ = towers[pos].Position.Y;
                        //first prio
                    }
                    else
                    {
                        e.Enabled = true;
                        var pos = ((Svc.ClientState.LocalPlayer.GetRole() == CombatRole.DPS) != this.Controller.GetConfig<Config>().Reverse) ? 3 : 1;
                        e.refX = towers[pos].Position.X;
                        e.refY = towers[pos].Position.Z;
                        e.refZ = towers[pos].Position.Y;
                        //second prio
                    }
                }
            }
        }

        bool IsRoleMatching(PlayerCharacter pc)
        {
            if(Svc.ClientState.LocalPlayer.GetRole() == CombatRole.DPS)
            {
                return pc.GetRole() == CombatRole.DPS;
            }
            else
            {
                return pc.GetRole() != CombatRole.DPS;
            }
        }

        List<string> GetPriority()
        {
            var x = this.Controller.GetConfig<Config>().Priorities.FirstOrDefault(z => z.All(n => Svc.Objects.Any(e => e is PlayerCharacter pc && pc.Name.ToString() == n)));
            if(x != null)
            {
                DuoLog.Information($"Got priority list: {x.Print()}");
                return x;
            }
            DuoLog.Warning("Could not find priority list");
            return new();
        }

        int GetAngle(BattleChara x)
        {
            return (int)(MathHelper.GetRelativeAngle(new(100, 0, 100), x.Position) + 180) % 360;
        }

        IEnumerable<BattleChara> GetEarliestTowers()
        {
            return GetTowers().OrderBy(x => x.CurrentCastTime).Take(4);
        }

        IEnumerable<BattleChara> GetTowers()
        {
            return Svc.Objects.Where(x => x is BattleChara b && b.IsCasting && b.CastActionId == 31196).Cast<BattleChara>();
        }

        public override void OnSettingsDraw()
        {
            ImGuiEx.TextWrapped("Priority lists. Fill in priority for your role left to right. You may create few lists, one that contains all players in your party will be used.");
            var c = this.Controller.GetConfig<Config>().Priorities;
            int toRem = -1;
            for (int i = 0; i < c.Count; i++)
            {
                ImGui.PushID("List" + i);
                EditList(c[i]);
                ImGui.SameLine();
                if (ImGuiEx.IconButton(Dalamud.Interface.FontAwesomeIcon.FastForward))
                {
                    var people = GetPriority();
                    var s = people.Count == 4;
                    foreach(var x in people)
                    {
                        if(Svc.Objects.TryGetFirst(z => z is PlayerCharacter pc && pc.Name.ToString() == x, out var o))
                        {
                            if (!IsRoleMatching((PlayerCharacter)o))
                            {
                                DuoLog.Warning($"Role mismatch with {o.Name}");
                                s = false;
                            }
                        }
                        else
                        {
                            DuoLog.Warning($"Could not find player {x}");
                            s = false;
                        }
                    }
                    if (s)
                    {
                        DuoLog.Information("Test success!");
                    }
                    else
                    {
                        DuoLog.Warning("Test failed");
                    }
                }
                ImGuiEx.Tooltip("Check if current party matches this priority list. All players must be within visible range.");
                ImGui.SameLine();
                if(ImGuiEx.IconButton(Dalamud.Interface.FontAwesomeIcon.Trash) && ImGui.GetIO().KeyCtrl)
                {
                    toRem = i;
                }
                ImGuiEx.Tooltip("Delete list. Hold CTRL+click.");
                ImGui.Separator();
                ImGui.PopID();
            }
            if(toRem > -1)
            {
                c.RemoveAt(toRem);
            }
            ImGui.Checkbox("Reverse (DPS left, H+T right)", ref this.Controller.GetConfig<Config>().Reverse);
            ImGui.SameLine();
            if (ImGui.Button("Add new priority list"))
            {
                c.Add(new() { "", "", "", "" });
            }
        }

        public void EditList(List<string> s)
        {
            for (int i = 0; i < s.Count; i++)
            {
                var t = s[i];
                ImGui.SetNextItemWidth(ImGui.GetContentRegionMax().X / 6f);
                if(ImGui.InputText($"##in{i}", ref t, 100))
                {
                    s[i] = t;
                }
                ImGui.SameLine();
                if (ImGui.Button($"  T  ##{i}") && Svc.Targets.Target is PlayerCharacter pc)
                {
                    s[i] = pc.Name.ToString();
                }
                ImGui.SameLine();
            }
            ImGui.Dummy(Vector2.Zero);
        }

        public class Config : IEzConfig
        {
            public bool Reverse = false;
            public List<List<string>> Priorities = new();
        }
    }
}
