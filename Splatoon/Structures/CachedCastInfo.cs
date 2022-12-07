using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splatoon.Structures;

internal record struct CachedCastInfo
{
    internal uint ID;
    internal long StartTime;

    public CachedCastInfo(uint iD, long startTime)
    {
        ID = iD;
        StartTime = startTime;
    }

    internal float StartTimeF
    {
        get
        {
            return (float)StartTime / 1000f;
        }
    }

    internal long Age
    {
        get
        {
            return Environment.TickCount64 - StartTime;
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
