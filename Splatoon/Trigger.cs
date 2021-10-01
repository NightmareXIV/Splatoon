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
        [NonSerialized] public static string[] Types = { "Show at time in combat", "Hide at time in combat", "Show at message", "Hide at message" };
        /// <summary>
        /// 0: Show at time |
        /// 1: Hide at time |
        /// 2: Show at text |
        /// 3: Hide at text
        /// </summary>
        [DefaultValue(0)] public int Type = 0;
        [DefaultValue(0)] public int TimeBegin = 0;
        [DefaultValue(0)] public int Duration = 0;
        [DefaultValue("")] public string Match = "";
        [DefaultValue(true)] public bool ResetOnCombatExit = true;
        [DefaultValue(true)] public bool ResetOnTChange = true;
        [NonSerialized] public bool HasFired = false;
    }
}
