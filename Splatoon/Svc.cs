using Dalamud.Data;
using Dalamud.Game;
using Dalamud.Game.ClientState;
using Dalamud.Game.ClientState.Buddy;
using Dalamud.Game.ClientState.Conditions;
using Dalamud.Game.ClientState.Fates;
using Dalamud.Game.ClientState.JobGauge;
using Dalamud.Game.ClientState.Keys;
using Dalamud.Game.ClientState.Objects;
using Dalamud.Game.ClientState.Party;
using Dalamud.Game.Command;
using Dalamud.Game.Gui;
using Dalamud.Game.Gui.FlyText;
using Dalamud.Game.Gui.PartyFinder;
using Dalamud.Game.Gui.Toast;
using Dalamud.Game.Libc;
using Dalamud.Game.Network;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.IoC;

namespace Splatoon;
class Svc
{
    [PluginService] static internal DalamudPluginInterface PluginInterface { get; private set; }
    [PluginService] static internal BuddyList Buddies { get; private set; }
    [PluginService] static internal ChatGui Chat { get; private set; }
    [PluginService] static internal ChatHandlers ChatHandlers { get; private set; }
    [PluginService] static internal ClientState ClientState { get; private set; }
    [PluginService] static internal CommandManager Commands { get; private set; }
    [PluginService] static internal Condition Condition { get; private set; }
    [PluginService] static internal DataManager Data { get; private set; }
    [PluginService] static internal FateTable Fates { get; private set; }
    [PluginService] static internal FlyTextGui FlyText { get; private set; }
    [PluginService] static internal Framework Framework { get; private set; }
    [PluginService] static internal GameGui GameGui { get; private set; }
    [PluginService] static internal GameNetwork GameNetwork { get; private set; }
    [PluginService] static internal JobGauges Gauges { get; private set; }
    [PluginService] static internal KeyState KeyState { get; private set; }
    [PluginService] static internal LibcFunction LibcFunction { get; private set; }
    [PluginService] static internal ObjectTable Objects { get; private set; }
    [PluginService] static internal PartyFinderGui PfGui { get; private set; }
    [PluginService] static internal PartyList Party { get; private set; }
#pragma warning disable CS0618 // Type or member is obsolete
    [PluginService] static internal SeStringManager SeStringManager { get; private set; }
#pragma warning restore CS0618 // Type or member is obsolete
    [PluginService] static internal SigScanner SigScanner { get; private set; }
    [PluginService] static internal TargetManager Targets { get; private set; }
    [PluginService] static internal ToastGui Toasts { get; private set; }
}
