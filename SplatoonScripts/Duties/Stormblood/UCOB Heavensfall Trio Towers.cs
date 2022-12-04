using Dalamud.Game.ClientState.Objects.Types;
using ECommons;
using ECommons.Configuration;
using ECommons.DalamudServices;
using ECommons.GameFunctions;
using ECommons.ImGuiMethods;
using ECommons.MathHelpers;
using ImGuiNET;
using Splatoon;
using Splatoon.SplatoonScripting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SplatoonScriptsOfficial.Duties.Stormblood;

public class UCOB_Heavensfall_Trio_Towers : SplatoonScript
{
    public override HashSet<uint> ValidTerritories => new() { 733 };

    public override Metadata? Metadata => new(2, "NightmareXIV");

    public override void OnSetup()
    {
        for(var i = 0; i < 8; i++)
        {
            this.Controller.TryRegisterElement($"tower{i}", new(0) { Enabled = false, radius = 3f, thicc = 2f });
        }
    }

    public override void OnUpdate()
    {
        var towers = FindTowers();
        if (towers.Count() == 8 && FindNael().NotNull(out var nael))
        {
            var zeroAngle = (int)(MathHelper.GetRelativeAngle(Vector2.Zero, nael.Position.ToVector2()) - (int)this.Controller.GetConfig<Config>().NaelTowerPos + 360) % 360;
            var i = 0;
            foreach(var x in towers.OrderBy(z => (int)(MathHelper.GetRelativeAngle(Vector2.Zero, z.Position.ToVector2()) - zeroAngle + 360) % 360 ))
            {
                if(this.Controller.TryGetElementByName($"tower{i}", out var e))
                {
                    SetPos(e, x.Position);
                    e.overlayText = $"Tower {(TowerPosition)i}";
                    if(i == (int)this.Controller.GetConfig<Config>().TowerNum)
                    {
                        e.Enabled = true;
                        e.tether = true;
                        e.thicc = 10;
                    }
                    else
                    {
                        if (this.Controller.GetConfig<Config>().ShowAll)
                        {
                            e.Enabled = true;
                            e.tether = false;
                            e.thicc = 2;
                        }
                        else
                        {
                            e.Enabled = false;
                        }
                    }
                    i++;
                }
            }
        }
        else
        {
            DisableAllElements();
        }
    }

    public override void OnDisable()
    {
        DisableAllElements();
    }

    void DisableAllElements()
    {
        for (var i = 0; i < 8; i++)
        {
            if (this.Controller.TryGetElementByName($"tower{i}", out var e))
            {
                e.Enabled = false;
            }
        }
    }

    void SetPos(Element e, Vector3 pos)
    {
        e.refX = pos.X;
        e.refY = pos.Z;
        e.refZ = pos.Y;
    }

    IEnumerable<BattleChara> FindTowers()
    {
        return Svc.Objects.Where(x => x is BattleChara c && c.IsCasting && c.CastActionId == 9951).Cast<BattleChara>();
    }

    BattleChara? FindNael()
    {
        return (BattleChara?)Svc.Objects.Where(x => x is BattleChara c && c.NameId == 2612 && c.IsCharacterVisible()).FirstOrDefault();
    }

    public override void OnSettingsDraw()
    {
        ImGui.SetNextItemWidth(100f);
        ImGuiEx.EnumCombo("Your designated tower", ref this.Controller.GetConfig<Config>().TowerNum);
        ImGui.SetNextItemWidth(100f);
        ImGuiEx.EnumCombo("Tower directly at Nael", ref this.Controller.GetConfig<Config>().NaelTowerPos);
        ImGui.Checkbox("Display all towers", ref this.Controller.GetConfig<Config>().ShowAll);
    }

    public class Config : IEzConfig
    {
        public TowerPosition TowerNum = TowerPosition.Right_1;
        public bool ShowAll = false;
        public NaelTower NaelTowerPos = NaelTower.Right_1;
    }

    public enum NaelTower
    {
        Left_1 = -1, Right_1 = 1
    }

    public enum TowerPosition : int
    {
        Right_1 = 0, Right_2 = 1, Right_3 = 2, Right_4 = 3, Left_1 = 7, Left_2 = 6, Left_3 = 5, Left_4 = 4
    }
}
