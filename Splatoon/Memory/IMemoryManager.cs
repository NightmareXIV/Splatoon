using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splatoon
{
    public unsafe interface IMemoryManager
    {
        public int ErrorCode { get; set; }
        public float* CameraAddressX { get; set; }
        public float* CameraAddressY { get; set; }
        public float* CameraZoom { get; set; }
        public bool GetIsTargetable(GameObject a);
    }
}
