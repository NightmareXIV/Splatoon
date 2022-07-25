using Dalamud.Interface.Colors;

namespace Splatoon;

class ChlogGui
{
    public const int ChlogVersion = 55;
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
        ImGuiEx.Text(ImGuiColors.DalamudRed, "This is important update. Read at least until separator.");
        ImGuiEx.Text(
@"- Reworked rectangle fill:
   - Instead of solid color filling, rectangles are now filled with crossing lines by default.
      It fully resolves clipping issue at the cost of some performance (and slightly worse look), but it should not be very noticeable.
      You may disable this behavior, if you wish, although it is not recommended.
      You may configure fill options in Splatoon's general settings, although it is configured optimally by default.
      Each individual element has fill control as well. 
");
        ImGuiEx.Text(ImGuiColors.DalamudOrange, "     Please consider going into The Bowl of Embers (hard) and seeing how new filling method works for you before going into any raid.");
        ImGui.SetCursorPosX(50f);
        if(ImGui.Button("Import test layout for Ifrit hard"))
        {
            //P.ConfigGui.ImportFromText("Ifrit Test~{\"ZoneLockH\":[292],\"Elements\":{\"Ifrit\":{\"type\":3,\"refY\":50.0,\"radius\":1.0,\"color\":1509949695,\"refActorNPCNameID\":1185,\"refActorComparisonType\":6,\"includeHitbox\":true,\"includeRotation\":true,\"onlyVisible\":true}}}");
        }
        ImGuiEx.Text(ImGuiColors.DalamudOrange, "     You should see rectangle in front of ifrit. Damage him until he jumps into the air, then\n" +
            "     wait for a while until 3 copies of him charge from the edge of the arena.\n" +
            "     See how it looks and adjust settings if necessary.");
        ImGui.Separator();
        ImGuiEx.Text(@"- Added possibility to create ""donuts"" with similar filling to rectangles.
- Added possibility to target character with certain Battle NPC Name ID. This allows name selectors to become international.
   - If unambiguously possible, there will be button in element with which you can convert text into Name ID.
   - It is recommended to use NPC ID, Name NPC ID or Model ID to target certain NPC rather than name. This way elements become international.
- Added possibility to target specified object with standard in-game placeholders like <1>, <2>, <tt>, <mo> etc.
   - In addition, custom placeholders were added: <t1> - <t9>, <h1> - <h9>, <d1> - <d9> allowing to select players in your party by class.
- Object logger: added viewer mode which displays only objects that are currently present.
- Added an option to switch between decimal and hexadecimal numbers in the plugin interface. Select one that is to your liking.

As always, please report bugs and problems you find to my Github or Discord. And if you have any presets to share - they are always appreciated as well.");
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
