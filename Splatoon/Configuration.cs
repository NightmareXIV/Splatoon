using Dalamud.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splatoon
{
    [Serializable]
    class Configuration : IPluginConfiguration
    {
        [NonSerialized]
        Splatoon plugin;

        public int Version { get; set; } = 1;
        public Dictionary<string, Layout> Layouts = new Dictionary<string, Layout>();

        public void Initialize(Splatoon plugin)
        {
            this.plugin = plugin;
        }

        public void Save()
        {
            plugin._pi.SavePluginConfig(this);
        }
    }
}
