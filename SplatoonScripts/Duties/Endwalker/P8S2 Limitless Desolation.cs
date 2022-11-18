using Dalamud.Interface.Colors;
using ECommons;
using ECommons.DalamudServices;
using ECommons.GameFunctions;
using ECommons.ImGuiMethods;
using ECommons.Logging;
using Splatoon.SplatoonScripting;
using Splatoon.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SplatoonScriptsOfficial.Duties.Endwalker
{
    public class P8S2_Limitless_Desolation : SplatoonScript
    {
        public override HashSet<uint> ValidTerritories => new() { 1088 };
        long HideAt = 0;

        public override void OnSetup()
        {
            if(!this.Controller.TryRegisterElement("TowerDisplay", new(0)
            {
                Enabled = false,
                thicc = 5,
                radius = 4,
                tether = true
            }))
            {
                DuoLog.Error("Could not register layout");
            }
        }

        public override void OnUpdate()
        {
            if(HideAt != 0 && this.Controller.TryGetElementByName("TowerDisplay", out var e))
            {
                if (Environment.TickCount64 > HideAt)
                {
                    e.Enabled = false;
                    HideAt = 0;
                }
                e.color = GradientColor.Get(Colors.Red.ToVector4(), Colors.Green.ToVector4()).ToUint();
            }
        }

        public override void OnCombatStart()
        {
            if(this.Controller.TryGetElementByName("TowerDisplay", out var e))
            {
                e.Enabled = false;
                HideAt = 0;
            }
        }

        public override void OnMapEffect(uint position, ushort data1, ushort data2)
        {
            if (data1 == 1 && data2 == 2 
                && Svc.ClientState.LocalPlayer?.StatusList.Any(x => x.StatusId == 2098 && x.RemainingTime > 7.5f) == true 
                && EffectData.TryGetValue(position, out var x) 
                && Positions.TryGetValue(x, out var loc) 
                && (Svc.ClientState.LocalPlayer?.GetRole() == CombatRole.DPS) == IsDpsPosition(loc) 
                && this.Controller.TryGetElementByName("TowerDisplay", out var e))
            {
                e.Enabled = true;
                e.refX = loc.X;
                e.refY = loc.Y;
                HideAt = Environment.TickCount64 + 11000;
                PluginLog.Information($"Displaying tower...");
            }
        }

        bool IsDpsPosition(Vector2 v)
        {
            return v.X > 100;
        }

        Dictionary<TowerPosition, Vector2> Positions = new()
        {
            {TowerPosition.Top1, new(85, 85) },
            {TowerPosition.Top2, new(95, 85) },
            {TowerPosition.Top3, new(105, 85) },
            {TowerPosition.Top4, new(115, 85) },
            {TowerPosition.Mid1, new(85, 95) },
            {TowerPosition.Mid2, new(95, 95) },
            {TowerPosition.Mid3, new(105, 95) },
            {TowerPosition.Mid4, new(115, 95) },
            {TowerPosition.Bot1, new(85, 105) },
            {TowerPosition.Bot2, new(95, 105) },
            {TowerPosition.Bot3, new(105, 105) },
            {TowerPosition.Bot4, new(115, 105) },
        };

        Dictionary<uint, TowerPosition> EffectData = new()
        {
            {70, TowerPosition.Top1 },
            {71, TowerPosition.Top2 },
            {72, TowerPosition.Top3 },
            {73, TowerPosition.Top4 },
            {74, TowerPosition.Mid1 },
            {5,  TowerPosition.Mid2 },
            {6,  TowerPosition.Mid3 },
            {75, TowerPosition.Mid4 },
            {82, TowerPosition.Bot1 },
            {7,  TowerPosition.Bot2 },
            {8,  TowerPosition.Bot3 },
            {83, TowerPosition.Bot4 },
        };

        enum TowerPosition
        {
            Top1, Top2, Top3, Top4,
            Mid1, Mid2, Mid3, Mid4,
            Bot1, Bot2, Bot3, Bot4,
        }
    }
}
