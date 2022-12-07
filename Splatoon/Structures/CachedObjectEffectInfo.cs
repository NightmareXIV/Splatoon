namespace Splatoon.Structures;

internal record struct CachedObjectEffectInfo
{
    internal long StartTime;
    internal ushort data1;
    internal ushort data2;

    internal CachedObjectEffectInfo(long startTime, ushort data1, ushort data2)
    {
        this.StartTime = startTime;
        this.data1 = data1;
        this.data2 = data2;
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
