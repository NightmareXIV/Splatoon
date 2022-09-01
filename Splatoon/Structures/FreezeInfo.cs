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
        internal long ShowAt = 0;

        internal bool IsActive()
        {
            return ShowUntil > Environment.TickCount64 && Environment.TickCount64 >= ShowAt;
        }

        internal bool IsExpired()
        {
            return ShowUntil < Environment.TickCount64;
        }
    }

}
