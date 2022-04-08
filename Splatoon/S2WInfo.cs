using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splatoon
{
    internal class S2WInfo
    {
        string x;
        string y;
        string z;
        object cls;

        internal S2WInfo(object cls, string x, string y, string z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.cls = cls;
        }

        internal void Apply(float xf, float yf, float zf)
        {
            cls.GetType().GetField(x).SetValue(cls, xf);
            cls.GetType().GetField(y).SetValue(cls, yf);
            cls.GetType().GetField(z).SetValue(cls, zf);
        }

        internal (float x, float y, float z) GetValues()
        {
            return ((float)cls.GetType().GetField(x).GetValue(cls), (float)cls.GetType().GetField(y).GetValue(cls), (float)cls.GetType().GetField(z).GetValue(cls));
        }
    }
}
