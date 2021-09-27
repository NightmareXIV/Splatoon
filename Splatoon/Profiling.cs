using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splatoon
{
    class Profiling
    {
        Splatoon p;
        public bool Enabled = false;
        public StopwatchWrapper MainTickDequeue = new StopwatchWrapper();
        public StopwatchWrapper MainTickPrepare = new StopwatchWrapper();
        public StopwatchWrapper MainTickFind = new StopwatchWrapper();
        public StopwatchWrapper MainTickCalcPresets = new StopwatchWrapper();
        public StopwatchWrapper MainTickCalcDynamic = new StopwatchWrapper();
        public StopwatchWrapper MainTickActorTableScan = new StopwatchWrapper();
        public StopwatchWrapper MainTick = new StopwatchWrapper();
        public StopwatchWrapper Gui = new StopwatchWrapper();
        public StopwatchWrapper GuiLines = new StopwatchWrapper();

        internal Profiling(Splatoon p)
        {
            this.p = p;
        }

        public class StopwatchWrapper
        {
            public Stopwatch stopwatch;
            long time;
            long ticks;
            ulong curTick;

            public StopwatchWrapper()
            {
                stopwatch = new Stopwatch();
            }

            public void Reset()
            {
                time = 0;
                ticks = 0;
                curTick = 0;
            }

            public void StartTick()
            {
                if (curTick != Svc.PluginInterface.UiBuilder.FrameCount)
                {
                    ticks++;
                    curTick = Svc.PluginInterface.UiBuilder.FrameCount;
                }
                stopwatch.Restart();
            }

            public void StopTick()
            {
                stopwatch.Stop();
                time += stopwatch.ElapsedTicks;
            }

            public long GetTotalTicks()
            {
                return ticks;
            }

            public long GetTotalTime()
            {
                return time;
            }

            public float GetAverageMSPT()
            {
                return ((float)time / (float)ticks) / (float)Stopwatch.Frequency * 1000f;
            }

            public float GetAverageTicks()
            {
                return (float)time / (float)ticks;
            }
        }
    }
}
