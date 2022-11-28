using ECommons.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splatoon.SplatoonScripting
{
    [Serializable]
    public class Configuration : IEzConfig
    {
        public Dictionary<string, object> Objects = new();
    }
}
