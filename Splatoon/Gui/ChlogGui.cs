using ECommons.LanguageHelpers;

namespace Splatoon.Gui;

class ChlogGui
{
    public const int ChlogVersion = 62;
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
        ImGui.Begin("Splatoon has been updated".Loc(), ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.AlwaysAutoResize);
        ImGuiEx.Text(
@"Attention!
This update brings breaking changes to the scripting system. \nPlease check that all your scripts are installed, updated, loaded and working.");
        if (ImGui.Button("Close this window".Loc()))
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
