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
    class Splatoon : IDalamudPlugin
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
            if (_pi.ClientState == null) return;
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
