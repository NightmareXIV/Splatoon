using Dalamud.Game.ClientState.Actors.Types.NonPlayer;
using Dalamud.Plugin;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using Num = System.Numerics;

namespace Splatoon
{
    unsafe class Gui : IDisposable
    {
        readonly Splatoon p;
        public Gui(Splatoon p)
        {
            this.p = p;
            p._pi.UiBuilder.OnBuildUi += Draw;
        }

        public void Dispose()
        {
            p._pi.UiBuilder.OnBuildUi -= Draw;
        }

        void Draw()
        {
            try
            {
                if(p.Config.verboselog) p.Log("d:begin draw sequence");
                int uid = 0;
                if (p._pi.ClientState == null || p._pi.ClientState.LocalPlayer == null) return;
                if (p._pi.ClientState.LocalPlayer.Address == IntPtr.Zero) //let's try to catch this event actually
                {
                    try { PluginLog.Fatal("Pointer to LocalPlayer.Address is zero"); } catch (Exception) { }
                    p.Log("Pointer to LocalPlayer.Address is zero");
                    return;
                } 
                ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, new Num.Vector2(0, 0));
                ImGui.Begin("Splatoon ring", ImGuiWindowFlags.NoInputs | ImGuiWindowFlags.NoNav | ImGuiWindowFlags.NoTitleBar
                    | ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoBackground);
                ImGui.SetWindowPos(new Num.Vector2(0, 0));
                if (p.Config.verboselog) p.Log("d:1");
                ImGui.SetWindowSize(ImGui.GetIO().DisplaySize);
                if (p.Config.verboselog) p.Log("d:2");
                foreach (var i in p.Config.Layouts.Values)
                {
                    if (p.Config.verboselog) p.Log("d:3 "+i);
                    if (!i.Enabled) continue;
                    if (i.ZoneLock != 0 && i.ZoneLock != p._pi.ClientState.TerritoryType) continue;
                    if ((i.DCond == 1 || i.DCond == 3) && !p._pi.ClientState.Condition[Dalamud.Game.ClientState.ConditionFlag.InCombat]) continue;
                    if ((i.DCond == 2 || i.DCond == 3) && !p._pi.ClientState.Condition[Dalamud.Game.ClientState.ConditionFlag.BoundByDuty]) continue;
                    if (i.DCond == 4 && !(p._pi.ClientState.Condition[Dalamud.Game.ClientState.ConditionFlag.InCombat]
                        || p._pi.ClientState.Condition[Dalamud.Game.ClientState.ConditionFlag.BoundByDuty])) continue;
                    if (i.Visibility == 1)
                    {
                        if (!p._pi.ClientState.Condition[Dalamud.Game.ClientState.ConditionFlag.InCombat]) continue;
                        var tic = DateTimeOffset.Now.ToUnixTimeSeconds() - p.CombatStarted;
                        if (tic < i.BattleTimeBegin || tic > i.BattleTimeEnd) continue;
                    }
                    foreach (var e in i.Elements.Values.ToArray())
                    {
                        if (!e.Enabled) continue;
                        float x = 0f, y = 0f, z = 0f;
                        bool draw = false;
                        float radius = e.radius;
                        if (e.type == 0)
                        {
                            draw = true;
                            x = e.refX;
                            y = e.refY;
                            z = e.refZ;
                        }
                        else if (e.type == 1)
                        {
                            if (e.refActorType == 1)
                            {
                                draw = true;
                                x = p._pi.ClientState.LocalPlayer.Position.X;
                                y = p._pi.ClientState.LocalPlayer.Position.Y;
                                z = p._pi.ClientState.LocalPlayer.Position.Z;
                            }
                            else if (e.refActorType == 2 && p._pi.ClientState.Targets.CurrentTarget != null
                                && p._pi.ClientState.Targets.CurrentTarget is BattleNpc
                                && p._pi.ClientState.Targets.CurrentTarget.Address != IntPtr.Zero)
                            {
                                draw = true;
                                x = p._pi.ClientState.Targets.CurrentTarget.Position.X;
                                y = p._pi.ClientState.Targets.CurrentTarget.Position.Y;
                                z = p._pi.ClientState.Targets.CurrentTarget.Position.Z;
                                if (p.Config.verboselog) p.Log("d:ptr1");
                                if (e.includeHitbox) radius += *(float*)(p._pi.ClientState.Targets.CurrentTarget.Address + 0xC0);
                            }
                            else if (e.refActorType == 0 && e.refActorName.Length > 0)
                            {
                                foreach (var a in p._pi.ClientState.Actors)
                                {
                                    if (a.Name.ToLower().Contains(e.refActorName.ToLower())
                                         && a.Address != IntPtr.Zero)
                                    {
                                        draw = true;
                                        x = a.Position.X;
                                        y = a.Position.Y;
                                        z = a.Position.Z;
                                        if (p.Config.verboselog) p.Log("d:ptr2");
                                        if (e.includeHitbox) radius += *(float*)(a.Address + 0xC0);
                                        break;
                                    }
                                }
                            }
                            if (e.includeOwnHitbox)
                            {
                                if (p.Config.verboselog) p.Log("d:ptr3");
                                radius += *(float*)(p._pi.ClientState.LocalPlayer.Address + 0xC0);
                            }
                        }
                        if (!draw) continue;
                        if (e.thicc > 0)
                        {
                            if (radius > 0)
                            {
                                if (p.Config.verboselog) p.Log("d:draw1");
                                DrawRingWorld(x + e.offX, y + e.offY, z + e.offZ, radius, 100, e.thicc, e.color);
                            }
                            else
                            {
                                if (p.Config.verboselog) p.Log("d:draw2");
                                DrawPoint(x + e.offX, y + e.offY, z + e.offZ, e.thicc, e.color);
                            }
                        }
                        if (e.overlayText.Length > 0)
                        {
                            if (p.Config.verboselog) p.Log("d:txt1");
                            if (p._pi.Framework.Gui.WorldToScreen(
                                new SharpDX.Vector3(x + e.offX, z + e.offZ + e.overlayVOffset, y + e.offY),
                                out SharpDX.Vector2 pos))
                            {
                                var size = ImGui.CalcTextSize(e.overlayText);
                                size = new Num.Vector2(size.X + 10f, size.Y + 10f);
                                ImGui.SetNextWindowPos(new Num.Vector2(pos.X - size.X / 2, pos.Y - size.Y / 2));
                                ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, new Num.Vector2(5, 5));
                                ImGui.PushStyleVar(ImGuiStyleVar.ChildRounding, 10f);
                                ImGui.PushStyleColor(ImGuiCol.ChildBg, ImGui.ColorConvertU32ToFloat4(e.overlayBGColor));
                                ImGui.BeginChild("##child" + e.overlayText + ++uid, size, false,
                                    ImGuiWindowFlags.NoInputs | ImGuiWindowFlags.NoNav
                                    | ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.AlwaysUseWindowPadding);
                                ImGui.TextColored(ImGui.ColorConvertU32ToFloat4(e.overlayTextColor), e.overlayText);
                                ImGui.EndChild();
                                ImGui.PopStyleColor();
                                ImGui.PopStyleVar(2);
                            }
                        }
                    }
                }
                ImGui.End();
                ImGui.PopStyleVar();
                if (p.Config.verboselog) p.Log("d:endsequence");
            }
            catch(Exception e)
            {
                p.Log("Splatoon exception: please report it to developer", true);
                p.Log(e.Message, true);
                p.Log(e.StackTrace, true);
            }
        }

        public void DrawRingWorld(float x, float y, float z, float radius, int num_segments, float thicc, uint colour)
        {
            int seg = num_segments / 2;

            for (int i = 0; i <= num_segments; i++)
            {
                p._pi.Framework.Gui.WorldToScreen(new SharpDX.Vector3(x + (radius * (float)Math.Sin((Math.PI / seg) * i)), z, y + (radius * (float)Math.Cos((Math.PI / seg) * i))), out SharpDX.Vector2 pos);
                ImGui.GetWindowDrawList().PathLineTo(new Num.Vector2(pos.X, pos.Y));
            }
            ImGui.GetWindowDrawList().PathStroke(colour, true, thicc);
        }

        public void DrawPoint(float x, float y, float z, float thicc, uint col)
        {
            p._pi.Framework.Gui.WorldToScreen(new SharpDX.Vector3(x, z, y), out SharpDX.Vector2 pos);
            ImGui.GetWindowDrawList().AddCircleFilled(
                new Num.Vector2(pos.X, pos.Y),
                thicc,
                ImGui.GetColorU32(col),
                100);
        }
    }
}
