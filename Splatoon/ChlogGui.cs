using Dalamud.Interface.Colors;

namespace Splatoon;

class ChlogGui
{
    public const int ChlogVersion = 56;
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
        ImGuiEx.Text(ImGuiColors.DalamudRed, "This is important update. ");
        ImGuiEx.Text(
@"You are using preview Splatoon version. Thank you for considering testing it before release.
Your configuration has been converted into new format and is no longer compatible with old version.
If you want to return to previous version, you will need to unload splatoon 2.x, move
%appdata%\XIVLauncher\pluginConfigs\Splatoon\configV1.json
file into 
%appdata%\XIVLauncher\pluginConfigs
and rename it into Splatoon.json.
Then you may load 1.x version and continue using it.
Please report bugs and suggestions in discord.");
        ImGui.Checkbox("I have read and understood the update.", ref understood);
        if (understood)
        {
            ImGui.SameLine();
            if (ImGui.Button("Close this window"))
            {
                open = false;
            }

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
