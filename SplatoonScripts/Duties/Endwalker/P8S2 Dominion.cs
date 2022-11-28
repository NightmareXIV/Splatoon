using Dalamud.Game.ClientState.Objects.SubKinds;
using Dalamud.Game.ClientState.Objects.Types;
using Dalamud.Utility;
using ECommons;
using ECommons.DalamudServices;
using ECommons.GameFunctions;
using ECommons.Logging;
using ECommons.MathHelpers;
using ImGuiNET;
using Splatoon.SplatoonScripting;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            this.Controller.TryRegisterElement("Tower1", new(0) { Enabled = false, overlayText = "tower 1" });
            this.Controller.TryRegisterElement("Tower2", new(0) { Enabled = false, overlayText = "tower 2" });
            this.Controller.TryRegisterElement("Tower3", new(0) { Enabled = false, overlayText = "tower 3" });
            this.Controller.TryRegisterElement("Tower4", new(0) { Enabled = false, overlayText = "tower 4" });
            this.Controller.TryRegisterElement("MyTower", new(0) { Enabled = false, thicc=10, tether=true, radius = 0 });
        }

        public override void OnMessage(string Message)
        {
            if (Message.Contains("(11402>31193)"))
            {
                ResetMechanic();
                Stage = 1;
                DuoLog.Information($"Stage 1");
            }
        }

        void ResetMechanic()
        {
            { if (this.Controller.TryGetElementByName($"Tower1", out var e)) e.Enabled = false; }
            { if (this.Controller.TryGetElementByName($"Tower2", out var e)) e.Enabled = false; }
            { if (this.Controller.TryGetElementByName($"Tower3", out var e)) e.Enabled = false; }
            { if (this.Controller.TryGetElementByName($"Tower4", out var e)) e.Enabled = false; }
        }

        public override void OnUpdate()
        {
            //tower cast: 31196
            //debuff:   Earth Resistance Down II (3372)

            if (Svc.ClientState.LocalPlayer == null) return;

            if (Stage == 1)
            {
                var playersSecondTowers = Svc.Objects.Where(x => x is PlayerCharacter pc && pc.StatusList.Any(x => x.StatusId == 3372));
                if (playersSecondTowers.Count() == 4)
                {
                    FirstPlayers = Svc.Objects.Where(x => x is PlayerCharacter pc && !pc.StatusList.Any(x => x.StatusId == 3372) && IsRoleMatching(pc)).Select(x => x.ObjectId).ToList();
                    SecondPlayers = playersSecondTowers.Where(x => x is PlayerCharacter pc && IsRoleMatching(pc)).Select(x => x.ObjectId).ToList();
                    DuoLog.Information($"First towers: {FirstPlayers.Print()}\nSecond towers: {SecondPlayers.Print()}");
                    Stage = 2;
                    DuoLog.Information($"Stage 2");
                }
            }
            else if(Stage == 2)
            {
                var towers = GetTowers();
                if(towers.Count() == 4)
                {
                    if (Controller.TryGetElementByName("MyTower", out var e)) e.Enabled = false;
                    Process(towers.OrderBy(GetAngle).ToArray(), FirstPlayers);
                }
                else if(towers.Count() == 8)
                {
                    if (Controller.TryGetElementByName("MyTower", out var e)) e.Enabled = false;
                    towers = GetEarliestTowers();
                    Process(towers.OrderBy(GetAngle).ToArray(), SecondPlayers);
                }
                else if(!towers.Any())
                {
                    if (Controller.TryGetElementByName("MyTower", out var e)) e.Enabled = false;
                    Stage = 0;
                    DuoLog.Information($"Reset");
                }
            }
            else if(Stage == 3)
            {
                if (!GetTowers().Any())
                {
                    if (Controller.TryGetElementByName("MyTower", out var e)) e.Enabled = false;
                    Stage = 0;
                    DuoLog.Information($"Reset");
                }
            }
        }

        void Process(BattleChara[] towers, List<uint> players)
        {
            if (players.Contains(Svc.ClientState.LocalPlayer!.ObjectId) && Controller.TryGetElementByName("MyTower", out var e))
            {
                var prio = GetPriority().Where(x => players.Select(z => z.GetObject()!.Name.ToString()).Contains(x)).ToArray();
                Svc.Chat.Print($"Priority: {prio.Select(x => x.ToString()).Join(", ")}");
                if (prio[0] == Svc.ClientState.LocalPlayer.Name.ToString())
                {
                    e.Enabled = true;
                    var pos = Svc.ClientState.LocalPlayer.GetRole() == CombatRole.DPS ? 2 : 0;
                    e.refX = towers[pos].Position.X;
                    e.refY = towers[pos].Position.Z;
                    e.refZ = towers[pos].Position.Y;
                    //first prio
                }
                else
                {
                    e.Enabled = true;
                    var pos = Svc.ClientState.LocalPlayer.GetRole() == CombatRole.DPS ? 3 : 1;
                    e.refX = towers[pos].Position.X;
                    e.refY = towers[pos].Position.Z;
                    e.refZ = towers[pos].Position.Y;
                    //second prio
                }
                Stage = 3;
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
            var x = this.Controller.GetOption<string>("Priority");
            if(x != null)
            {
                var strings = x.Split("\n").Select(x => x.Trim()).Where(x => !x.IsNullOrWhitespace());
                return strings.ToList();
            }
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
            var txt = this.Controller.GetOption<string>("Priority") ?? "";
            if(ImGui.InputTextMultiline("##prio", ref txt, 5000, new(ImGui.GetContentRegionAvail().X, 200)))
            {
                this.Controller.SetOption("Priority", txt);
            }
        }
    }
}
