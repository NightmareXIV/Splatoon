using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splatoon
{
    public unsafe interface IMemoryManager
    {
        public delegate bool W2SDelegate<A, B>(A input, out B output);
        /// <summary>
        /// Must return 0 if memory manager initialized successfully and non-0 if not
        /// </summary>
        public int ErrorCode { get; set; }
        public float GetCamAngleX();
        public float GetCamAngleY();
        public float GetCamZoom();
        public bool GetIsTargetable(GameObject a);
        public bool GetIsVisible(Character chr);
        public int GetModelId(Character a);
        public uint GetNpcID(GameObject a);
        public W2SDelegate<Vector3, Vector2> WorldToScreen { get; set; }
    }
}
