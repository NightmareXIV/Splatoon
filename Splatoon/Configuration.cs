﻿using Dalamud.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Splatoon
{
    [Serializable]
    class Configuration : IPluginConfiguration
    {
        [NonSerialized] Splatoon plugin;
        [NonSerialized] SemaphoreSlim ZipSemaphore;

        public int Version { get; set; } = 1;

        public Dictionary<string, Layout> Layouts = new Dictionary<string, Layout>();
        public bool dumplog = false;
        public bool verboselog = false;
        public int segments = 100;
        public float maxdistance = 100;
        //public float maxcamY = 0.05f;
        public int ChlogReadVer = ChlogGui.ChlogVersion;
        public int lineSegments = 10;
        public bool UseHttpServer = false;
        public int port = 47774;
        public bool TetherOnFind = true;
        public bool TriggerAnyMessages = false;

        public void Initialize(Splatoon plugin)
        {
            this.plugin = plugin;
            ZipSemaphore = new SemaphoreSlim(1);
            Svc.PluginInterface.UiBuilder.OpenConfigUi += delegate
            {
                plugin.ConfigGui.Open = true;
            };
        }

        public void Save()
        {
            Svc.PluginInterface.SavePluginConfig(this);
        }

        public bool Backup()
        {
            if (!ZipSemaphore.Wait(0))
            {
                plugin.Log("Failed to create backup: previous backup did not completed yet. ", true);
                return false;
            }
            var cFile = Path.Combine(Svc.PluginInterface.GetPluginConfigDirectory(), "..", "Splatoon.json");
            var configStr = File.ReadAllText(cFile);
            var bkpFPath = Path.Combine(Svc.PluginInterface.GetPluginConfigDirectory(), "Backups");
            Directory.CreateDirectory(bkpFPath);
            var tempDir = Path.Combine(bkpFPath, "temp");
            Directory.CreateDirectory(tempDir);
            var tempFile = Path.Combine(tempDir, "Splatoon.json");
            var bkpFile = Path.Combine(bkpFPath, "Backup." + DateTimeOffset.Now.ToString("yyyy-MM-dd HH-mm-ss-fffffff") + ".zip");
            File.Copy(cFile, tempFile, true);
            Task.Run(new Action(delegate { 
                try
                {
                    ZipFile.CreateFromDirectory(tempDir, bkpFile, CompressionLevel.Optimal, false);
                    File.Delete(tempFile);
                    plugin.Log("Backup created: " + bkpFile);
                }
                catch (Exception e)
                {
                    plugin.Log("Failed to create backup: " + e.Message, true);
                    plugin.Log(e.StackTrace, true);
                }
                ZipSemaphore.Release();
            }));
            return true;
        }
    }
}
