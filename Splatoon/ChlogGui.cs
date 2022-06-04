namespace Splatoon;

class ChlogGui
{
    public const int ChlogVersion = 46;
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
@"- Added new element conditions for elements relative to object position: object life and cast requirement.
   This allows you to only display elements relative to characters that are casting something
   and filter objects by amount of time they exist.
- An attempt to fix problems when camera zoom goes beyond standard values.
- Web api update: passing destroy=* will clear all existing dynamic elements.
- Tether being offset for objects with rotation accounted should be fixed.");
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
