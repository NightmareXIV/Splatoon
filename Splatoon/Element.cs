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
        [NonSerialized] public static string[] ElementTypes = { "Object at fixed coordinates", "Object relative to actor position", 
        "Line between two fixed coordinates"};
        [NonSerialized] public static string[] ActorTypes = { "Game object with specific name", "Self", "Targeted enemy" };
        [NonSerialized] public int screen2world = 0;
        public int type;
        public Element(int t)
        {
            type = t;
        }
        [DefaultValue(true)] public bool Enabled = true;
        [DefaultValue(0f)] public float refX = 0f;
        [DefaultValue(0f)] public float refY = 0f;
        [DefaultValue(0f)] public float refZ = 0f;
        [DefaultValue(0f)] public float offX = 0f;
        [DefaultValue(0f)] public float offY = 0f;
        [DefaultValue(0f)] public float offZ = 0f;
        [DefaultValue(0.35f)] public float radius = 0.35f; // if it's 0, draw it as point, otherwise as circle
        [DefaultValue(0xc80000ff)] public uint color = 0xc80000ff;
        [DefaultValue(0x70000000)] public uint overlayBGColor = 0x70000000;
        [DefaultValue(0xC8FFFFFF)] public uint overlayTextColor = 0xC8FFFFFF;
        [DefaultValue(0f)] public float overlayVOffset = 0f;
        [DefaultValue(2f)] public float thicc = 2f;
        [DefaultValue("")] public string overlayText = "";
        [DefaultValue("")] public string refActorName = "";
        [DefaultValue(0)] public int refActorType = 0;
        [DefaultValue(false)] public bool includeHitbox = false;
        [DefaultValue(false)] public bool includeOwnHitbox = false;
        [DefaultValue(false)] public bool includeRotation = false;
        [DefaultValue(false)] public bool onlyTargetable = false;
    }
}
