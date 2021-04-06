using Dalamud.Game.ClientState.Actors.Types.NonPlayer;
using Dalamud.Plugin;
using ImGuiNET;
using System;
using Num = System.Numerics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lumina.Excel.GeneratedSheets;
using Dalamud.Game.Command;
using Dalamud.Game.Internal;

namespace Splatoon
{
    unsafe class Splatoon : IDalamudPlugin
    {
        public string Name => "Splatoon";
        internal DalamudPluginInterface _pi;
        internal Gui DrawingGui;
        internal CGui ConfigGui;
        internal DGui DebugGui;
        internal Configuration Config;
        internal Dictionary<ushort, TerritoryType> Zones;
        internal string[] LogStorage = new string[100];
        internal long CombatStarted = 0;
        internal HashSet<DisplayObject> displayObjects = new HashSet<DisplayObject>();

        public void Dispose()
        {
            Config.Save();
            DrawingGui.Dispose();
            ConfigGui.Dispose();
            DebugGui.Dispose();
            _pi.Framework.OnUpdateEvent -= HandleUpdate;
            _pi.CommandManager.RemoveHandler("/splatoon");
            _pi.Dispose();
        }

        public void Initialize(DalamudPluginInterface pluginInterface)
        {
            _pi = pluginInterface;
            Zones = _pi.Data.GetExcelSheet<TerritoryType>().ToDictionary(row => (ushort)row.RowId, row => row);
            _pi.UiBuilder.OnOpenConfigUi += delegate
            {
                ConfigGui.Open = true;
            };
            _pi.CommandManager.AddHandler("/splatoon", new CommandInfo(delegate(string command, string arguments)
            {
                if(arguments == "")
                {
                    ConfigGui.Open = true;
                }
                else if(arguments == "d")
                {
                    DebugGui.Open = true;
                }
                else if (arguments.StartsWith("enable "))
                {
                    try
                    {
                        var name = arguments.Substring(arguments.IndexOf("enable ") + 7);
                        Config.Layouts[name].Enabled = true;
                    }
                    catch (Exception e)
                    {
                        Log(e.Message);
                    }
                }
                else if (arguments.StartsWith("disable "))
                {
                    try
                    {
                        var name = arguments.Substring(arguments.IndexOf("disable ") + 8);
                        Config.Layouts[name].Enabled = false;
                    }
                    catch (Exception e)
                    {
                        Log(e.Message);
                    }
                }
            }) { });
            Config = pluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
            Config.Initialize(this);
            _pi.Framework.OnUpdateEvent += HandleUpdate;
            DrawingGui = new Gui(this);
            ConfigGui = new CGui(this);
            DebugGui = new DGui(this);
        }

        public void HandleUpdate(Framework framework)
        {
            displayObjects.Clear();
            if (_pi.ClientState == null || _pi.ClientState.LocalPlayer == null) return;

            if (_pi.ClientState.LocalPlayer.Address == IntPtr.Zero) 
            {
                Log("Pointer to LocalPlayer.Address is zero");
                return;
            }

            if (_pi.ClientState.Condition[Dalamud.Game.ClientState.ConditionFlag.InCombat])
            {
                if (CombatStarted == 0)
                {
                    CombatStarted = DateTimeOffset.Now.ToUnixTimeSeconds();
                }
            }
            else
            {
                if (CombatStarted != 0)
                {
                    CombatStarted = 0;
                }
            }

            foreach (var i in Config.Layouts.Values)
            {
                if (Config.verboselog) Log("d:3 " + i);
                if (!i.Enabled) continue;
                if (i.ZoneLock != 0 && i.ZoneLock != _pi.ClientState.TerritoryType) continue;
                if ((i.DCond == 1 || i.DCond == 3) && !_pi.ClientState.Condition[Dalamud.Game.ClientState.ConditionFlag.InCombat]) continue;
                if ((i.DCond == 2 || i.DCond == 3) && !_pi.ClientState.Condition[Dalamud.Game.ClientState.ConditionFlag.BoundByDuty]) continue;
                if (i.DCond == 4 && !(_pi.ClientState.Condition[Dalamud.Game.ClientState.ConditionFlag.InCombat]
                    || _pi.ClientState.Condition[Dalamud.Game.ClientState.ConditionFlag.BoundByDuty])) continue;
                if (i.Visibility == 1)
                {
                    if (!_pi.ClientState.Condition[Dalamud.Game.ClientState.ConditionFlag.InCombat]) continue;
                    var tic = DateTimeOffset.Now.ToUnixTimeSeconds() - CombatStarted;
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
                            x = _pi.ClientState.LocalPlayer.Position.X;
                            y = _pi.ClientState.LocalPlayer.Position.Y;
                            z = _pi.ClientState.LocalPlayer.Position.Z;
                        }
                        else if (e.refActorType == 2 && _pi.ClientState.Targets.CurrentTarget != null
                            && _pi.ClientState.Targets.CurrentTarget is BattleNpc
                            && _pi.ClientState.Targets.CurrentTarget.Address != IntPtr.Zero)
                        {
                            draw = true;
                            x = _pi.ClientState.Targets.CurrentTarget.Position.X;
                            y = _pi.ClientState.Targets.CurrentTarget.Position.Y;
                            z = _pi.ClientState.Targets.CurrentTarget.Position.Z;
                            if (e.includeHitbox) radius += *(float*)(_pi.ClientState.Targets.CurrentTarget.Address + 0xC0);
                        }
                        else if (e.refActorType == 0 && e.refActorName.Length > 0)
                        {
                            foreach (var a in _pi.ClientState.Actors)
                            {
                                if (a.Name.ToLower().Contains(e.refActorName.ToLower())
                                     && a.Address != IntPtr.Zero)
                                {
                                    draw = true;
                                    x = a.Position.X;
                                    y = a.Position.Y;
                                    z = a.Position.Z;
                                    if (e.includeHitbox) radius += *(float*)(a.Address + 0xC0);
                                    break;
                                }
                            }
                        }
                        if (e.includeOwnHitbox)
                        {
                            radius += *(float*)(_pi.ClientState.LocalPlayer.Address + 0xC0);
                        }
                    }
                    if (!draw) continue;
                    if (e.thicc > 0)
                    {
                        if (radius > 0)
                        {
                            displayObjects.Add(new DisplayObjectCircle(x + e.offX, y + e.offY, z + e.offZ, radius, e.thicc, e.color));
                        }
                        else
                        {
                            displayObjects.Add(new DisplayObjectDot(x + e.offX, y + e.offY, z + e.offZ, e.thicc, e.color));
                        }
                    }
                    if (e.overlayText.Length > 0)
                    {
                        displayObjects.Add(new DisplayObjectText(x + e.offX, y + e.offY, z + e.offZ + e.overlayVOffset, e.overlayText, e.overlayBGColor, e.overlayTextColor));
                    }
                }
            }

        }

        public void Log(string s, bool tochat = false)
        {
            if (tochat)
            {
                _pi.Framework.Gui.Chat.Print("[Splatoon]" + s);
            }
            if (Config.dumplog)
            {
                try { PluginLog.Log(s); } catch (Exception) { }
            }
            var line = DateTimeOffset.Now.ToString() + ": " + s;
            for (var i = 0; i < LogStorage.Length; i++)
            {
                if (LogStorage[i] == null)
                {
                    LogStorage[i] = line;
                    return;
                }
            }
            for (var i = 1; i < LogStorage.Length; i++)
            {
                LogStorage[i - 1] = LogStorage[i];
            }
            LogStorage[LogStorage.Length - 1] = line;
        }


        public void HandleChat()
        {

        }
    }
}
