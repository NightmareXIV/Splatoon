using ECommons;
using Lumina.Excel.GeneratedSheets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splatoon.Modules
{
    internal static class Logger
    {
        static SimpleLogger currentLogger = null;

        internal static void BeginLogging()
        {
            Safe(delegate
            {
                EndLogging();
                var dirName = $"Logs {DateTimeOffset.Now:yyyy-MM-ddzzz}".Replace(":", "_");
                var directory = Path.Combine(Svc.PluginInterface.GetPluginConfigDirectory(), dirName);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                var fileName = $"{DateTimeOffset.Now:yyyy-MM-ddzzz HH.mm.ss} - {Svc.Data.GetExcelSheet<TerritoryType>().GetRow(Svc.ClientState.TerritoryType).ContentFinderCondition.Value.Name.ToString()}.txt".Replace(":", "_");
                currentLogger = new SimpleLogger(directory, fileName);
            });
        }

        internal static void OnTerritoryChanged()
        {
            EndLogging();
            if (P.Config.Logging)
            {
                if (Svc.Data.GetExcelSheet<TerritoryType>().GetRow(Svc.ClientState.TerritoryType).ContentFinderCondition.Value.Name.ToString() != String.Empty)
                {
                    BeginLogging();
                }
            }
        }

        internal static void Log(string message)
        {
            if(currentLogger != null)
            {
                var combatTime = Environment.TickCount64 - P.CombatStarted; ;
                currentLogger.Log($"[{(P.CombatStarted != 0?$"Combat: {((float)combatTime / 1000f):F1}s":"Not in combat")}] {message}");
            }
        }

        internal static void EndLogging()
        {
            if (currentLogger != null)
            {
                currentLogger.Dispose();
                currentLogger = null;
            }
        }
        }
}
