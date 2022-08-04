using Dalamud.Game.Network;
using Dalamud.Hooking;
using Splatoon.Structures;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Splatoon.Memory
{
    internal unsafe static class AttachedInfo
    {
        delegate IntPtr GameObject_ctor(IntPtr obj);
        static Hook<GameObject_ctor> GameObject_ctor_hook = null;
        internal static Dictionary<IntPtr, CachedCastInfo> CastInfos = new();
        static HashSet<IntPtr> Casters = new();

        internal static void Init()
        {
            GameObject_ctor_hook = Hook<GameObject_ctor>.FromAddress(Svc.SigScanner.ScanText("E8 ?? ?? ?? ?? 48 8D 05 ?? ?? ?? ?? 48 8D 35"), GameObject_ctor_detour);
            GameObject_ctor_hook.Enable();
            Svc.Framework.Update += Tick;
        }


        internal static void Dispose()
        {
            Svc.Framework.Update -= Tick;
            if (GameObject_ctor_hook != null)
            {
                GameObject_ctor_hook.Disable();
                GameObject_ctor_hook.Dispose();
                GameObject_ctor_hook = null;
            }
            CastInfos = null;
        }

        static IntPtr GameObject_ctor_detour(IntPtr ptr)
        {
            CastInfos.Remove(ptr);
            Casters.Remove(ptr);
            return GameObject_ctor_hook.Original(ptr);
        }

        static void Tick(object _)
        {
            foreach(var x in Svc.Objects)
            {
                if(x is BattleChara b)
                {
                    if (b.IsCasting)
                    {
                        if (!Casters.Contains(b.Address)) 
                        {
                            CastInfos[b.Address] = new(b.CastActionId, Environment.TickCount64 - (long)(b.CurrentCastTime * 1000));
                            Casters.Add(b.Address);
                            PluginLog.Debug($"{b.Name} starts casting {b.CastActionId}");
                        }
                    }
                    else
                    {
                        if (Casters.Contains(b.Address))
                        {
                            PluginLog.Debug($"{b.Name} stops casting");
                            Casters.Remove(b.Address);
                        }
                    }
                }
            }
        }

        internal static bool TryGetCastTime(IntPtr ptr, IEnumerable<uint> castId, out float castTime)
        {
            if(CastInfos.TryGetValue(ptr, out var info))
            {
                if(castId.Contains(info.ID))
                {
                    castTime = (float)(Environment.TickCount64 - info.StartTime) / 1000f;
                    return true;
                }
            }
            castTime = default;
            return false;
        }
    }
}
