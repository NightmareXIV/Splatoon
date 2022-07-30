using Dalamud.Interface.Colors;
using System.Diagnostics;

namespace Splatoon;

class ChlogGui
{
    public const int ChlogVersion = 58;
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
@"Splatoon 2.0 is a big update of the plugin and includes UI rework which allows to:
- Group your layouts and elements
- Sort them
- Rename them
- Name no longer has to be unique
- Name is no longer required at all

If you are updating from version 1.x, your configuration will be converted to new format. 
Nothing should be lost, and backup of your old configuration will be made and stored at %appdata%\XIVLauncher\pluginConfigs\Splatoon\configV1.json,
but please make sure to backup your configuration file manually if you have valuable presets there.
");
        if(ImGui.Button("Open folder with my configuration file"))
        {
            Safe(delegate
            {
                Process.Start(new ProcessStartInfo()
                {
                    FileName = "explorer.exe",
                    UseShellExecute = true,
                    Arguments = $"/select,\"{Svc.PluginInterface.ConfigFile.FullName}\""
                });
            });
        }
        ImGuiEx.Text(
@"Don't forget to test that the plugin functions correctly before you go into your raid.
If you will encounter any problems, feel free to join the discord for help: ");
        if(ImGui.Button("Join discord"))
        {
            ShellStart("https://discord.gg/m8NRt4X8Gf");
        }
        ImGuiEx.Text(
@"Additionally, all the details about plugin updates are always posted there.");
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
