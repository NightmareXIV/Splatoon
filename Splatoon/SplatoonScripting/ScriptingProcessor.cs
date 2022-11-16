using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Splatoon.SplatoonScripting
{
    internal static class ScriptingProcessor
    {
        internal static List<SplatoonScript> Scripts = new();
        internal static ConcurrentQueue<string> LoadScriptQueue = new();
        internal static volatile bool ThreadIsRunning = false;

        internal static void CompileAndLoad(string sourceCode)
        {
            PluginLog.Information($"Requested script loading");
            LoadScriptQueue.Enqueue(sourceCode);
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
                                var md5 = MD5.HashData(Encoding.UTF8.GetBytes(sourceCode));
                                var cacheFile = Path.Combine(dir, $"{md5}-{P.loader.splatoonVersion}.bin");
                                PluginLog.Information($"Cache path: {cacheFile}");
                                var code = Array.Empty<byte>();
                                if (File.Exists(cacheFile))
                                {
                                    PluginLog.Information($"Loading from cache...");
                                    code = File.ReadAllBytes(cacheFile);
                                }
                                else
                                {
                                    PluginLog.Information($"Compiling...");
                                    code = Compiler.Compile(sourceCode);
                                    File.WriteAllBytes(cacheFile, code);
                                    PluginLog.Information($"Compiled and saved");
                                }
                                new TickScheduler(delegate
                                {
                                    var assembly = Compiler.Load(code);
                                    foreach (var t in assembly.GetTypes())
                                    {
                                        if (t.BaseType?.FullName == "Splatoon.SplatoonScripting.SplatoonScript")
                                        {
                                            var instance = (SplatoonScript)assembly.CreateInstance(t.FullName);
                                            Scripts.Add(instance);
                                            instance.InternalData = new("", instance);
                                            instance.OnSetup();
                                            PluginLog.Information($"Load success");
                                            instance.UpdateState();
                                        }
                                    }
                                });
                            }
                            catch(Exception e)
                            {
                                e.Log();
                            }
                            idleCount = 0;
                        }
                        else
                        {
                            PluginLog.Verbose($"Script loading thread is idling, count {idleCount}");
                            idleCount++;
                            Thread.Sleep(100);
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

        internal static void OnTetherCreate(uint source, uint target)
        {
            for (var i = 0; i < Scripts.Count; i++)
            {
                if (Scripts[i].IsEnabled)
                {
                    try
                    {
                        Scripts[i].OnTetherCreate(source, target);
                    }
                    catch (Exception e) { e.Log(); }
                }
            }
        }

        internal static void OnTetherRemoval(uint source)
        {
            for (var i = 0; i < Scripts.Count; i++)
            {
                if (Scripts[i].IsEnabled)
                {
                    try
                    {
                        Scripts[i].OnTetherRemoval(source);
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
            var to = Svc.ClientState.IsLoggedIn ? Svc.ClientState.TerritoryType : 0u;
            if ((s.ValidTerritories.Count == 0 || s.ValidTerritories.Contains(to)) && !s.IsEnabled)
            {
                s.Enable();
            }
            else if (!s.ValidTerritories.Contains(to) && s.IsEnabled)
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
}
