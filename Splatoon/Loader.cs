using Dalamud.Game;
using Dalamud.Game.Command;
using Dalamud.Interface.Colors;
using ECommons.Reflection;
using System.Net.Http;
using System.Threading;

namespace Splatoon
{
    internal class Loader
    {
        const string url = "";
        Splatoon p;
        internal DalamudPluginInterface pi;
        internal CommandManager cmd;
        Framework f;
        HttpClient client;
        internal volatile Verdict verdict = Verdict.Unknown;
        internal volatile Version maxVersion = new("0.0.0.0");
        internal string gVersion;
        internal Version splatoonVersion;
        string file;

        internal Loader(Splatoon p, DalamudPluginInterface pi, Framework f, CommandManager cmd)
        {
            PluginLog.Information("Splatoon loader started");
            client = new()
            {
                Timeout = TimeSpan.FromSeconds(10)
            };
            this.p = p;
            this.pi = pi;
            this.f = f;
            this.cmd = cmd;
            cmd.AddHandler("/loadsplatoon", new(delegate { Load(f); }) { HelpMessage = "Manually load Splatoon"});
            splatoonVersion = p.GetType().Assembly.GetName().Version;
            file = Path.Combine(pi.GetPluginConfigDirectory(), "safeVersion.nfo");
            if (DalamudReflector.TryGetDalamudStartInfo(out var startInfo, pi))
            {
                gVersion = startInfo.GameVersion.ToString();
                PluginLog.Information($"Game version: {gVersion}, Splatoon version: {splatoonVersion}");
                new Thread(() =>
                {
                    PluginLog.Information("Splatoon loader thread started");
                    //Thread.Sleep(5000);
                    try
                    {
                        if (File.Exists(file))
                        {
                            if(File.ReadAllText(file) == gVersion)
                            {
                                PluginLog.Information("Loading is allowed via file, skipping checking GitHub...");
                                f.Update += Load;
                                return;
                            }
                        }
                        var res = client.GetAsync("https://raw.githubusercontent.com/Eternita-S/Splatoon/master/versions.txt").Result;
                        res.EnsureSuccessStatusCode();
                        foreach (var x in res.Content.ReadAsStringAsync().Result.Split("\n"))
                        {
                            PluginLog.Debug(x);
                            var s = x.Split(":");
                            if (s.Length != 2) continue;
                            var ver = Version.Parse(s[1]);
                            if (ver > maxVersion) maxVersion = ver;
                            if (s[0] == gVersion && splatoonVersion >= ver)
                            {
                                verdict = Verdict.Confirmed;
                                break;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        PluginLog.Error($"{e.Message}\n{e.StackTrace}");
                        verdict = Verdict.Error;
                    }
                    Safe(delegate
                    {
                        if (p.Disposed)
                        {
                            PluginLog.Fatal("Splatoon has been disposed, loading is impossible");
                            return;
                        }
                        if (verdict == Verdict.Confirmed)
                        {
                            PluginLog.Information("Splatoon loading allowed, continuing");
                            f.Update += Load;
                            File.WriteAllText(file, gVersion);
                        }
                        else
                        {
                            PluginLog.Warning("Splatoon loading disallowed. Displaying confirmation window.");
                            pi.UiBuilder.Draw += Draw;
                        }
                    });
                }).Start();
            }
            else
            {
                PluginLog.Error("Could not get Dalamud start info");
            }
        }

        void Load(Framework fr)
        {
            PluginLog.Information("Splatoon begins loading process");
            fr.Update -= Load;
            p.Load(pi);
            PluginLog.Information("Splatoon has been loaded");
        }

        Vector2 size = Vector2.Zero;
        internal void Draw()
        {
            ImGuiHelpers.ForceNextWindowMainViewport();
            ImGuiHelpers.SetNextWindowPosRelativeMainViewport(ImGuiHelpers.MainViewport.Size / 2 - size / 2);
            if(ImGui.Begin("Splatoon - Can not confirm compatibility with current game version", ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoCollapse))
            {
                if(verdict == Verdict.Error)
                {
                    ImGuiEx.Text(ImGuiColors.DalamudRed, "Splatoon could not connect to the GitHub to verify\n" +
                        "if it could be running on current game version.");
                    ImGuiEx.Text("If the game has just updated, please test if the plugin works fine and if it does,\n" +
                        "press \"Load Splatoon and never display this window until next game update\" " +
                        "\nbutton in this window next time you start the game.");
                }
                else
                {
                    ImGuiEx.Text(ImGuiColors.DalamudOrange, "There is no information about compatibility of current version of\n" +
                        "Splatoon with current version of the game.");
                    ImGuiEx.Text("You may try to load plugin and continue using it. \n" +
                    "On a smaller patches it will usually work, but it may crash your game\n" +
                    "in which case please wait for an update.");
                }
                if(maxVersion > splatoonVersion)
                {
                    ImGuiEx.TextWrapped(ImGuiColors.DalamudViolet, "An update for Splatoon is available. \n" +
                        "Please open plugin installer and update Splatoon plugin.");
                    if(ImGui.Button("Open plugin installer"))
                    {
                        cmd.ProcessCommand("/xlplugins");
                    }
                }
                if(ImGui.Button("Load Splatoon anyway"))
                {
                    pi.UiBuilder.Draw -= Draw;
                    PluginLog.Warning("Received confirmation to load Splatoon with unverified game version");
                    f.Update += Load;
                }
                if (ImGui.Button("Close this window"))
                {
                    pi.UiBuilder.Draw -= Draw;
                }
                if (ImGui.Button("Load Splatoon and never display this window until next game update"))
                {
                    pi.UiBuilder.Draw -= Draw;
                    PluginLog.Warning("Received confirmation to load Splatoon with unverified game version and override game version");
                    f.Update += Load;
                    Safe(delegate
                    {
                        File.WriteAllText(file, gVersion);
                    });
                }
            }
            size = ImGui.GetWindowSize();
            ImGui.End();
        }

        internal enum Verdict
        {
            Unknown, Error, Outdated, Confirmed
        }
    }
}
