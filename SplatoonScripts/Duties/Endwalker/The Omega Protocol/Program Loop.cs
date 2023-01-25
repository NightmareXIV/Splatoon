﻿using Dalamud.Game.ClientState.Objects.SubKinds;
using Dalamud.Game.ClientState.Objects.Types;
using Dalamud.Interface.Colors;
using Dalamud.Interface.Style;
using Dalamud.Logging;
using ECommons;
using ECommons.Configuration;
using ECommons.DalamudServices;
using ECommons.GameFunctions;
using ECommons.Hooks;
using ECommons.ImGuiMethods;
using ECommons.Logging;
using ECommons.MathHelpers;
using ECommons.Schedulers;
using ImGuiNET;
using Splatoon.SplatoonScripting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using PluginLog = ECommons.Logging.PluginLog;

namespace SplatoonScriptsOfficial.Duties.Endwalker.The_Omega_Protocol
{
    public unsafe class Program_Loop : SplatoonScript
    {
        public override HashSet<uint> ValidTerritories => new() { 1122 };
        public override Metadata? Metadata => new(2, "NightmareXIV");
        Config Conf => Controller.GetConfig<Config>();
        HashSet<uint> TetheredPlayers = new();
        List<uint> Towers = new();
        List<uint> TowerOrder = new();
        List<uint> TetherOrder = new();


        public override void OnSetup()
        {
            SetupElements();
        }

        void SetupElements()
        {
            Controller.Clear();
            Controller.RegisterElement("dbg1", new(1) { Enabled = false, refActorComparisonType = 2, overlayVOffset = 1, radius = 3f, color = 0xFFFFFF00 });
            Controller.RegisterElement("dbg2", new(1) { Enabled = false, refActorComparisonType = 2, overlayVOffset = 1, radius = 3f, color = 0xFFFFFF00 });
            Controller.RegisterElement("TetherAOE1", new(1)
            {
                color = Conf.TetherAOECol.ToUint(),
                refActorComparisonType = 2,
                onlyTargetable = true,
                Filled = true,
                Enabled = false,
                radius = 16f
            });
            Controller.RegisterElement("TetherAOE2", new(1)
            {
                color = Conf.TetherAOECol.ToUint(),
                refActorComparisonType = 2,
                onlyTargetable = true,
                Filled = true,
                Enabled = false,
                radius = 16f
            });
            Controller.RegisterElement("Tether1", new(2)
            {
                thicc = 5f,
                radius = 0f
            });
            Controller.RegisterElement("Tether2", new(2)
            {
                thicc = 5f,
                radius = 0f
            });
            Controller.RegisterElement("SelfTetherReminder", new(1)
            {
                Enabled = false,
                refActorType = 1,
                radius = 0,
                overlayVOffset = 2f,
                overlayTextColor = ImGuiColors.DalamudWhite.ToUint()
            });
            Controller.RegisterElement("SelfTower", new(1)
            {
                Enabled = false,
                refActorComparisonType = 2,
                radius = 3f,
                thicc = 7f,
                overlayText = "Take tower",
                overlayTextColor = 0xFF000000,
                tether = true,
                overlayBGColor = ImGuiColors.ParsedPink.ToUint()
            });
        }

        public override void OnUpdate()
        {
            if(TetherOrder.Count == 8)
            {
                UpdateTethers();
            }
        }

        public override void OnTetherCreate(uint source, uint target, byte data2, byte data3, byte data5)
        {
            if (IsOmega(target, out _))
            {
                TetheredPlayers.Add(source);
                //UpdateTethers();
            }
        }

        public override void OnTetherRemoval(uint source, byte data2, byte data3, byte data5)
        {
            TetheredPlayers.Remove(source);
            //UpdateTethers();
        }

        void UpdateTethers()
        {
            var items = TetheredPlayers.ToArray();
            if(items.Length >= 2)
            {
                var omega = GetOmega();
                {
                    var cTowers = Towers.TakeLast(2).ToArray();
                    if (cTowers.Length == 2)
                    {
                        {
                            if (Controller.TryGetElementByName("dbg1", out var e))
                            {
                                e.Enabled = true;
                                e.refActorObjectID = cTowers[0];
                                e.overlayText = Conf.Debug?$"{(MathHelper.GetRelativeAngle(new(100f, 100f), cTowers[0].GetObject().Position.ToVector2()) + 360 - 45) % 360}":"";
                            }
                        }
                        {
                            if (Controller.TryGetElementByName("dbg2", out var e))
                            {
                                e.Enabled = true;
                                e.refActorObjectID = cTowers[1];
                                e.overlayText = Conf.Debug?$"{(MathHelper.GetRelativeAngle(new(100f, 100f), cTowers[1].GetObject().Position.ToVector2()) + 360 - 45) % 360}":"";
                            }
                        }
                    }
                }

                {
                    if(Controller.TryGetElementByName("SelfTetherReminder", out var e))
                    {
                        if (IsTakingCurrentTether(Svc.ClientState.LocalPlayer.ObjectId))
                        {
                            e.Enabled = true;
                            if (items.Contains(Svc.ClientState.LocalPlayer.ObjectId))
                            {
                                e.overlayBGColor = ImGuiColors.HealerGreen.ToUint();
                                e.overlayFScale = 1;
                                e.overlayText = "Tether";
                            }
                            else
                            {
                                e.overlayBGColor = GradientColor.Get(ImGuiColors.DalamudRed, ImGuiColors.DalamudYellow, 500).ToUint();
                                e.overlayTextColor = 0xFF000000;
                                e.overlayFScale = 2;
                                e.overlayText = "!!! PICK UP TETHER !!!";
                            }
                        }
                        else
                        {
                            e.Enabled = false;
                        }
                    }
                }
                {
                    if (Controller.TryGetElementByName("TetherAOE1", out var e))
                    {
                        e.Enabled = IsTakingCurrentTether(items[0]) || Conf.ShowAOEAlways;
                        e.refActorObjectID = items[0];
                    }
                }
                {
                    if (Controller.TryGetElementByName("Tether1", out var e))
                    {
                        e.Enabled = true;
                        e.SetRefPosition(omega.Position);
                        e.SetOffPosition(items[0].GetObject().Position);
                        e.color = (IsTakingCurrentTether(items[0]) ? ImGuiColors.ParsedGreen : GradientColor.Get(ImGuiColors.DalamudYellow, ImGuiColors.DalamudRed, 500)).ToUint();
                    }
                }
                {
                    if (Controller.TryGetElementByName("TetherAOE2", out var e))
                    {
                        e.Enabled = IsTakingCurrentTether(items[1]) || Conf.ShowAOEAlways;
                        e.refActorObjectID = items[1];
                    }
                }
                {
                    if (Controller.TryGetElementByName("Tether2", out var e))
                    {
                        e.Enabled = true;
                        e.SetRefPosition(omega.Position);
                        e.SetOffPosition(items[1].GetObject().Position);
                        e.color = (IsTakingCurrentTether(items[1]) ? ImGuiColors.ParsedGreen : GradientColor.Get(ImGuiColors.DalamudRed, ImGuiColors.DalamudYellow, 500)).ToUint();
                    }
                }
                {
                    if(Controller.TryGetElementByName("SelfTower", out var e))
                    {
                        if (IsTakingCurrentTower(Svc.ClientState.LocalPlayer.ObjectId))
                        {
                            e.Enabled = true;
                            e.color = GradientColor.Get(ImGuiColors.ParsedGreen, 0xFFFFFF00.ToVector4()).ToUint();
                            var currentTowers = Towers.GetPairNumber(GetCurrentMechanicStep()).OrderBy(x => (MathHelper.GetRelativeAngle(new(100f, 100f), x.GetObject().Position.ToVector2()) + 360-45) % 360).ToArray();
                            if(currentTowers.Length == 2)
                            {
                                if(!Conf.PlayerToSwap.IsNullOrEmpty() && Svc.Objects.Any(x => x is PlayerCharacter pc && pc.Name.ToString() == Conf.PlayerToSwap && pc.StatusList.Any(z => z.StatusId == GetDebuffByNumber(GetCurrentMechanicStep()) ) ) )
                                {
                                    e.refActorObjectID = currentTowers[Conf.MyDirection == Direction.Counter_clockwise ? 1 : 0];
                                }
                                else
                                {
                                    e.refActorObjectID = currentTowers[Conf.MyDirection == Direction.Counter_clockwise ? 0 : 1];
                                }
                            }
                        }
                        else
                        {
                            e.Enabled = false;
                        }
                    }
                }
            }
            else
            {
                Controller.GetElementByName("TetherAOE1").Enabled = false;
                Controller.GetElementByName("TetherAOE2").Enabled = false;
                Controller.GetElementByName("Tether1").Enabled = false;
                Controller.GetElementByName("Tether2").Enabled = false;
                Controller.GetElementByName("SelfTetherReminder").Enabled = false;
                Controller.GetElementByName("dbg1").Enabled = false;
                Controller.GetElementByName("dbg2").Enabled = false;
            }
        }

        bool IsTakingCurrentTether(uint p)
        {
            var step = GetCurrentMechanicStep();
            return TetherOrder.GetPairNumber(step).Contains(p);
        }

        bool IsTakingCurrentTower(uint p)
        {
            var step = GetCurrentMechanicStep();
            return TowerOrder.GetPairNumber(step).Contains(p);
        }

        public override void OnObjectCreation(nint newObjectPtr)
        {
            new TickScheduler(delegate
            {
                var obj = Svc.Objects.FirstOrDefault(x => x.Address == newObjectPtr);
                if (obj != null)
                {
                    if (obj.ObjectKind == Dalamud.Game.ClientState.Objects.Enums.ObjectKind.EventObj)
                    {
                        PluginLog.Information($"Event obj spawn: {obj} {obj.DataId}");
                    }
                    if (obj.DataId == 2013244)
                    {
                        Towers.Add(obj.ObjectId);
                        if (TowerOrder.Count == 0)
                        {
                            GetPlayersWithNumber(1).Each(x => TowerOrder.Add(x.ObjectId));
                            GetPlayersWithNumber(2).Each(x => TowerOrder.Add(x.ObjectId));
                            GetPlayersWithNumber(3).Each(x => TowerOrder.Add(x.ObjectId));
                            GetPlayersWithNumber(4).Each(x => TowerOrder.Add(x.ObjectId));
                            GetPlayersWithNumber(3).Each(x => TetherOrder.Add(x.ObjectId));
                            GetPlayersWithNumber(4).Each(x => TetherOrder.Add(x.ObjectId));
                            GetPlayersWithNumber(1).Each(x => TetherOrder.Add(x.ObjectId));
                            GetPlayersWithNumber(2).Each(x => TetherOrder.Add(x.ObjectId));
                        }
                    }
                }
            });
        }

        public override void OnMessage(string Message)
        {
            if(Message.Contains("31491 (7695>31491)")) //starts casting program loop
            {
                Reset();
            }
        }

        public override void OnDirectorUpdate(DirectorUpdateCategory category)
        {
            if(category.EqualsAny(DirectorUpdateCategory.Commence, DirectorUpdateCategory.Recommence, DirectorUpdateCategory.Wipe))
            {
                Reset();
            }
        }

        void Reset()
        {
            TetheredPlayers.Clear();
            UpdateTethers();
            Towers.Clear();
            TowerOrder.Clear();
            TetherOrder.Clear();
        }

        int GetCurrentMechanicStep()
        {
            if (GetPlayersWithNumber(1).Any()) return 1;
            if (GetPlayersWithNumber(2).Any()) return 2;
            if (GetPlayersWithNumber(3).Any()) return 3;
            if (GetPlayersWithNumber(4).Any()) return 4;
            return 0;
        }

        IEnumerable<PlayerCharacter> GetPlayersWithNumber(int n)
        {
            var debuff = GetDebuffByNumber(n);
            foreach(var x in Svc.Objects)
            {
                if(x is PlayerCharacter p && p.StatusList.Any(z => z.StatusId == debuff))
                {
                    yield return (PlayerCharacter)x;
                }
            }
        }

        int GetDebuffByNumber(int n)
        {
            if (n == 1) return 3004;
            if (n == 2) return 3005;
            if (n == 3) return 3006;
            if (n == 4) return 3451;
            throw new Exception($"Invalid GetDebuffByNumber query {n}");
        }

        BattleChara? GetOmega()
        {
            return Svc.Objects.FirstOrDefault(x => x is BattleChara o && o.NameId == 7695 && o.IsTargetable()) as BattleChara;
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
            ImGui.Checkbox($"Display AOE under incorrect tether", ref Conf.ShowAOEAlways);
            ImGui.SetNextItemWidth(150f);
            ImGuiEx.EnumCombo("My tower direction, starting from NorthEast", ref Conf.MyDirection);
            ImGui.SetNextItemWidth(120f);
            ImGui.InputText($"My tower swap partner (leave empty if you are non-swap)", ref Conf.PlayerToSwap, 50);
            ImGui.SameLine();
            ImGui.SetNextItemWidth(120f);
            if (ImGui.BeginCombo("##partysel","Select from party"))
            {
                FakeParty.Get().Each((x) => { if (ImGui.Selectable(x.Name.ToString())) Conf.PlayerToSwap = x.Name.ToString(); });
                ImGui.EndCombo();
            }

            if (ImGui.CollapsingHeader("Debug"))
            {
                ImGui.Checkbox($"Debug info", ref Conf.Debug);
                foreach(var x in TetheredPlayers)
                {
                    ImGuiEx.Text($"Tether Player: {x} {x.GetObject()}");
                }
                ImGui.Separator();
                TetherOrder.Each(x => ImGuiEx.Text($"Tether order: {x.GetObject()}"));
                TowerOrder.Each(x => ImGuiEx.Text($"Tower order: {x.GetObject()}"));
                ImGuiEx.Text($"GetCurrentMechanicStep() {GetCurrentMechanicStep()}");
                Towers.Each(x => ImGuiEx.Text($"Towers: {x.GetObject()?.Position.ToString() ?? "unk position"} {MathHelper.GetRelativeAngle(new(100, 100), x.GetObject().Position.ToVector2())}"));
            }
        }

        public class Config : IEzConfig
        {
            public Vector4 TetherAOECol = new(0f, 0f, 1f, 0.3f);
            public bool ShowAOEAlways = false;
            public string PlayerToSwap = "";
            public Direction MyDirection = Direction.Counter_clockwise;
            public bool Debug = false;
        }

        public enum Direction { Clockwise, Counter_clockwise}
    }

    internal static class ProgramLoopExtensions
    {
        internal static IEnumerable<T> GetPairNumber<T>(this IEnumerable<T> e, int n)
        {
            var s = e.ToArray();
            if (n == 1)
            {
                yield return s[0];
                yield return s[1];
            }
            if (n == 2)
            {
                yield return s[2];
                yield return s[3];
            }
            if (n == 3)
            {
                yield return s[4];
                yield return s[5];
            }
            if (n == 4)
            {
                yield return s[6];
                yield return s[7];
            }
        }
    }
}