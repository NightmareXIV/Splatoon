using Dalamud.Game.ClientState.Objects.SubKinds;
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
        public override Metadata? Metadata => new(4, "NightmareXIV");
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
            Controller.RegisterElement("dbg1", new(1) { Enabled = false, refActorComparisonType = 2, overlayVOffset = 1, radius = 3f, color = Conf.TowerColor1.ToUint() });
            Controller.RegisterElement("dbg2", new(1) { Enabled = false, refActorComparisonType = 2, overlayVOffset = 1, radius = 3f, color = Conf.TowerColor1.ToUint() });
            Controller.RegisterElement("TetherAOE1", new(1) { color = Conf.TetherAOECol.ToUint(), refActorComparisonType = 2, onlyTargetable = true, Filled = true, Enabled = false, radius = 16f });
            Controller.RegisterElement("TetherAOE2", new(1) { color = Conf.TetherAOECol.ToUint(), refActorComparisonType = 2, onlyTargetable = true, Filled = true, Enabled = false, radius = 16f });
            Controller.RegisterElement("Tether1", new(2) { thicc = 5f, radius = 0f });
            Controller.RegisterElement("Tether2", new(2) { thicc = 5f, radius = 0f });
            Controller.RegisterElement("SelfTetherReminder", new(1) { Enabled = false, refActorType = 1, radius = 0, overlayVOffset = 2f, overlayTextColor = ImGuiColors.DalamudWhite.ToUint() });
            Controller.RegisterElement("SelfTower", new(1) { Enabled = false, refActorComparisonType = 2, radius = 3f, thicc = 7f, overlayText = "Take tower", overlayTextColor = 0xFF000000, tether = true, overlayBGColor = ImGuiColors.ParsedPink.ToUint() });
            Controller.TryRegisterLayoutFromCode("Proximity", "~Lv2~{\"Enabled\":false,\"Name\":\"Proximity\",\"Group\":\"\",\"ZoneLockH\":[1122],\"ElementsL\":[{\"Name\":\"\",\"type\":1,\"radius\":0.0,\"color\":4278129920,\"thicc\":4.0,\"refActorPlaceholder\":[\"<2>\",\"<3>\",\"<4>\",\"<5>\",\"<6>\",\"<7>\",\"<8>\"],\"refActorComparisonType\":5,\"tether\":true}],\"MaxDistance\":16.2,\"UseDistanceLimit\":true,\"DistanceLimitType\":1}", out _);
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
                if(Conf.Debug)
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
                                e.overlayBGColor = Conf.ValidTetherColor.ToUint();
                                e.overlayTextColor = Conf.OverlayTextColor.ToUint();
                                e.overlayFScale = 1;
                                e.overlayText = "Tether";
                                if (Conf.UseProximity && Controller.TryGetLayoutByName("Proximity", out var l))
                                {
                                    l.ElementsL[0].color = Conf.ProximityColor.ToUint();
                                    l.Enabled = true;
                                }
                            }
                            else
                            {
                                e.overlayBGColor = GradientColor.Get(Conf.InvalidTetherColor1, Conf.InvalidTetherColor2, 500).ToUint();
                                e.overlayTextColor = Conf.OverlayTextColor.ToUint();
                                e.overlayFScale = Conf.InvalidOverlayScale;
                                e.overlayText = "!!! PICK UP TETHER !!!";
                            }
                        }
                        else
                        {
                            if (Controller.TryGetLayoutByName("Proximity", out var l))
                            {
                                l.Enabled = false;
                            }
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
                        e.color = (IsTakingCurrentTether(items[0]) ? Conf.ValidTetherColor : GradientColor.Get(Conf.InvalidTetherColor1, Conf.InvalidTetherColor2, 500)).ToUint();
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
                        e.color = (IsTakingCurrentTether(items[1]) ? Conf.ValidTetherColor : GradientColor.Get(Conf.InvalidTetherColor1, Conf.InvalidTetherColor2, 500)).ToUint();
                    }
                }
                {
                    if(Controller.TryGetElementByName("SelfTower", out var e))
                    {
                        if (IsTakingCurrentTower(Svc.ClientState.LocalPlayer.ObjectId))
                        {
                            e.Enabled = true;
                            e.color = GradientColor.Get(Conf.TowerColor1, Conf.TowerColor2).ToUint();
                            e.overlayBGColor = e.color;
                            e.overlayTextColor = Conf.OverlayTextColor.ToUint();
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
                if (Controller.TryGetLayoutByName("Proximity", out var l)){l.Enabled = false;}
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
                    if (obj.DataId == 2013244 && GetOmega() != null)
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
            ImGuiEx.Text($"Tethers:");
            ImGui.ColorEdit4("Tether's AOE color", ref Conf.TetherAOECol, ImGuiColorEditFlags.NoInputs);
            ImGui.ColorEdit4("Valid tether color", ref Conf.ValidTetherColor, ImGuiColorEditFlags.NoInputs);
            ImGui.ColorEdit4("##Invalid1", ref Conf.InvalidTetherColor1, ImGuiColorEditFlags.NoInputs);
            ImGui.SameLine();
            ImGui.ColorEdit4("Invalid tether colors", ref Conf.InvalidTetherColor2, ImGuiColorEditFlags.NoInputs);
            ImGui.SetNextItemWidth(100f);
            ImGui.SliderFloat("Invalid tether reminder size", ref Conf.InvalidOverlayScale.ValidateRange(1, 5), 1, 5);
            ImGui.ColorEdit4("Invalid tether reminder color", ref Conf.OverlayTextColor, ImGuiColorEditFlags.NoInputs);
            ImGui.Checkbox($"Display AOE under incorrect tether", ref Conf.ShowAOEAlways);
            ImGui.Checkbox($"Tether AOE proximity detector", ref Conf.UseProximity);
            if (Conf.UseProximity)
            {
                ImGui.SameLine();
                ImGui.ColorEdit4("Proximity circle color", ref Conf.ProximityColor, ImGuiColorEditFlags.NoInputs);
            }

            ImGui.Separator();

            ImGuiEx.Text($"Towers:");
            ImGui.SetNextItemWidth(150f);
            ImGuiEx.EnumCombo("My tower direction, starting from NorthEast", ref Conf.MyDirection);
            ImGuiEx.Text($"My tower swap partner (leave empty if you are non-swap):");
            ImGui.SetNextItemWidth(150f);
            ImGui.InputText($"##swap", ref Conf.PlayerToSwap, 50);
            ImGui.SameLine();
            ImGui.SetNextItemWidth(120f);
            if (ImGui.BeginCombo("##partysel","Select from party"))
            {
                FakeParty.Get().Each((x) => { if (ImGui.Selectable(x.Name.ToString())) Conf.PlayerToSwap = x.Name.ToString(); });
                ImGui.EndCombo();
            }
            ImGui.ColorEdit4("Primary tower color", ref Conf.TowerColor1, ImGuiColorEditFlags.NoInputs);
            ImGui.SameLine();
            ImGui.ColorEdit4("Secondary tower color", ref Conf.TowerColor2, ImGuiColorEditFlags.NoInputs);

            ImGui.Separator();
            if(ImGui.Button("Apply settings"))
            {
                this.SetupElements();
            }

            ImGui.Separator();

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
            public Vector4 TowerColor1 = 0xFFFF00FF.ToVector4();
            public Vector4 TowerColor2 = 0xFFFFFF00.ToVector4();
            public Vector4 ValidTetherColor = ImGuiColors.ParsedGreen;
            public Vector4 InvalidTetherColor1 = ImGuiColors.DalamudOrange;
            public Vector4 InvalidTetherColor2 = ImGuiColors.DalamudRed;
            public Vector4 OverlayTextColor = 0xFF000000.ToVector4();
            public bool UseProximity = false;
            public Vector4 ProximityColor = ImGuiColors.ParsedBlue;
            public float InvalidOverlayScale = 2f;
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
            if (n == 1 && s.Length >=2)
            {
                yield return s[0];
                yield return s[1];
            }
            if (n == 2 && s.Length >= 4)
            {
                yield return s[2];
                yield return s[3];
            }
            if (n == 3 && s.Length >= 6)
            {
                yield return s[4];
                yield return s[5];
            }
            if (n == 4 && s.Length >= 8)
            {
                yield return s[6];
                yield return s[7];
            }
        }
    }
}
