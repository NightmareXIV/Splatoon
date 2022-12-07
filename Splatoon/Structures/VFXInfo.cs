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
