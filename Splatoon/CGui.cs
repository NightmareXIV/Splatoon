using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splatoon
{
    class CGui:IDisposable
    {
        readonly Splatoon p;
        public bool Open = false;

        public CGui(Splatoon p)
        {
            this.p = p;
            p._pi.UiBuilder.OnBuildUi += Draw;
        }

        public void Dispose()
        {
            p._pi.UiBuilder.OnBuildUi -= Draw;
        }

        void Draw()
        {
            if (!Open) return;
            if(ImGui.Begin("Splatoon configuration", ref Open))
            {
                
            }
        }
    }
}
