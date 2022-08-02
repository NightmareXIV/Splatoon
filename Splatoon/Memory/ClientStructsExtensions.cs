using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splatoon.Memory
{
    internal unsafe static class ClientStructsExtensions
    {
        internal static bool GetIsTargetable(this GameObject a)
        {
            return ((FFXIVClientStructs.FFXIV.Client.Game.Object.GameObject*)a.Address)->GetIsTargetable();
        }

        internal static uint GetNpcID(this GameObject a)
        {
            return ((FFXIVClientStructs.FFXIV.Client.Game.Object.GameObject*)a.Address)->GetNpcID();
        }
    }
}
