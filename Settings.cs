﻿using System;
using System.Collections.Generic;
using Avalonia.Media;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using System.IO;
using System.Xml.Serialization;
using Avalonia.Controls;
using vaYolo.Model;
using vaYolo.Helpers;

namespace vaYolo
{
    public class Settings
    {
        public int ImageDecodeWidth { get; set; }

        public int GaugeSize { get; set; }

        public int OffsetTextX { get; set; }
        
        public int OffsetTextY { get; set; }

        public bool MaximizeAfterLoad { get; set; }

        public string? SshServer { get; set; }

        public int SshPort { get; set; }

        public string? SshUsername { get; set; }

        public string? SshPassword { get; set; }

        public string SshRemoteDarknet { get; set; } = "/home/manovella/darknet";

        public string SshRemote { get; set; } = "/tmp";

        public int NetworkWidth { get; set; } = 416;

        public int NetworkHeight { get; set; } = 416;

        public int BatchSize { get; set; } = 64;

        public int MaxBatches { get; set; } = 6000;

        public int Subdivision { get; set; } = 16;

        private static Settings? instance = null;

        public static Settings? Get()
        {
           if (instance == null)
                instance = Default();

            return instance;
        }

        public VaSetup GetSetup()
        {
            return new VaSetup()
            {
                batch = BatchSize,
                subdivision = Subdivision,
                max_batches = MaxBatches,
                network_size_height = NetworkHeight,
                network_size_width = NetworkWidth,
                classes = VaNames.Classes.Count()
            };
        }

        public Point TextDelta {
            get {
                return new Point(OffsetTextX, OffsetTextY);
            }
        }

        public Point GaugePoint {
            get {
                return new Point(GaugeSize, GaugeSize);
            }
        }        

        public static void Load(string? folder)
        {
            if (folder == null)
                return;

            string path = VaUtil.SettingsPath(folder);
            Settings? result = null;
            if (File.Exists(path))
                result = LoadExisting(path);

            if (result == null)
            {
                if (File.Exists(path))
                    File.Copy(path, path + ".old");

                result = CreateNew(path);
            }

            instance = result;
        }

        public void Save(string folder)
        {
            Save(instance, VaUtil.SettingsPath(folder));
        }

        private static Settings Default()
        {
            return new Settings()
            {
                ImageDecodeWidth = 1920,
                GaugeSize = 12,
                OffsetTextX = 2,
                OffsetTextY = 6,
                MaximizeAfterLoad = false,
                SshServer = "127.0.0.1",
                SshPort = 65022,
                SshUsername = "manovella",
                SshPassword = "",
                SshRemoteDarknet = "/home/manovella/darknet",
                SshRemote = "/tmp"
            };
        }

        private static Settings? CreateNew(string path)
        {
            Settings? data = null;
            try
            {
                var newdata = Default();
                Save(newdata, path);
                return newdata;
            }
            catch (Exception exc)
            {
                System.Diagnostics.Debug.WriteLine(exc.Message);
            }

            return data;
        }

        private static Settings? LoadExisting(string path)
        {
            Settings? data = null;
            try
            {
                FileStream fStream = new FileStream(path, FileMode.Open);
                XmlSerializer xmlReader = new XmlSerializer(typeof(Settings));
                data = (Settings)xmlReader.Deserialize(fStream);
                fStream.Close();
            }
            catch (Exception exc)
            {
                System.Diagnostics.Debug.WriteLine(exc.Message);
                data = null;
            }

            return data;
        }

        private static void Save(Settings settings, string path)
        {
            FileStream fStream = new FileStream(path, FileMode.Create);
            XmlSerializer xmlReader = new XmlSerializer(settings.GetType());
            xmlReader.Serialize(fStream, settings);
            fStream.Close();
        }
    }
}
