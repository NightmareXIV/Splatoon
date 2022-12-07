using ECommons.LanguageHelpers;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Splatoon.SplatoonScripting;

internal static class ScriptingProcessor
{
    internal static List<SplatoonScript> Scripts = new();
    internal static ConcurrentQueue<(string code, string path)> LoadScriptQueue = new();
    internal static volatile bool ThreadIsRunning = false;
    internal readonly static string[] TrustedURLs = new string[]
    {
        "https://github.com/NightmareXIV/",
        "https://www.github.com/NightmareXIV/",
        "https://raw.githubusercontent.com/NightmareXIV/"
    };

    internal static bool IsUrlTrusted(string url)
    {
        return url.StartsWithAny(ScriptingProcessor.TrustedURLs, StringComparison.OrdinalIgnoreCase);
    }

    internal static void DownloadScript(string url)
    {
        Task.Run(delegate
        {
            try
            {
                var result = P.HttpClient.GetStringAsync(url).Result;
                ScriptingProcessor.CompileAndLoad(result, null);
            }
            catch (Exception e)
            {
                e.Log();
            }
        });

        Notify.Info("Downloading script from trusted URL...".Loc());
    }

    internal static void ReloadAll()
    {
        Scripts.ForEach(x => x.Disable());
        Scripts.Clear();
        var dir = Path.Combine(Svc.PluginInterface.GetPluginConfigDirectory(), "Scripts");
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
        foreach(var f in Directory.GetFiles(dir, "*.cs", SearchOption.AllDirectories))
        {
            CompileAndLoad(File.ReadAllText(f, Encoding.UTF8), f);
        }
    }

    internal static void CompileAndLoad(string sourceCode, string fpath)
    {
        PluginLog.Information($"Requested script loading");
        LoadScriptQueue.Enqueue((sourceCode, fpath));
        if (!ThreadIsRunning)
        {
            ThreadIsRunning = true;
            PluginLog.Information($"Beginning new thread");
            new Thread(() =>
            {
                PluginLog.Information($"Compiler thread started");
                int idleCount = 0;
                var dir = Path.Combine(Svc.PluginInterface.GetPluginConfigDirectory(), "ScriptCache");
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                while (idleCount < 10)
                {
                    if (LoadScriptQueue.TryDequeue(out var result))
                    {
                        try
                        {
                            var md5 = MD5.HashData(Encoding.UTF8.GetBytes(result.code)).Select(x=>$"{x:X2}").Join("");
                            var cacheFile = Path.Combine(dir, $"{md5}-{P.loader.splatoonVersion}.bin");
                            PluginLog.Information($"Cache path: {cacheFile}");
                            byte[] code = null;
                            if (File.Exists(cacheFile))
                            {
                                PluginLog.Information($"Loading from cache...");
                                code = File.ReadAllBytes(cacheFile);
                            }
                            else
                            {                                    PluginLog.Information($"Compiling...");
                                code = Compiler.Compile(result.code, result.path == null?"":Path.GetFileNameWithoutExtension(result.path));
                                if (code != null)
                                {
                                    File.WriteAllBytes(cacheFile, code);
                                    PluginLog.Information($"Compiled and saved");
                                }
                            }
                            if (code != null)
                            {
                                new TickScheduler(delegate
                                {
                                    if (P != null && !P.Disposed)
                                    {
                                        var assembly = Compiler.Load(code);
                                        foreach (var t in assembly.GetTypes())
                                        {
                                            if (t.BaseType?.FullName == "Splatoon.SplatoonScripting.SplatoonScript")
                                            {
                                                var instance = (SplatoonScript)assembly.CreateInstance(t.FullName);
                                                instance.InternalData = new(result.path, instance);
                                                bool rewrite = false;
                                                if (Scripts.TryGetFirst(z => z.InternalData.FullName == instance.InternalData.FullName, out var loadedScript))
                                                {
                                                    DuoLog.Information($"Script {instance.InternalData.FullName} already loaded, replacing.");
                                                    result.path = loadedScript.InternalData.Path;
                                                    loadedScript.Disable();
                                                    ScriptingProcessor.Scripts.RemoveAll(x => ReferenceEquals(loadedScript, x));
                                                    rewrite = true;
                                                }
                                                Scripts.Add(instance);
                                                if (result.path == null)
                                                {
                                                    var dir = Path.Combine(Svc.PluginInterface.GetPluginConfigDirectory(), "Scripts", instance.InternalData.Namespace);
                                                    if (!Directory.Exists(dir))
                                                    {
                                                        Directory.CreateDirectory(dir);
                                                    }
                                                    var newPath = Path.Combine(dir, $"{instance.InternalData.Name}.cs");
                                                    instance.InternalData.Path = newPath;
                                                    File.WriteAllText(newPath, result.code, Encoding.UTF8);
                                                    DuoLog.Information($"Script installed to {newPath}");
                                                }
                                                else if (rewrite)
                                                {
                                                    //DeleteFileToRecycleBin(result.path);
                                                    File.WriteAllText(result.path, result.code, Encoding.UTF8);
                                                    instance.InternalData.Path = result.path;
                                                    DuoLog.Information($"Script overwritten at {instance.InternalData.Path}");
                                                }
                                                instance.OnSetup();
                                                PluginLog.Information($"Load success");
                                                instance.UpdateState();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        PluginLog.Fatal("Plugin was disposed during script loading");
                                    }
                                });
                            }
                            else
                            {
                                PluginLog.Error("Loading process ended with error");
                            }
                        }
                        catch(Exception e)
                        {
                            e.Log();
                        }
                        idleCount = 0;
                    }
                    else
                    {
                        //PluginLog.Verbose($"Script loading thread is idling, count {idleCount}");
                        idleCount++;
                        Thread.Sleep(250);
                    }
                }
                ThreadIsRunning = false;
                PluginLog.Information($"Exited from compiler thread");
            }).Start();
        }
    }

    internal static void OnUpdate()
    {
        for(var i = 0;i < Scripts.Count; i++)
        {
            if (Scripts[i].IsEnabled)
            {
                try
                {
                    Scripts[i].OnUpdate();
                }
                catch (Exception e) { e.Log(); }
            }
        }
    }

    internal static void OnCombatStart()
    {
        for (var i = 0; i < Scripts.Count; i++)
        {
            if (Scripts[i].IsEnabled)
            {
                try
                {
                    Scripts[i].OnCombatStart();
                }
                catch (Exception e) { e.Log(); }
            }
        }
    }

    internal static void OnCombatEnd()
    {
        for (var i = 0; i < Scripts.Count; i++)
        {
            if (Scripts[i].IsEnabled)
            {
                try
                {
                    Scripts[i].OnCombatEnd();
                }
                catch (Exception e) { e.Log(); }
            }
        }
    }

    internal static void OnMapEffect(uint Position, ushort Param1, ushort Param2)
    {
        for (var i = 0; i < Scripts.Count; i++)
        {
            if (Scripts[i].IsEnabled)
            {
                try
                {
                    Scripts[i].OnMapEffect(Position, Param1, Param2);
                }
                catch (Exception e) { e.Log(); }
            }
        }
    }

    internal static void OnObjectEffect(uint Target, ushort Param1, ushort Param2)
    {
        for (var i = 0; i < Scripts.Count; i++)
        {
            if (Scripts[i].IsEnabled)
            {
                try
                {
                    Scripts[i].OnObjectEffect(Target, Param1, Param2);
                }
                catch (Exception e) { e.Log(); }
            }
        }
    }

    internal static void OnMessage(string Message)
    {
        for (var i = 0; i < Scripts.Count; i++)
        {
            if (Scripts[i].IsEnabled)
            {
                try
                {
                    Scripts[i].OnMessage(Message);
                }
                catch (Exception e) { e.Log(); }
            }
        }
    }

    internal static void OnVFXSpawn(uint target, string vfxPath)
    {
        for (var i = 0; i < Scripts.Count; i++)
        {
            if (Scripts[i].IsEnabled)
            {
                try
                {
                    Scripts[i].OnVFXSpawn(target, vfxPath);
                }
                catch (Exception e) { e.Log(); }
            }
        }
    }

    internal static void OnTetherCreate(uint source, uint target, byte data2, byte data3, byte data5)
    {
        for (var i = 0; i < Scripts.Count; i++)
        {
            if (Scripts[i].IsEnabled)
            {
                try
                {
                    Scripts[i].OnTetherCreate(source, target, data2, data3, data5);
                }
                catch (Exception e) { e.Log(); }
            }
        }
    }

    internal static void OnTetherRemoval(uint source, byte data2, byte data3, byte data5)
    {
        for (var i = 0; i < Scripts.Count; i++)
        {
            if (Scripts[i].IsEnabled)
            {
                try
                {
                    Scripts[i].OnTetherRemoval(source, data2, data3, data5);
                }
                catch (Exception e) { e.Log(); }
            }
        }
    }

    internal static void OnPhaseChange(int phase)
    {
        for (var i = 0; i < Scripts.Count; i++)
        {
            if (Scripts[i].IsEnabled)
            {
                try
                {
                    Scripts[i].OnPhaseChange(phase);
                }
                catch (Exception e) { e.Log(); }
            }
        }
    }

    internal static void TerritoryChanged()
    {
        for (var i = 0; i < Scripts.Count; i++)
        {
            var s = Scripts[i];
            UpdateState(s);
        }
    }

    internal static void UpdateState(this SplatoonScript s)
    {
        var territoryIsValid = Svc.ClientState.IsLoggedIn && (s.ValidTerritories.Count == 0 || s.ValidTerritories.Contains(Svc.ClientState.TerritoryType));
        if (territoryIsValid && !P.Config.DisabledScripts.Contains(s.InternalData.FullName))
        {
            if (!s.IsEnabled)
            {
                s.Enable();
            }
        }
        else if (s.IsEnabled)
        {
            s.Disable();
        }
    }

    internal static void Dispose()
    {
        for (var i = 0; i < Scripts.Count; i++)
        {
            Scripts[i].Disable();
        }
        Scripts.Clear();
    }
}
