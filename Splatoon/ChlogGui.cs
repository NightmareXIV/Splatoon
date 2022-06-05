namespace Splatoon;

class ChlogGui
{
    public const int ChlogVersion = 48;
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
@"- New element: cone relative to object position
   Limitations:
   - Always has to be filled
   - Fake filling with multiple lines because there is currently no known way to properly cull cone object
   - May be performance heavy in certain situations
- Experimental feature: Face me
   Makes cone or line face your position
- Version checker
   Splatoon will be disallowed to load on new game patches.
   You will be given a choice to either wait until confirmation or update,
   or ignore the warning and load the plugin immediately.");
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
