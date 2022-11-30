using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable enable
namespace Splatoon.SplatoonScripting
{
    public class InternalData
    {
        public string Path { get; internal set; }
        public string Namespace { get; internal set; }
        public string Name { get; internal set; }
        public string GUID { get; } = "Script" + Guid.NewGuid().ToString();
        public string FullName { get; internal set; }

        internal bool ConfigOpen = false;

        internal SplatoonScript Script;

        public InternalData(string path, SplatoonScript instance)
        {
            Script = instance;
            Path = path;
            Namespace = instance.GetType().Namespace ?? "Default";
            Name = instance.GetType().Name;
            FullName = $"{Namespace}@{Name}";
            PluginLog.Information($"Script {FullName} ready.");
        }

        public bool SettingsPresent => Script.GetType().GetMethod("OnSettingsDraw")!.DeclaringType != typeof(SplatoonScript);
    }
}
