using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Splatoon
{
    unsafe class Memory
    {
        internal float* CameraAddressX;
        internal float* CameraAddressY;
        internal float* CameraZoom;

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate byte Character_GetIsTargetable(IntPtr characterPtr);
        internal Character_GetIsTargetable GetIsTargetable_Character;

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate byte GameObject_GetIsTargetable(IntPtr characterPtr);
        internal GameObject_GetIsTargetable GetIsTargetable_GameObject;

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate long GameObject_GetObjectID_delegate(IntPtr objectPtr);
        internal GameObject_GetObjectID_delegate GameObject_GetObjectID;

        public Memory(Splatoon p)
        {
            GetIsTargetable_Character = Marshal.GetDelegateForFunctionPointer<Character_GetIsTargetable>(
                Svc.SigScanner.ScanText("F3 0F 10 89 ?? ?? ?? ?? 0F 57 C0 0F 2E C8 7A 05 75 03 32 C0 C3 80 B9"));
            GetIsTargetable_GameObject = Marshal.GetDelegateForFunctionPointer<GameObject_GetIsTargetable>(
                Svc.SigScanner.ScanText("0F B6 91 ?? ?? ?? ?? F6 C2 02"));
            var cameraAddress = *(IntPtr*)Svc.SigScanner.GetStaticAddressFromSig("48 8D 35 ?? ?? ?? ?? 48 8B 34 C6 F3");
            CameraAddressX = (float*)(cameraAddress + 0x130);
            CameraAddressY = (float*)(cameraAddress + 0x134);
            CameraZoom = (float*)(cameraAddress + 0x114);
            GameObject_GetObjectID = Marshal.GetDelegateForFunctionPointer<GameObject_GetObjectID_delegate>(
                Svc.SigScanner.ScanText("40 53 48 83 EC 20 8B 41 74"));
        }
    }

    [StructLayout(LayoutKind.Explicit, Size = 0x8)]
    public struct GameObjectID
    {
        [FieldOffset(0x0)] public uint ObjectID;
        [FieldOffset(0x4)] public byte Type;
    }
}
