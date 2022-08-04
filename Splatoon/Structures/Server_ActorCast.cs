using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Splatoon.Structures
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Server_ActorCast
    {
        public ushort ActionID;
        public byte SkillType;
        public byte Unknown;
        public uint Unknown1; // also action ID
        public float CastTime;
        public uint TargetID;
        public float Rotation; // in radians
        public uint Unknown2;
        public ushort PosX;
        public ushort PosY;
        public ushort PosZ;
        public ushort Unknown3;
    }
}
