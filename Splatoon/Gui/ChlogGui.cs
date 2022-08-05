using Dalamud.Interface.Colors;
using System.Diagnostics;

namespace Splatoon.Gui;

class ChlogGui
{
    public const int ChlogVersion = 60;
    readonly Splatoon p;
    bool open = true;
    internal bool openLoggedOut = false;
    bool understood = false;
    public ChlogGui(Splatoon p)
    {
        this.p = p;
        Svc.PluginInterface.UiBuilder.Draw += Draw;
    }

    public void Dispose()
    {
        Svc.PluginInterface.UiBuilder.Draw -= Draw;
    }

    void Draw()
    {
        if (!open) return;
        if (!Svc.ClientState.IsLoggedIn && !openLoggedOut) return;
        ImGui.Begin("Splatoon has been updated", ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.AlwaysAutoResize);
        ImGuiEx.Text(
@"- Added possibility to limit cast and buff time to certain values.
- Casts can now support time after cast has been finished.
- Most placeholders (<1>, <t1> etc) will work in duty recorder replays.

- Experimental feature: freezing layout.
  - Once it becomes visible, it's elements will be fixed on screen for specified time, 
     and snapshots will repeat at specified time.
  - This feature is in beta test and may work unexpectedly/may change in future.
  - When used not carefully, it may become the cause of FPS drop.

- Added a detection of popular streaming software running. 
   Plugin will remind you that you shouldn't stream while running third party software.
      Reminding you that ALL third party tools are not officially allowed by FFXIV. 
       - This includes Dalamud plugins both official and 3rd party, mods, ACT, latency mitigation tools.
       - Game will not attempt to detect use of such tools but if you talk about them in game 
          or STREAM them, you may face consequences.
   Plugin will not stop functioning while you're streaming 
       and will not attempt to prevent you from streaming in any way.
   You may disable this function in settings as well.

- Implemented internal optimizations and performance improvements.");
        if (ImGui.Button("Close this window"))
        {
            open = false;
        }
        ImGui.End();
        if (!open) Close();
    }

    void Close()
    {
        p.Config.Backup(true);
        p.Config.ChlogReadVer = ChlogVersion;
        p.Config.Save();
        Dispose();
    }
}
