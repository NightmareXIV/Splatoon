using Dalamud.Game;
using Dalamud.Game.Command;
using Dalamud.Interface.Colors;
using ECommons.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Splatoon
{
    internal class Loader
    {
        const string url = "";
        Splatoon p;
        DalamudPluginInterface pi;
        CommandManager cmd;
        Framework f;
        HttpClient client;
        volatile Verdict verdict = Verdict.Unknown;
        volatile Version maxVersion = new("0.0.0.0");
        Version splatoonVersion;

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
            splatoonVersion = p.GetType().Assembly.GetName().Version;
            if (DalamudReflector.TryGetDalamudStartInfo(out var startInfo, pi))
            {
                var gVersion = startInfo.GameVersion.ToString();
                PluginLog.Information($"Game version: {gVersion}, Splatoon version: {splatoonVersion}");
                new Thread(() =>
                {
                    //Thread.Sleep(5000);
                    try
                    {
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
                        PluginLog.Information("Splatoon loader thread started");
                        if (p.Disposed)
                        {
                            PluginLog.Fatal("Splatoon has been disposed, loading is impossible");
                            return;
                        }
                        if (verdict == Verdict.Confirmed)
                        {
                            PluginLog.Information("Splatoon loading allowed, continuing");
                            f.Update += Load;
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

        void Draw()
        {
            if(ImGui.Begin("Splatoon - Can not confirm compatibility with current game version"))
            {
                if(verdict == Verdict.Error)
                {
                    ImGuiEx.TextWrapped(ImGuiColors.DalamudRed, "Splatoon could not connect to the update servers to verify if it could be running on current game version.");
                }
                else
                {
                    ImGuiEx.TextWrapped(ImGuiColors.DalamudOrange, "There is no information about compatibility of current version of Splatoon with current version of the game.");
                }
                if(maxVersion > splatoonVersion)
                {
                    ImGuiEx.TextWrapped(ImGuiColors.DalamudViolet, "An update for Splatoon is available. Please open plugin installer and update Splatoon plugin.");
                    if(ImGui.Button("Open plugin installer"))
                    {
                        cmd.ProcessCommand("/xlplugins");
                    }
                }
                ImGuiEx.TextWrapped("You may try to load plugin and continue using it. On a smaller patches it will usually work, but it may crash your game in which case please wait for an update.");
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
                }
            }
            ImGui.End();
        }

        enum Verdict
        {
            Unknown, Error, Outdated, Confirmed
        }
    }
}
