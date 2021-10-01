using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Splatoon
{
    [Serializable]
    class Layout
    {
        /// <summary>
        /// 0: Always shown |
        /// 1: Only in combat |
        /// 2: Only in instance |
        /// 3: Only in combat AND instance |
        /// 4: Only in combat OR instance |
        /// 5: Never
        /// </summary>
        [NonSerialized] public static readonly string[] DisplayConditions = 
            //0               1                 2                   3                              4
            { "Always shown", "Only in combat", "Only in instance", "Only in combat AND instance", "Only in combat OR instance", "Never" }; 
        [NonSerialized] public static readonly string[] VisibilityType =                                 
            { "Always visible", 
            "Show between A and B time since combat has started",
            "Show between A and B time since receiving a message in combat", 
            "Show on receiving message A, hide on receiving message B in combat", 
            "External control, hidden by default" };
        [DefaultValue(true)] public bool Enabled = true;
        public HashSet<ushort> ZoneLockH = new HashSet<ushort>();
        [DefaultValue(0)] public int DCond = 0;
        [DefaultValue(0)] public int BattleTimeBegin = 0;
        [DefaultValue(0)] public int BattleTimeEnd = 0;
        [DefaultValue(0)] public int Visibility = 0;
        [DefaultValue("")] public string MessageToWatch = "";
        [NonSerialized] internal long MessageTimeStarted = 0;
        public Dictionary<string, Element> Elements = new Dictionary<string, Element>();
        [DefaultValue(false)] public bool DisableDisabling = false;
        [DefaultValue(0)] public ulong JobLock = 0;
        [DefaultValue("")] public string MessageToWatchForEnd = "";
        [DefaultValue(true)] public bool AutoHideOutCombat = true;
        [DefaultValue(false)] public bool DisableInDuty = false;
        [DefaultValue(false)] public bool UseTriggers = false;
        public List<Trigger> Triggers = new List<Trigger>();
    }
}
