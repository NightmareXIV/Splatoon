using Dalamud.Game.Network;
using Dalamud.Hooking;
using ECommons.GameFunctions;
using Reloaded.Hooks.Definitions.X64;
using Splatoon.Modules;
using Splatoon.Structures;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Splatoon.Memory
{
    internal unsafe static class AttachedInfo
    {
        delegate IntPtr GameObject_ctor(IntPtr obj);
        static Hook<GameObject_ctor> GameObject_ctor_hook = null;
        internal static Dictionary<IntPtr, CachedCastInfo> CastInfos = new();
        internal static Dictionary<IntPtr, Dictionary<string, VFXInfo>> VFXInfos = new();
        static HashSet<IntPtr> Casters = new();

        [Function(Reloaded.Hooks.Definitions.X64.CallingConventions.Microsoft)]
        delegate IntPtr ActorVfxCreateDelegate2(char* a1, IntPtr a2, IntPtr a3, float a4, char a5, ushort a6, char a7);
        static Hook<ActorVfxCreateDelegate2> ActorVfxCreateHook;

        internal static void Init()
        {
            Safe(delegate
            {
                GameObject_ctor_hook = Hook<GameObject_ctor>.FromAddress(Svc.SigScanner.ScanText("E8 ?? ?? ?? ?? 48 8D 05 ?? ?? ?? ?? 48 8D 35"), GameObject_ctor_detour);
                GameObject_ctor_hook.Enable();
            });
            Safe(delegate
            {
                var actorVfxCreateAddress = Svc.SigScanner.ScanText("40 53 55 56 57 48 81 EC ?? ?? ?? ?? 0F 29 B4 24 ?? ?? ?? ?? 48 8B 05 ?? ?? ?? ?? 48 33 C4 48 89 84 24 ?? ?? ?? ?? 0F B6 AC 24 ?? ?? ?? ?? 0F 28 F3 49 8B F8");
                ActorVfxCreateHook = Hook<ActorVfxCreateDelegate2>.FromAddress(actorVfxCreateAddress, ActorVfxNewHandler);
                ActorVfxCreateHook.Enable();
            });
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
            ActorVfxCreateHook?.Disable();
            ActorVfxCreateHook?.Dispose();
            CastInfos = null;
            VFXInfos = null;
        }

        static IntPtr ActorVfxNewHandler(char* a1, IntPtr a2, IntPtr a3, float a4, char a5, ushort a6, char a7)
        {
            try
            {
                var vfxPath = Dalamud.Memory.MemoryHelper.ReadString(new IntPtr(a1), Encoding.ASCII, 256);
                if (!VFXInfos.ContainsKey(a2))
                {
                    VFXInfos[a2] = new();
                }
                VFXInfos[a2][vfxPath] = new()
                {
                    SpawnTime = Environment.TickCount64
                };
                if (P.Config.Logging)
                {
                    var obj = Svc.Objects.CreateObjectReference(a2);
                    if (obj is Character c)
                    {
                        Logger.Log($"VFX {vfxPath} spawned on {obj.ToString()} npc id={obj.Struct()->GetNpcID()}, model id={c.Struct()->ModelCharaId}, name npc id={c.NameId}, position={obj.Position.ToString()}");
                    }
                    else
                    {
                        Logger.Log($"VFX {vfxPath} spawned on {obj.ToString()} npc id={obj.Struct()->GetNpcID()}, position={obj.Position.ToString()}");
                    }
                }
            }
            catch(Exception e)
            {
                e.Log();
            }
            return ActorVfxCreateHook.Original(a1, a2, a3, a4, a5, a6, a7);
        }

        internal static bool TryGetVfx(this GameObject go, out Dictionary<string, VFXInfo> fx)
        {
            if (VFXInfos.ContainsKey(go.Address))
            {
                fx = VFXInfos[go.Address];
                return true;
            }
            fx = default;
            return false;
        }

        internal static bool TryGetSpecificVfxInfo(this GameObject go, string path, out VFXInfo info)
        {
            if (TryGetVfx(go, out var dict) && dict.ContainsKey(path))
            {
                info = dict[path];
                return true;
            }
            info = default;
            return false;
        }

        static IntPtr GameObject_ctor_detour(IntPtr ptr)
        {
            CastInfos.Remove(ptr);
            Casters.Remove(ptr);
            VFXInfos.Remove(ptr);
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
                            if (P.Config.Logging)
                            {
                                Logger.Log($"{b.Name} starts casting {b.CastActionId}");
                            }
                        }
                    }
                    else
                    {
                        if (Casters.Contains(b.Address))
                        {
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
