using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Splatoon
{
    unsafe class GlobalMemory : IMemoryManager
    {
        public int ErrorCode { get; set; } = -1;
        public IMemoryManager.W2SDelegate<Vector3, Vector2> WorldToScreen { get; set; } = Svc.GameGui.WorldToScreen;

        IntPtr cameraAddressPtr;
        float* Xptr;
        float* Yptr;
        float* ZoomPtr;

        

        /*[UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate byte Character_GetIsTargetable(IntPtr characterPtr);
        private Character_GetIsTargetable GetIsTargetable_Character;

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate byte GameObject_GetIsTargetable(IntPtr characterPtr);
        private GameObject_GetIsTargetable GetIsTargetable_GameObject;*/

        public GlobalMemory(Splatoon p)
        {
            try
            {
                //throw new Exception("Failsafe mode.");
                if (p.Config.NoMemory) throw new Exception("No memory mode was requested by an user.");
                //p.Config.NoMemory = true;
                //p.Config.Save();
                /*GetIsTargetable_Character = Marshal.GetDelegateForFunctionPointer<Character_GetIsTargetable>(
                    Svc.SigScanner.ScanText("F3 0F 10 89 ?? ?? ?? ?? 0F 57 C0 0F 2E C8 7A 05 75 03 32 C0 C3 80 B9"));
                GetIsTargetable_GameObject = Marshal.GetDelegateForFunctionPointer<GameObject_GetIsTargetable>(
                    Svc.SigScanner.ScanText("0F B6 91 ?? ?? ?? ?? F6 C2 02"));*/
                //var ptr = Svc.SigScanner.GetStaticAddressFromSig("48 8D 35 ?? ?? ?? ?? 48 8B 09");
                cameraAddressPtr = *(IntPtr*)Svc.SigScanner.GetStaticAddressFromSig("48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8B 8B ?? ?? ?? ?? 48 85 C9 74 11 48 8B 01");
                if (cameraAddressPtr == IntPtr.Zero) throw new Exception("Camera address was zero");
                PluginLog.Information($"Camera address ptr: {cameraAddressPtr:X16}");
                Xptr = (float*)(cameraAddressPtr + 0x130);
                Yptr = (float*)(cameraAddressPtr + 0x134);
                ZoomPtr = (float*)(cameraAddressPtr + 0x114);
                ErrorCode = 0;
                PluginLog.Information("Memory manager initialized successfully");
                //p.Config.NoMemory = false;
                //p.Config.Save();
            }
            catch(Exception e)
            {
                PluginLog.Error($"Failed to initialize memory manager.\n{e.Message}\n{e.StackTrace.NotNull()}\nSplatoon is using failsafe mode.");
                ErrorCode = 1;
            }
        }

        public bool GetIsTargetable(GameObject a)
        {
            /*if (ErrorCode != 0) return true;
            if (a is Character)
            {
                return GetIsTargetable_Character(a.Address) != 0;
            }
            else
            {
                return GetIsTargetable_GameObject(a.Address) != 0;
            }*/
            return ((FFXIVClientStructs.FFXIV.Client.Game.Object.GameObject*)a.Address)->GetIsTargetable();
        }

        public bool GetIsVisible(Character chr)
        {
            if (ErrorCode != 0) return true;
            var v = (IntPtr)(((FFXIVClientStructs.FFXIV.Client.Game.Character.Character*)chr.Address)->GameObject.DrawObject);
            if (v == IntPtr.Zero) return false;
            return Bitmask.IsBitSet(*(byte*)(v + 136), 0);
        }

        public int GetModelId(Character a)
        {
            if (ErrorCode != 0) return 0;
            return *(int*)(a.Address + 0x01B4);
        }

        public float GetCamAngleX()
        {
            if(ErrorCode != 0)
            {
                return 0;
            }
            else
            {
                return *Xptr;
            }
        }

        public float GetCamAngleY()
        {
            if (ErrorCode != 0)
            {
                return 0;
            }
            else
            {
                return *Yptr;
            }
        }

        public float GetCamZoom()
        {
            if (ErrorCode != 0)
            {
                return 10;
            }
            else
            {
                return *ZoomPtr;
            }
        }
    }
}
