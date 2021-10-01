using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splatoon
{
    [Serializable]
    public class Trigger
    {
        /// <summary>
        /// 0: Show at time |
        /// 1: Hide at time |
        /// 2: Show at text |
        /// 3: Hide at text
        /// </summary>
        [DefaultValue(0)] public int Type = 0;
        [DefaultValue(0)] public long TimeBegin = 0;
        [DefaultValue(0)] public long Duration = 0;
        [DefaultValue("")] public string Match = "";
        [DefaultValue(true)] public bool ResetOnCombatExit = true;
        [DefaultValue(true)] public bool ResetOnTChange = true;
        [NonSerialized] public bool HasFired = false;
    }
}
