﻿using ECommons.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splatoon.Gui
{
    internal static class RapidImport
    {
        internal static bool RapidImportEnabled = false;
        internal static void Draw() 
        {
            if(ImGui.Checkbox("Enable Rapid Import", ref RapidImportEnabled))
            {
                ImGui.SetClipboardText("");
            }
            ImGuiEx.TextWrapped("Import multiple presets with ease by simply copying them. Splatoon will read your clipboard and attempt to import whatever you copy. Your clipboard will be cleared upon enabling.");
            if (RapidImportEnabled)
            {
                var text = ImGui.GetClipboardText();
                if(text != "")
                {
                    if (CGui.ImportFromClipboard())
                    {
                        TryNotify("Import success");
                    }
                    else
                    {
                        TryNotify("Import failed");
                    }
                    ImGui.SetClipboardText("");
                }
            }
        }

        static void TryNotify(string s)
        {
            if(DalamudReflector.TryGetDalamudPlugin("NotificationMaster", out var instance, true, true))
            {
                Safe(delegate
                {
                    instance.GetType().Assembly.GetType("NotificationMaster.TrayIconManager", true).GetMethod("ShowToast").Invoke(null, new object[] { "Splatoon", s });
                });
            }
        }
    }
}
