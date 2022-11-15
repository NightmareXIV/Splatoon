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
        public string Name => System.IO.Path.GetFileName(Path);

        public InternalData(string path)
        {
            Path = path;
            var text = File.ReadAllText(path, Encoding.UTF8);
            
        }
    }
}
