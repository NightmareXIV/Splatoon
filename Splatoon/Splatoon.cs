using Dalamud.Game.ClientState.Actors.Types.NonPlayer;
using Dalamud.Plugin;
using ImGuiNET;
using System;
using Num = System.Numerics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dalamud.Game.Command;
using Dalamud.Game.Internal;
using Dalamud.Game.ClientState.Actors.Types;
using System.Runtime.ExceptionServices;
using Lumina.Excel.GeneratedSheets;

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
        internal IntPtr CameraAddress;
        internal double CamAngleX;
        internal Dictionary<int, string> Jobs = new Dictionary<int, string>();
        internal HashSet<(float x, float y, float z, float r)> draw = new HashSet<(float x, float y, float z, float r)>();
        internal bool AccessViolation = false;
        internal double CamAngleY;

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
            Jobs = _pi.Data.GetExcelSheet<ClassJob>().ToDictionary(row => (int)row.RowId, row => row.Name.ToString());
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
            CameraAddress = *(IntPtr*)_pi.TargetModuleScanner.GetStaticAddressFromSig("48 8D 35 ?? ?? ?? ?? 48 8B 34 C6 F3");
        }

        [HandleProcessCorruptedStateExceptions]
        public void HandleUpdate(Framework framework)
        {
            try
            {
                if (AccessViolation)
                {
                    AccessViolation = false;
                    var a = *(float*)new IntPtr(0x12345678);
                }
                displayObjects.Clear();
                if (_pi.ClientState == null || _pi.ClientState.LocalPlayer == null) return;
                var pl = _pi.ClientState.LocalPlayer;
                if (_pi.ClientState.LocalPlayer.Address == IntPtr.Zero)
                {
                    Log("Pointer to LocalPlayer.Address is zero");
                    return;
                }

                CamAngleX = *(float*)(CameraAddress + 0x130) + Math.PI;
                if (CamAngleX > Math.PI) CamAngleX -= 2 * Math.PI;
                CamAngleY = *(float*)(CameraAddress + 0x134);

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

                if (CamAngleY > Config.maxcamY) return;

                foreach (var i in Config.Layouts.Values)
                {
                    if (!i.Enabled) continue;
                    if (i.ZoneLockH.Count > 0 && !i.ZoneLockH.Contains(_pi.ClientState.TerritoryType)) continue;
                    if (i.JobLock != 0 && !Bitmask.IsBitSet(i.JobLock, (int)_pi.ClientState.LocalPlayer.ClassJob.Id)) continue;
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
                        draw.Clear();
                        float radius = e.radius;
                        if (e.type == 0)
                        {
                            draw.Add((e.refX, e.refY, e.refZ, radius));
                        }
                        else if (e.type == 1)
                        {
                            if (e.includeOwnHitbox) radius += *(float*)(_pi.ClientState.LocalPlayer.Address + 0xC0);
                            if (e.refActorType == 1)
                            {
                                draw.Add((_pi.ClientState.LocalPlayer.Position.X, _pi.ClientState.LocalPlayer.Position.Y,
                                    _pi.ClientState.LocalPlayer.Position.Z, radius));
                            }
                            else if (e.refActorType == 2 && _pi.ClientState.Targets.CurrentTarget != null
                                && _pi.ClientState.Targets.CurrentTarget is BattleNpc
                                && _pi.ClientState.Targets.CurrentTarget.Address != IntPtr.Zero)
                            {
                                if (e.includeHitbox) radius += *(float*)(_pi.ClientState.Targets.CurrentTarget.Address + 0xC0);
                                draw.Add((_pi.ClientState.Targets.CurrentTarget.Position.X, _pi.ClientState.Targets.CurrentTarget.Position.Y,
                                    _pi.ClientState.Targets.CurrentTarget.Position.Z, radius));
                            }
                            else if (e.refActorType == 0 && e.refActorName.Length > 0)
                            {
                                foreach (var a in _pi.ClientState.Actors)
                                {
                                    if (a.Name.ToLower().Contains(e.refActorName.ToLower())
                                         && a.Address != IntPtr.Zero)
                                    {
                                        var aradius = radius;
                                        if (e.includeHitbox) aradius += *(float*)(a.Address + 0xC0);
                                        draw.Add((a.Position.X, a.Position.Y, a.Position.Z, aradius));
                                    }
                                }
                            }

                        }
                        else if (e.type == 2)
                        {
                            if (ShouldDraw(e.offX, _pi.ClientState.LocalPlayer.Position.X, e.offY, _pi.ClientState.LocalPlayer.Position.Y)
                                || ShouldDraw(e.refX, _pi.ClientState.LocalPlayer.Position.X, e.refY, _pi.ClientState.LocalPlayer.Position.Y))
                            displayObjects.Add(new DisplayObjectLine(e.refX, e.refY, e.refZ, e.offX, e.offY, e.offZ, e.thicc, e.color));
                        }
                        if (draw.Count == 0) continue;
                        foreach (var (x, y, z, r) in draw)
                        {
                            if (!ShouldDraw(x + e.offX, _pi.ClientState.LocalPlayer.Position.X, y + e.offY, _pi.ClientState.LocalPlayer.Position.Y)) continue;
                            if (e.thicc > 0)
                            {
                                if (r > 0)
                                {
                                    displayObjects.Add(new DisplayObjectCircle(x + e.offX, y + e.offY, z + e.offZ, r, e.thicc, e.color));
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
            }
            catch(Exception e)
            {
                Log("Caught exception: "+e.Message);
                Log(e.StackTrace);
            }
        }

        public bool ShouldDraw(float x1, float x2, float y1, float y2)
        {
            return ((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2)) < Config.maxdistance * Config.maxdistance;
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
