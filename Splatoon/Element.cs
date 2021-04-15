using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splatoon
{
    [Serializable]
    public class Element
    {
        [NonSerialized] public static string[] ElementTypes = { "Shape at fixed coordinates", "Shape relative to actor position" };
        [NonSerialized] public static string[] ActorTypes = { "Game object with specific name", "Self", "Targeted enemy" };
        public int type;
        public Element(int t)
        {
            type = t;
        }
        public bool Enabled = true;
        public float refX = 0f;
        public float refY = 0f;
        public float refZ = 0f;
        public float offX = 0f;
        public float offY = 0f;
        public float offZ = 0f;
        public float radius = 0.35f; // if it's 0, draw it as point, otherwise as circle
        public uint color = 0xc80000ff;
        public uint overlayBGColor = 0x70000000;
        public uint overlayTextColor = 0xC8FFFFFF;
        public float overlayVOffset = 0f;
        public float thicc = 2f;
        [DefaultValue("")] public string overlayText = "";
        [DefaultValue("")] public string refActorName = "";
        public int refActorType = 0;
        public bool includeHitbox = false;
        public bool includeOwnHitbox = false;
        public bool includeRotation = false;
    }
}
