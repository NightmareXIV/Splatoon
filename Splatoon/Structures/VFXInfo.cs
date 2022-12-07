using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splatoon.Structures;

internal record struct VFXInfo
{
    internal long SpawnTime;

    internal long Age
    {
        get
        {
            return Environment.TickCount64 - SpawnTime;
        }
    }

    internal float AgeF
    {
        get
        {
            return (float)Age / 1000f;
        }
    }
}
