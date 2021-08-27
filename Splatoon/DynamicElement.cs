using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splatoon
{
    class DynamicElement
    {
        public string Name;
        public Layout[] Layouts;
        public Element[] Elements;
        public DestroyAt DestroyTime;
    }

    public enum DestroyAt : long
    {
        NEVER = 0,
        COMBAT_EXIT = -1,
        TERRITORY_CHANGE = -2,
    }
}
