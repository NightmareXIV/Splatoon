#nullable enable
using ECommons.Hooks;
using ECommons.Hooks.ActionEffectTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;

namespace Splatoon.SplatoonScripting;

public abstract class SplatoonScript
{
    protected SplatoonScript()
    {
        Controller = new(this);
    }

    /// <summary>
    /// Controller provides easy access to various helper functions that may be helpful for your script.
    /// </summary>
    public Controller Controller { get; }

    /// <summary>
    /// Metadata of a script that optionally contains author, description, version and script's origin website. This data will be displayed in Splatoon's interface.
    /// </summary>
    public virtual Metadata? Metadata { get; }

    /// <summary>
    /// Indicates whether your script operates strictly within Splatoon, ECommons and Dalamud APIs. 
    /// </summary>
    public virtual bool Safe { get; } = false;

    public InternalData InternalData { get; internal set; } = null!;

    /// <summary>
    /// Valid territories where script will be executed. Specify an empty array if you want it to work in all territories. 
    /// </summary>
    public abstract HashSet<uint> ValidTerritories { get; }

    /// <summary>
    /// Indicates whether script is currently enabled and should be executed or not.
    /// </summary>
    public bool IsEnabled { get; private set; } = false;

    internal bool IsDisabledByUser => P.Config.DisabledScripts.Contains(this.InternalData.FullName);

    /// <summary>
    /// Executed once after script is compiled and loaded into memory. Setup your layouts, elements and other static data that is not supposed to change within a game session. You should not setup any hooks or direct Dalamud events here, as method to cleanup is not provided (by design). Such things are to be done in OnEnable method.
    /// </summary>
    public virtual void OnSetup() { }

    /// <summary>
    /// Executed when player enters whitelisted territory. Will not trigger when player moves from one whitelisted territory to another.
    /// </summary>
    public virtual void OnEnable() { }

    /// <summary>
    /// Executed when player leaves whitelisted territory. Will not trigger when player moves from one non-whitelisted territory to another.
    /// </summary>
    public virtual void OnDisable() { }

    /// <summary>
    /// Will be called on combat start. This method will only be called if a script is enabled.
    /// </summary>
    public virtual void OnCombatStart() { }

    /// <summary>
    /// Will be called on combat end. This method will only be called if a script is enabled.
    /// </summary>
    public virtual void OnCombatEnd() { }

    /// <summary>
    /// Will be called on phase change. This method will only be called if a script is enabled. This method will be called if user manually changes phase as well.
    /// </summary>
    /// <param name="newPhase">New phase</param>
    public virtual void OnPhaseChange(int newPhase) { }

    /// <summary>
    /// Will be called on receiving map effect. This method will only be called if a script is enabled.
    /// </summary>
    /// <param name="position">Positional data of map effect. It is not related to actual map coordinates.</param>
    /// <param name="data1">First parameter of map effect.</param>
    /// <param name="data2">Second parameter of map effect.</param>
    public virtual void OnMapEffect(uint position, ushort data1, ushort data2) { }

    /// <summary>
    /// Will be called on receiving object effect. This method will only be called if a script is enabled.
    /// </summary>
    /// <param name="target">Targeted object's ID</param>
    /// <param name="data1">First parameter of object effect.</param>
    /// <param name="data2">Second parameter of object effect.</param>
    public virtual void OnObjectEffect(uint target, ushort data1, ushort data2) { }

    /// <summary>
    /// Will be called when a tether created between two game objects. This method will only be called if a script is enabled.
    /// </summary>
    /// <param name="source">Source object ID of pair.</param>
    /// <param name="target">Target object ID of pair.</param>
    /// <param name="data2">Second argument of hooked method.</param>
    /// <param name="data3">Third argument of hooked method.</param>
    /// <param name="data5">Fifth argument of hooked method.</param>
    public virtual void OnTetherCreate(uint source, uint target, byte data2, byte data3, byte data5) { }

    /// <summary>
    /// Will be called when a previously created tether between two game objects removed. This method will only be called if a script is enabled.
    /// </summary>
    /// <param name="source">Source object ID of pair.</param>
    /// <param name="data2">Second argument of hooked method.</param>
    /// <param name="data3">Third argument of hooked method.</param>
    /// <param name="data5">Fifth argument of hooked method.</param>
    public virtual void OnTetherRemoval(uint source, byte data2, byte data3, byte data5) { }

    /// <summary>
    /// Will be called when a VFX spawns on a certain game object. This method will only be called if a script is enabled.
    /// </summary>
    /// <param name="target">Object ID that is targeted by VFX.</param>
    /// <param name="vfxPath">VFX game path</param>
    public virtual void OnVFXSpawn(uint target, string vfxPath) { }

    /// <summary>
    /// Will be called whenever plugin processes a message. These are the same messages which layout trigger system receives. This method will only be called if a script is enabled.
    /// </summary>
    /// <param name="Message"></param>
    public virtual void OnMessage(string Message) { }

    /// <summary>
    /// Will be called when a duty director update is happening, for example, joining, restarting, or wiping in duty. 
    /// </summary>
    /// <param name="category">Director update category</param>
    public virtual void OnDirectorUpdate(DirectorUpdateCategory category) { }

    public virtual void OnObjectCreation(nint newObjectPtr) { }

    public virtual void OnActionEffect(uint ActionID, ushort animationID, ActionEffectType type, uint sourceID, ulong targetOID, uint damage) { }

    /// <summary>
    /// Will be called every framework update. You can execute general logic of your script here. 
    /// </summary>
    public virtual void OnUpdate() { }

    /// <summary>
    /// If you override this method, settings section will be added to your script. You can call ImGui methods in this function to draw configuration UI. Keep it simple.
    /// </summary>
    public virtual void OnSettingsDraw() { }

    internal void DrawRegisteredElements()
    {
        ImGui.Checkbox($"Unconditional draw", ref InternalData.UnconditionalDraw);
        foreach (var x in Controller.GetRegisteredElements())
        {
            ImGui.PushID(x.Value.GUID);
            ImGuiEx.HashSetCheckbox($"Enable draw", x.Value.GUID, InternalData.UnconditionalDrawElements);
            ImGui.SameLine();
            if (ImGui.Button("Copy"))
            {
                ImGui.SetClipboardText(JsonConvert.SerializeObject(x.Value, new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Ignore }));
            }
            ImGui.SameLine();
            if (ImGui.Button("Edit"))
            {
                if (!InternalData.Overrides.Elements.ContainsKey(x.Key))
                {
                    Notify.Info($"Created override for {x.Key}");
                    InternalData.Overrides.Elements[x.Key] = x.Value;
                }
                P.PinnedElementEditWindow.Open(this, x.Key);
            }
            ImGui.SameLine();
            if (InternalData.Overrides.Elements.ContainsKey(x.Key))
            {
                if(ImGui.Button("Reset") && ImGui.GetIO().KeyCtrl)
                {
                    if(ReferenceEquals(P.PinnedElementEditWindow.EditingElement, x.Value))
                    {
                        P.PinnedElementEditWindow.EditingElement = null;
                        P.PinnedElementEditWindow.Script = null;
                    }
                    InternalData.Overrides.Elements.Remove(x.Key);
                    Controller.SaveOverrides();
                }
            }
            ImGui.SameLine();
            ImGuiEx.Text($"[{x.Key}] {x.Value.Name}");
            ImGui.PopID();
        }
    }

    public bool DoSettingsDraw => this.GetType().GetMethod(nameof(OnSettingsDraw))?.DeclaringType != typeof(SplatoonScript);

    internal bool Enable()
    {
        if (IsEnabled || IsDisabledByUser || !this.InternalData.Allowed || this.InternalData.Blacklisted)
        {
            return false;
        }
        try
        {
            PluginLog.Information($"Enabling script {this.InternalData.Name}");
            this.OnEnable();
        }
        catch (Exception ex)
        {
            ex.Log();
        }
        this.IsEnabled = true;
        return true;
    }

    internal bool Disable()
    {
        this.Controller.SaveConfig();
        if (!IsEnabled)
        {
            return false;
        }
        try
        {
            PluginLog.Information($"Disabling script {this}");
            this.OnDisable();
        }
        catch (Exception ex)
        {
            ex.Log();
        }
        this.IsEnabled = false;
        return true;
    }
}
