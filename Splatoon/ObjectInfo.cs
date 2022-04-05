using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splatoon
{
    internal class ObjectInfo
    {
        internal long ExistenceTicks = 0;
        internal long TargetableTicks = 0;
        internal long VisibleTicks = 0;
        internal bool Visible = false;
        internal bool Targetable = false;
        internal bool IsChar = false;
    }
}
