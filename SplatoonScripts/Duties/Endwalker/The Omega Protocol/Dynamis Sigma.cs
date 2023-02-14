using Dalamud.Game.ClientState.Objects.SubKinds;
using Dalamud.Game.ClientState.Objects.Types;
using ECommons;
using ECommons.Configuration;
using ECommons.DalamudServices;
using ECommons.Hooks.ActionEffectTypes;
using ECommons.Logging;
using ECommons.MathHelpers;
using ECommons.Reflection;
using FFXIVClientStructs.FFXIV.Component.GUI;
using ImGuiNET;
using Newtonsoft.Json;
using Splatoon.SplatoonScripting;
using Splatoon.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SplatoonScriptsOfficial.Duties.Endwalker.The_Omega_Protocol
{
    public class Dynamis_Sigma : SplatoonScript
    {
        public override HashSet<uint> ValidTerritories => new() { 1122 };

        public override Metadata? Metadata => new(3, "NightmareXIV");

        public const uint TowerSingle = 2013245;
        public const uint TowerDual = 2013246;
        public const uint TowerAny = 2013244;

        public const uint GlitchFar = 3428;
        public const uint GlitchClose = 3427;

        Vector3 OmegaPos = Vector3.Zero;

        Config Conf => Controller.GetConfig<Config>();

        GameObject[] GetTowers() => Svc.Objects.Where(x => x.DataId.EqualsAny<uint>(TowerSingle, TowerDual)).ToArray();

        public override void OnSetup()
        {
            for (int i = 0; i < 6; i++)
            {
                Controller.RegisterElementFromCode($"{i}", "{\"Enabled\":false,\"Name\":\"\",\"radius\":2.5,\"Donut\":0.5,\"color\":4278255615,\"overlayBGColor\":4110417920,\"overlayTextColor\":4278255615,\"overlayFScale\":2.0,\"overlayPlaceholders\":true,\"thicc\":4.0,\"overlayText\":\"L1\\\\nL2\",\"refActorDataID\":2013244,\"refActorComparisonType\":3}");
            }
            base.OnSetup();
        }

        public override void OnUpdate()
        {
            Off();
            if (Controller.Scene == 6)
            {
                if (GetTowers().Length.EqualsAny(5, 6))
                {
                    var towers = GetTowers().OrderBy(x => GetTowerAngle(x, IsInverted())).ToArray();
                    Queue<string> enumeration = Svc.ClientState.LocalPlayer.StatusList.Any(x => x.StatusId == GlitchFar)? new(Conf.FarTowers) : new(Conf.CloseTowers);
                    for (int i = 0; i < towers.Length; i++)
                    {
                        if (towers[i].DataId == TowerSingle) 
                        {
                            SetTowerAs(i, towers[i], enumeration.Dequeue());
                        }
                        else
                        {
                            SetTowerAs(i, towers[i], enumeration.Dequeue(), enumeration.Dequeue());
                        }
                    }
                }
            }
        }

        /*public override void OnMessage(string Message)
        {
            if (Controller.Scene != 6) return;
            if(Message.EqualsAny("The right arm unit uses Hyper Pulse."))
            {
                OmegaPos = Svc.Objects.FirstOrDefault(x => x.DataId == 15720)?.Position ?? Vector3.Zero;
                PluginLog.Information($"Omega position captured: {OmegaPos}");
            }
        }*/

        public override void OnActionEffect(uint ActionID, ushort animationID, ActionEffectType type, uint sourceID, ulong targetOID, uint damage)
        {
            if(Controller.Scene == 6 && ActionID == 31603)
            {
                OmegaPos = Svc.Objects.FirstOrDefault(x => x.DataId == 15720)?.Position ?? Vector3.Zero;
                DuoLog.Information($"Omega position captured: {OmegaPos}");
            }
        }

        void Off()
        {
            Controller.GetRegisteredElements().Each(x => x.Value.Enabled = false);
        }

        void SetTowerAs(int tower, GameObject obj, params string[] s)
        {
            if(Controller.TryGetElementByName($"{tower}", out var t))
            {
                Array.Sort(s);
                t.Enabled = true;
                t.SetRefPosition(obj.Position);
                t.overlayText = s.Join("\n") + (Conf.Angle?$"\n{GetTowerAngle(obj)}/{GetTowerAngle(obj, IsInverted())}":"");
            }
            else
            {
                DuoLog.Error($"Could not obtain element {tower}");
            }
        }

        float GetTowerAngle(GameObject t, bool inverted = false)
        {
            var z = new Vector3(100, 0, 100);
            var angle = (MathHelper.GetRelativeAngle(z, t.Position) + (inverted?181:1) + 360 - MathHelper.GetRelativeAngle(z, OmegaPos)) % 360;
            return angle;
        }

        bool IsInverted()
        {
            if(Svc.ClientState.LocalPlayer.StatusList.Any(x => x.StatusId == GlitchFar))
            {
                return !GetTowers().Any(x => GetTowerAngle(x) < 3);
            }
            else
            {
                return !GetTowers().Any(x => GetTowerAngle(x).InRange(90, 90+45));
            }
        }

        public override void OnSettingsDraw()
        {
            
            ImGui.PushID("Far");
            if (ImGui.CollapsingHeader("Far towers, clockwise"))
            {
                for (int i = 0; i < 8; i++)
                {
                    ImGui.InputText($"##{i}", ref Conf.FarTowers[i], 50);
                }

                if (ImGui.Button("Reset and reconfigure for UCOB strat (CTRL+click)") && ImGui.GetIO().KeyCtrl)
                {
                    Conf.FarTowers = new Config().FarTowers;
                }
                if (ImGui.Button("Reset and reconfigure for marker (CTRL+click)") && ImGui.GetIO().KeyCtrl)
                {
                    Conf.FarTowers = new Config().FarTowersMarkers;
                }
            }
            ImGui.PopID();
            ImGui.PushID("Near");
            if (ImGui.CollapsingHeader("Close towers, clockwise"))
            {
                for (int i = 0; i < 8; i++)
                {
                    ImGui.InputText($"##{i}", ref Conf.CloseTowers[i], 50);
                }

                if (ImGui.Button("Reset and reconfigure for UCOB strat (CTRL+click)") && ImGui.GetIO().KeyCtrl)
                {
                    Conf.CloseTowers = new Config().CloseTowers;
                }
                if (ImGui.Button("Reset and reconfigure for marker (CTRL+click)") && ImGui.GetIO().KeyCtrl)
                {
                    Conf.CloseTowers = new Config().CloseTowersMarkers;
                }
            }
            ImGui.PopID();
            if (ImGui.CollapsingHeader("Debug"))
            {
                ImGui.InputFloat3("Omega pos", ref OmegaPos);
                if (ImGui.Button("Copy"))
                {
                    ImGui.SetClipboardText(JsonConvert.SerializeObject(OmegaPos));
                }
                ImGui.SameLine();
                if (ImGui.Button("Paste"))
                {
                    GenericHelpers.Safe(() => { OmegaPos = JsonConvert.DeserializeObject<Vector3>(ImGui.GetClipboardText()); });
                }
                ImGui.SameLine();
                ImGui.Checkbox("Display tower angle", ref Conf.Angle);
            }
        }

        public class Config : IEzConfig
        {
            internal readonly string[] FarTowersMarkers = new string[] { "3", "2", "D", "4", "C", "1", "B", "A" };
            internal readonly string[] CloseTowersMarkers = new string[] { "4", "D", "2", "C", "B", "1", "A", "3" };

            public string[] FarTowers = new string[] { "R1", "L1", "R2", "R3", "L4", "R4", "L3", "L2" };
            public string[] CloseTowers = new string[] { "R1", "L4", "R2", "R3", "L3", "R4", "L2", "L1" };
            public bool Angle = false;
        }
    }
}
