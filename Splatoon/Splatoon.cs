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

        public void Dispose()
        {
            Config.Save();
            DrawingGui.Dispose();
            ConfigGui.Dispose();
            DebugGui.Dispose();
            _pi.CommandManager.RemoveHandler("/splatoon");
            _pi.Dispose();
        }

        public void Initialize(DalamudPluginInterface pluginInterface)
        {
            _pi = pluginInterface;
            Zones = _pi.Data.GetExcelSheet<TerritoryType>().ToDictionary(row => (ushort)row.RowId, row => row);
            DrawingGui = new Gui(this);
            ConfigGui = new CGui(this);
            DebugGui = new DGui(this);
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
            }) { });
            Config = pluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
            Config.Initialize(this);
        }

        public void Log(string s)
        {
            _pi.Framework.Gui.Chat.Print(s);
        }
    }
}
