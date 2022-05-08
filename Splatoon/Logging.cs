global using static Splatoon.Logging;
using Dalamud.Interface.Internal.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splatoon
{
    internal static class Logging
    {
        public static void Notify(string notification, NotificationType type = NotificationType.Info, uint delay = 3000)
        {
            Svc.PluginInterface.UiBuilder.AddNotification(notification, "Splatoon", type, delay);
        }

        public static void LogErrorAndNotify(Exception exception, string additionalDescription = null, uint notifyTime = 3000)
        {
            PluginLog.Error($"Error occurred during Splatoon plugin execution{(additionalDescription == null ? "" : $": {additionalDescription}")}");
            PluginLog.Error($"{exception.Message}\n{exception.StackTrace ?? "No stack trace smh"}");
            Notify(additionalDescription ?? exception.Message, NotificationType.Error, notifyTime);
        }

        public static void LogErrorAndNotify(string additionalDescription)
        {
            PluginLog.Error($"Error occurred during Splatoon plugin execution");
            Notify(additionalDescription);
        }
    }
}
