using Dalamud.Hooking;
using Dalamud.Memory;
using Dalamud.Utility.Signatures;
using Splatoon.Modules;
using Splatoon.SplatoonScripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splatoon.Memory
{
    internal unsafe class DirectorUpdateProcessor
    {
        internal delegate long ProcessDirectorUpdate(long a1, long a2, DirectorUpdateCategory a3, uint a4, uint a5, int a6, int a7);
        [Signature("48 89 5C 24 ?? 57 48 83 EC 30 41 8B D9", DetourName = nameof(ProcessDirectorUpdateDetour), Fallibility = Fallibility.Fallible)]
        internal Hook<ProcessDirectorUpdate> ProcessDirectorUpdateHook = null;
        internal long ProcessDirectorUpdateDetour(long a1, long a2, DirectorUpdateCategory a3, uint a4, uint a5, int a6, int a7)
        {
            try
            {
                if (P.Config.Logging)
                {
                    var text = $"Director Update: {a3:X}, {a4:X8}, {a5:X8}, {a6:X8}, {a7:X8}";
                    Logger.Log(text);
                }
                ScriptingProcessor.OnDirectorUpdate(a3);
            }
            catch (Exception e)
            {
                e.Log();
            }
            return ProcessDirectorUpdateHook.Original(a1, a2, a3,a4,a5,a6,a7);
        }

        internal DirectorUpdateProcessor()
        {
            SignatureHelper.Initialise(this);
            this.Enable();
        }

        internal void Enable()
        {
            if (!ProcessDirectorUpdateHook.IsEnabled) ProcessDirectorUpdateHook.Enable();
        }

        internal void Disable()
        {
            if (ProcessDirectorUpdateHook.IsEnabled) ProcessDirectorUpdateHook.Disable();
        }

        public void Dispose()
        {
            this.Disable();
            ProcessDirectorUpdateHook.Dispose();
        }
    }
}
