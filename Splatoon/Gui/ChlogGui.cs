using Dalamud.Interface.Colors;
using System.Diagnostics;

namespace Splatoon.Gui;

class ChlogGui
{
    public const int ChlogVersion = 61;
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
@"Attention!
Please make sure that you have Dynamic resolution option - disabled.
It's not compatible with Splatoon.");
        if(ThreadLoadImageHandler.TryGetTextureWrap("https://user-images.githubusercontent.com/5073202/190628533-39274d6f-8b1b-4b22-8c5a-3bae84fb9b3e.png", out var t))
        {
            ImGui.Image(t.ImGuiHandle, new Vector2(t.Width, t.Height) / 2);
        }
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
