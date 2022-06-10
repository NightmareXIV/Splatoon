namespace Splatoon;

class ChlogGui
{
    public const int ChlogVersion = 49;
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
        ImGui.TextUnformatted(
@"- Added possibility to require a character to have or to not have specific status(es)
- Added possibility to require self or target to be casting or have status");
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
        this.Dispose();
    }
}
