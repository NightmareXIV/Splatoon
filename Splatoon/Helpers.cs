using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splatoon
{
    class Helpers
    {
        public static void ToggleHashSet<T>(ref HashSet<T> h, T o)
        {
            if(h.Contains(o))
            {
                h.Remove(o);
            }
            else
            {
                h.Add(o);
            }
        }
    }
}
