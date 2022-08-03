namespace Splatoon.Structures
{
    internal class FreezeInfo
    {
        internal List<FreezeState> States = new();
        internal long AllowRefreezeAt;

        internal bool CanDisplay()
        {
            return Environment.TickCount64 > AllowRefreezeAt;
        }
    }

    internal class FreezeState
    {
        internal HashSet<DisplayObject> Objects;
        internal long ShowUntil;

        internal bool IsActive()
        {
            return ShowUntil > Environment.TickCount64;
        }
    }

}
