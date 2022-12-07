using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splatoon.SplatoonScripting
{
    public enum DirectorUpdateCategory : uint
    {
        Commence = 0x40000001,
        Recommence = 0x40000006,
        Complete = 0x40000003,
        Wipe = 0x40000005
    }
}
