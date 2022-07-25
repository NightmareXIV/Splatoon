using System.ComponentModel;

namespace Splatoon;

[Serializable]
public class Element
{
    [NonSerialized] public static string[] ElementTypes = { "Circle at fixed coordinates", "Circle relative to object position",
    "Line between two fixed coordinates", "Line relative to object position", "Cone relative to object position (beta)"};
    [NonSerialized] public static string[] ActorTypes = { "Game object with specific data", "Self", "Targeted enemy" };
    [NonSerialized] public static string[] ComparisonTypes = { "Name (case-insensitive, partial)", "Model ID", "Object ID", "Data ID", "NPC ID", "Placeholder", "NPC Name ID" };


    public string Name = "";
    [NonSerialized] internal string GUID = Guid.NewGuid().ToString();
    [NonSerialized] internal bool Delete = false;
    /// <summary>
    /// 0: Object at fixed coordinates |
    /// 1: Object relative to actor position | 
    /// 2: Line between two fixed coordinates | 
    /// 3: Line relative to object pos | 
    /// 4: Cone relative to object position
    /// </summary>
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
    [DefaultValue(0)] public float Donut = 0f;
    [DefaultValue(0)] public int coneAngleMin = 0;
    [DefaultValue(0)] public int coneAngleMax = 0;
    [DefaultValue(0xc80000ff)] public uint color = 0xc80000ff;
    [DefaultValue(0x70000000)] public uint overlayBGColor = 0x70000000;
    [DefaultValue(0xC8FFFFFF)] public uint overlayTextColor = 0xC8FFFFFF;
    [DefaultValue(0f)] public float overlayVOffset = 0f;
    [DefaultValue(1f)] public float overlayFScale = 1f;
    [DefaultValue(false)] public bool overlayPlaceholders = false;
    [DefaultValue(2f)] public float thicc = 2f;
    [DefaultValue("")] public string overlayText = "";
    [DefaultValue("")] public string refActorName = "";
    public InternationalString refActorNameIntl = new();
    [DefaultValue(0)] public uint refActorModelID = 0;
    [DefaultValue(0)] public uint refActorObjectID = 0;
    [DefaultValue(0)] public uint refActorDataID = 0;
    [DefaultValue(0)] public uint refActorNPCID = 0;
    [DefaultValue("")] public List<string> refActorPlaceholder = new();
    [DefaultValue(0)] public uint refActorNPCNameID = 0;
    [DefaultValue(false)] public bool refActorRequireCast = false;
    public List<uint> refActorCastId = new();
    [DefaultValue(false)] public bool refActorRequireBuff = false;
    public List<uint> refActorBuffId = new();
    [DefaultValue(false)] public bool refActorRequireAllBuffs = false;
    [DefaultValue(false)] public bool refActorRequireBuffsInvert = false;
    [DefaultValue(false)] public bool refActorObjectLife = false;
    [DefaultValue(0)] public float refActorLifetimeMin = 0;
    [DefaultValue(0)] public float refActorLifetimeMax = 0;
    [DefaultValue(0.5f)] public float FillStep = 0.5f;
    /// <summary>
    /// 0: Name |
    /// 1: Model ID |
    /// 2: Object ID |
    /// 3: Data ID | 
    /// 4: NPC ID |
    /// 5: Placeholder |
    /// 6: Name ID
    /// </summary>
    [DefaultValue(0)] public int refActorComparisonType = 0;
    /// <summary>
    /// 0: Game object with specific name |
    /// 1: Self |
    /// 2: Targeted enemy
    /// </summary>
    [DefaultValue(0)] public int refActorType = 0;
    [DefaultValue(false)] public bool includeHitbox = false;
    [DefaultValue(false)] public bool includeOwnHitbox = false;
    [DefaultValue(false)] public bool includeRotation = false;
    [DefaultValue(false)] public bool onlyTargetable = false;
    [DefaultValue(false)] public bool onlyUnTargetable = false;
    [DefaultValue(false)] public bool onlyVisible = false;
    [DefaultValue(false)] public bool tether = false;
    [DefaultValue(0f)] public float AdditionalRotation = 0f;
    [DefaultValue(false)] public bool LineAddHitboxLengthX = false;
    [DefaultValue(false)] public bool LineAddHitboxLengthY = false;
    [DefaultValue(false)] public bool LineAddHitboxLengthZ = false;
    [DefaultValue(false)] public bool LineAddHitboxLengthXA = false;
    [DefaultValue(false)] public bool LineAddHitboxLengthYA = false;
    [DefaultValue(false)] public bool LineAddHitboxLengthZA = false;
    [DefaultValue(false)] public bool LineAddPlayerHitboxLengthX = false;
    [DefaultValue(false)] public bool LineAddPlayerHitboxLengthY = false;
    [DefaultValue(false)] public bool LineAddPlayerHitboxLengthZ = false;
    [DefaultValue(false)] public bool LineAddPlayerHitboxLengthXA = false;
    [DefaultValue(false)] public bool LineAddPlayerHitboxLengthYA = false;
    [DefaultValue(false)] public bool LineAddPlayerHitboxLengthZA = false;
    [DefaultValue(false)] public bool Filled = false;
    [DefaultValue(false)] public bool FaceMe = false;

    public bool ShouldSerializerefActorNameIntl()
    {
        return ShouldSerializerefActorName() && !refActorNameIntl.IsEmpty();
    }

    public bool ShouldSerializeconeAngleMax()
    {
        return ShouldSerializeconeAngleMin();
    }

    public bool ShouldSerializeconeAngleMin()
    {
        return this.type == 4;
    }

    public bool ShouldSerializerefActorLifetimeMax()
    {
        return ShouldSerializerefActorLifetimeMin();
    }

    public bool ShouldSerializerefActorLifetimeMin()
    {
        return refActorObjectLife;
    }

    public bool ShouldSerializerefActorCastId()
    {
        return refActorRequireCast && refActorCastId.Count > 0;
    }

    public bool ShouldSerializerefActorBuffId()
    {
        return refActorRequireBuff && refActorBuffId.Count > 0;
    }

    public bool ShouldSerializerefActorName()
    {
        return refActorComparisonType == 0;
    }

    public bool ShouldSerializerefActorModelID()
    {
        return refActorComparisonType == 1;
    }

    public bool ShouldSerializerefActorObjectID()
    {
        return refActorComparisonType == 2;
    }

    public bool ShouldSerializerefActorDataID()
    {
        return refActorComparisonType == 3;
    }

    public bool ShouldSerializerefActorNPCID()
    {
        return refActorComparisonType == 4;
    }

    public bool ShouldSerializerefActorPlaceholder()
    {
        return refActorComparisonType == 5;
    }

    public bool ShouldSerializerefActorNPCNameID()
    {
        return refActorComparisonType == 6;
    }

    public bool ShouldSerializerefX()
    {
        return this.type != 1;
    }
    public bool ShouldSerializerefY() { return ShouldSerializerefX(); }
    public bool ShouldSerializerefZ() { return ShouldSerializerefX(); }

    public bool ShouldSerializeDonut()
    {
        return this.type.EqualsAny(0, 1) && Donut > 0;
    }
}
