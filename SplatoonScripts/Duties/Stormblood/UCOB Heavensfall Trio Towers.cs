using Dalamud.Game.ClientState.Objects.Types;
using Dalamud.Logging;
using ECommons;
using ECommons.Configuration;
using ECommons.DalamudServices;
using ECommons.GameFunctions;
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

    public override Metadata? Metadata => new(1, "NightmareXIV");

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
        if (towers.Any() && FindNael().NotNull(out var nael))
        {
            var zeroAngle = MathHelper.GetRelativeAngle(Vector2.Zero, nael.Position.ToVector2());
            var i = 0;
            foreach(var x in towers.OrderBy(z => (int)(MathHelper.GetRelativeAngle(Vector2.Zero, z.Position.ToVector2()) - zeroAngle + 360) % 360 ))
            {
                if(this.Controller.TryGetElementByName($"tower{i}", out var e))
                {
                    SetPos(e, x.Position);
                    e.overlayText = $"Tower {i + 1}";
                    i++;
                    if(i == this.Controller.GetConfig<Config>().TowerNum)
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
                }
            }
        }
        else
        {
            for (var i = 0; i < 8; i++)
            {
                if(this.Controller.TryGetElementByName($"tower{i}", out var e))
                {
                    e.Enabled = false;
                }
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
        ImGui.InputInt("Your designated tower", ref this.Controller.GetConfig<Config>().TowerNum.ValidateRange(1, 8));
        ImGui.Checkbox("Display all towers", ref this.Controller.GetConfig<Config>().ShowAll);
    }

    public class Config : IEzConfig
    {
        public int TowerNum = 1;
        public bool ShowAll = false;
    }
}
