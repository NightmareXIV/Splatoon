using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splatoon
{
    public unsafe interface IMemoryManager
    {
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
    }
}
