using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Splatoon
{
    [Serializable]
    class Layout
    {
        [NonSerialized] public static readonly string[] DisplayConditions = 
            //0               1                 2                   3                              4
            { "Always shown", "Only in combat", "Only in instance", "Only in combat AND instance", "Only in combat OR instance" }; 
        [NonSerialized] public static readonly string[] VisibilityType =
            //0                 1                                 2                                                  3
            { "Always visible", "Between A and B time in combat", "Between A and B time since receiving chat message in combat" };
        public bool Enabled = true;
        public ushort ZoneLock = 0;
        public int DCond = 0;
        public int BattleTimeBegin = 0;
        public int BattleTimeEnd = 0;
        public int Visibility = 0;
        [DefaultValue("")] public string MessageToWatch = "";
        [NonSerialized] internal long MessageTimeStarted = 0;
        public Dictionary<string, Element> Elements = new Dictionary<string, Element>();
        public bool DisableDisabling = false;
    }
}
