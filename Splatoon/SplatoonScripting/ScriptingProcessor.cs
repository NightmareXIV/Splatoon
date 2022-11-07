using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splatoon.SplatoonScripting
{
    internal static class ScriptingProcessor
    {
        internal static List<SplatoonScript> Scripts = new();

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

        internal static void TerritoryChanged(uint to)
        {
            for (var i = 0; i < Scripts.Count; i++)
            {
                var s = Scripts[i];
                if(s.ValidTerritories.Contains(to) && !s.IsEnabled)
                {
                    s.Enable();
                }
                else if (!s.ValidTerritories.Contains(to) && s.IsEnabled)
                {
                    s.Disable();
                }
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
