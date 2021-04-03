using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splatoon
{
    [Serializable]
    public class ElementFixedPoint : IElement
    {
        public string Name = "";
        public float refX = 0f;
        public float refY = 0f;
        public float refZ = 0f;
        public float offX = 0f;
        public float offY = 0f;
        public float offZ = 0f;
        public float radius = 0.5f;
        public uint color = 0xff0000ff;
        public float thiccness = 1f;
    }
}
