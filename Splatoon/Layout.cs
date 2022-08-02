﻿using System.ComponentModel;

namespace Splatoon;

[Serializable]
public class Layout
{
    [NonSerialized] public static readonly string[] DisplayConditions = 
        //0               1                 2                   3                              4
        { "Always shown", "Only in combat", "Only in instance", "Only in combat AND instance", "Only in combat OR instance", "On trigger only" }; 
    [DefaultValue(true)] public bool Enabled = true;
    public string Name = "";
    public string Group = "";
    [NonSerialized] internal string GUID = Guid.NewGuid().ToString();
    [NonSerialized] internal bool Delete = false;
    public HashSet<ushort> ZoneLockH = new();

    /// <summary>
    /// 0: Always shown |
    /// 1: Only in combat |
    /// 2: Only in instance |
    /// 3: Only in combat AND instance |
    /// 4: Only in combat OR instance |
    /// 5: Never
    /// </summary>
    [DefaultValue(0)] public int DCond = 0;
    [Obsolete] public Dictionary<string, Element> Elements = new(); //never delete
    public List<Element> ElementsL = new();
    [DefaultValue(false)] public bool DisableDisabling = false;
    [DefaultValue(0)] public ulong JobLock = 0;
    [DefaultValue(false)] public bool DisableInDuty = false;
    [DefaultValue(false)] public bool UseTriggers = false;
    public List<Trigger> Triggers = new();
    /// <summary>
    /// 0: Unchanged |
    /// -1: Hidden |
    /// 1: Shown
    /// </summary>
    [NonSerialized] public int TriggerCondition = 0;
    [DefaultValue(0f)] public float MinDistance = 0f;
    [DefaultValue(0f)] public float MaxDistance = 0f;
    [DefaultValue(false)] public bool UseDistanceLimit = false;
    [DefaultValue(false)] public bool DistanceLimitMyHitbox = false;
    [DefaultValue(false)] public bool DistanceLimitTargetHitbox = false;
    /// <summary>
    /// 0: To target | 1: To object
    /// </summary>
    [DefaultValue(0)] public int DistanceLimitType = 0;
    [DefaultValue(0)] public int Phase = 0;
    [DefaultValue(false)] public bool Freezing = false;
    [DefaultValue(0f)] public float FreezeFor = 0f;
    [NonSerialized] internal FreezeInfo freezeInfo = null;

    public bool ShouldSerializeMinDistance()
    {
        return this.UseDistanceLimit;
    }

    public bool ShouldSerializeMaxDistance()
    {
        return this.UseDistanceLimit;
    }

    public bool ShouldSerializeDistanceLimitMyHitbox()
    {
        return this.UseDistanceLimit;
    }

    public bool ShouldSerializeDistanceLimitTargetHitbox()
    {
        return this.UseDistanceLimit;
    }

    public bool ShouldSerializeDistanceLimitType()
    {
        return this.UseDistanceLimit;
    }

    public bool ShouldSerializeZoneLockH()
    {
        return ZoneLockH.Count > 0;
    }

    public bool ShouldSerializeTriggers()
    {
        return Triggers.Count > 0;
    }

    public bool ShouldSerializeElements()
    {
        return false;
    }
}
