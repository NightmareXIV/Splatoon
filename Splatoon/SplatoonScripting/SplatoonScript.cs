using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
#nullable enable
namespace Splatoon.SplatoonScripting
{
    public abstract class SplatoonScript
    {
        protected Dictionary<string, Layout> Layouts = new();
        protected Dictionary<string, Element> Elements = new();

        /// <summary>
        /// Attempts to register previously exported from plugin layout for further usage. End user will be able to edit this layout as they wish and results of the edit will be saved. Enabled layouts are subject for immediate processing when the script is enabled.
        /// </summary>
        /// <param name="UniqueName">Internal unique (within current script) name of the layout.</param>
        /// <param name="ExportString">An exported layout string.</param>
        /// <param name="layout">Decoded layout object.</param>
        /// <param name="overwrite">Whether to overwrite existing layout with same name if it's present.</param>
        /// <returns>Whether layout was successfully registered.</returns>
        public bool TryRegisterLayoutFromCode(string UniqueName, string ExportString, [NotNullWhen(true)] out Layout? layout, bool overwrite = false)
        {
            return ScriptingEngine.TryDecodeLayout(ExportString, out layout) && TryRegisterLayout(UniqueName, layout, overwrite);
        }

        /// <summary>
        /// Attempts to register previously constructed layout for further usage. End user will be able to edit this layout as they wish and results of the edit will be saved. Enabled layouts are subject for immediate processing when the script is enabled.
        /// </summary>
        /// <param name="UniqueName">Internal unique (within current script) name of the layout.</param>
        /// <param name="layout">Layout object.</param>
        /// <param name="overwrite">Whether to overwrite existing layout with same name if it's present.</param>
        /// <returns>Whether layout was successfully registered.</returns>
        public bool TryRegisterLayout(string UniqueName, Layout layout, bool overwrite = false)
        {
            if(!overwrite && Layouts.ContainsKey(UniqueName))
            {
                PluginLog.Warning($"There is a layout named {UniqueName} already.");
                return false;
            }
            Layouts[UniqueName] = layout;
            return true;
        }

        /// <summary>
        /// Attempts to register previously constructed element for further usage. End user will be able to edit this element as they wish and results of the edit will be saved. Enabled elements are subject for immediate processing when the script is enabled.
        /// </summary>
        /// <param name="UniqueName">Internal unique (within current script) name of the element.</param>
        /// <param name="element">Element object.</param>
        /// <param name="overwrite">Whether to overwrite existing element with same name if it's present.</param>
        /// <returns>Whether element was successfully registered.</returns>
        public bool TryRegisterElement(string UniqueName, Element element, bool overwrite = false)
        {
            if (!overwrite && Layouts.ContainsKey(UniqueName))
            {
                PluginLog.Warning($"There is an element named {UniqueName} already.");
                return false;
            }
            Elements[UniqueName] = element;
            return true;
        }

        /// <summary>
        /// Attempts to register previously exported from plugin element for further usage. End user will be able to edit this element as they wish and results of the edit will be saved. Enabled elements are subject for immediate processing when the script is enabled.
        /// </summary>
        /// <param name="UniqueName">Internal unique (within current script) name of the element</param>
        /// <param name="ExportString">An exported element string.</param>
        /// <param name="element">Decoded element object.</param>
        /// <param name="overwrite">Whether to overwrite existing element with same name if it's present.</param>
        /// <returns>Whether element was successfully registered.</returns>
        public bool TryRegisterElementFromCode(string UniqueName, string ExportString, [NotNullWhen(true)] out Element? element, bool overwrite = false)
        {
            return ScriptingEngine.TryDecodeElement(ExportString, out element) && TryRegisterElement(UniqueName, element, overwrite);
        }

        /// <summary>
        /// Tries to get previously registered layout by name.
        /// </summary>
        /// <param name="name">Layout's internal name.</param>
        /// <param name="layout">Result.</param>
        /// <returns>Whether operation succeeded.</returns>
        public bool TryGetLayoutByName(string name, [NotNullWhen(true)] out Layout? layout)
        {
            return Layouts.TryGetValue(name, out layout);
        }

        /// <summary>
        /// Tries to get previously registered element by name.
        /// </summary>
        /// <param name="name">Element's internal name.</param>
        /// <param name="element">Result.</param>
        /// <returns>Whether operation succeeded.</returns>
        public bool TryGetElementByName(string name, [NotNullWhen(true)] out Element? element)
        {
            return Elements.TryGetValue(name, out element);
        }

        /// <summary>
        /// Unregisters previously registered layout.
        /// </summary>
        /// <param name="name">Layout name.</param>
        /// <returns>Whether operation succeeded.</returns>
        public bool TryUnregisterLayout(string name)
        {
            return Layouts.Remove(name);
        }

        /// <summary>
        /// Unregisters previously registered element.
        /// </summary>
        /// <param name="name">Element name.</param>
        /// <returns>Whether operation succeeded.</returns>
        public bool TryUnregisterElement(string name)
        {
            return Elements.Remove(name);
        }

        /// <summary>
        /// Returns a dictionary of currently registered layouts.
        /// </summary>
        /// <returns>Read only dictionary of currently registered layouts.</returns>
        public ReadOnlyDictionary<string, Layout> RegisteredLayouts()
        {
            return new ReadOnlyDictionary<string, Layout>(Layouts);
        }

        /// <summary>
        /// Returns a dictionary of currently registered elements.
        /// </summary>
        /// <returns>Read only dictionary of currently registered elements.</returns>
        public ReadOnlyDictionary<string, Element> RegisteredElements()
        {
            return new ReadOnlyDictionary<string, Element>(Elements);
        }

        /// <summary>
        /// Name of a script
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Optional author of a script, will be displayed in the list of scripts.
        /// </summary>
        public virtual string? Author { get; }

        /// <summary>
        /// Optional description of a script, will be displayed in the list of scripts.
        /// </summary>
        public virtual string? Description { get; }

        /// <summary>
        /// Optional single digit version of a script, will be displayed in the list of scripts.
        /// </summary>
        public virtual uint Version { get; }

        /// <summary>
        /// Valid territories where script will be executed. Specify an empty array if you want it to work in all territories. 
        /// </summary>
        public abstract HashSet<uint> ValidTerritories { get; }

        /// <summary>
        /// Indicates whether script is currently enabled and should be executed or not.
        /// </summary>
        public bool IsEnabled => ValidTerritories.Contains(Svc.ClientState.TerritoryType);

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
        /// <param name="Position">Positional data of map effect. It is not related to actual map coordinates.</param>
        /// <param name="Param1">First parameter of map effect.</param>
        /// <param name="Param2">Second parameter of map effect.</param>
        public virtual void OnMapEffect(uint Position, ushort Param1, ushort Param2) { }

        /// <summary>
        /// Will be called when a tether created between two game objects. This method will only be called if a script is enabled.
        /// </summary>
        /// <param name="source">Source object of pair.</param>
        /// <param name="target">Target object of pair.</param>
        public virtual void OnTetherCreate(GameObject source, GameObject target) { }

        /// <summary>
        /// Will be called when a previously created tether between two game objects removed. This method will only be called if a script is enabled.
        /// </summary>
        /// <param name="source">Source object of pair.</param>
        public virtual void OnTetherRemoval(GameObject source) { }

        /// <summary>
        /// Will be called when a VFX spawns on a certain game object. This method will only be called if a script is enabled.
        /// </summary>
        /// <param name="target">Object that is targeted by VFX</param>
        /// <param name="vfxPath">VFX game path</param>
        public virtual void OnVFXSpawn(GameObject target, string vfxPath) { }

        /// <summary>
        /// Will be called whenever plugin processes a message. These are the same messages which layout trigger system receives. This method will only be called if a script is enabled.
        /// </summary>
        /// <param name="Message"></param>
        public virtual void OnMessage(string Message) { }

        /// <summary>
        /// Will be called every framework update. You can execute general logic of your script here. 
        /// </summary>
        public virtual void OnUpdate() { }

        /// <summary>
        /// If you override this method, settings section will be added to your script. You can call ImGui methods in this function to draw configuration UI. Keep it simple.
        /// </summary>
        public virtual void OnSettingsDraw() { }

        public bool DoSettingsDraw => this.GetType().GetMethod(nameof(OnSettingsDraw))?.DeclaringType != typeof(SplatoonScript);
    }
}
