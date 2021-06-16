using System;
using System.Collections.Generic;
using Avalonia.Media;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using System.IO;
using System.Xml.Serialization;
using Avalonia.Controls;

namespace vaYolo
{
    public class Settings
    {
        public int ImageDecodeWidth { get; set; }

        public int GaugeSize { get; set; }

        public int OffsetTextX { get; set; }
        
        public int OffsetTextY { get; set; }

        public double NormalizedDefaultRectWidth { get; set; }
        
        public double NormalizedDefaultRectHeight { get; set; }

        public bool MaximizeAfterLoad { get; set; }

        public string? SshServer { get; set; }

        public uint SshPort { get; set; }

        public string? SshUsername { get; set; }

        public string? SshPassword { get; set; }

        public string SshRemoteDarknet { get; set; } = "/home/manovella/darknet";

        public string SshRemote { get; set; } = "/tmp";

        private static Settings? instance = null;

        public static Settings? Get()
        {
           if (instance == null)
                instance = Default();

            return instance;
        }

        public Size DefaultRectSize {
            get {
                return new Size(NormalizedDefaultRectWidth, NormalizedDefaultRectHeight);
            }
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
                
            string path = Path.Combine(folder, "vayolo.xml");
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

        public static void Save(string folder)
        {
            try
            {
                string path = Path.Combine(folder, "vayolo.xml");
                FileStream fStream = new FileStream(path, FileMode.Create);
                XmlSerializer xmlReader = new XmlSerializer(typeof(Settings));
                xmlReader.Serialize(fStream, instance);
                fStream.Close();
            }
            catch (Exception exc)
            {
                System.Diagnostics.Debug.WriteLine(exc.Message);
            }
        }

        private static Settings Default()
        {
            return new Settings()
            {
                ImageDecodeWidth = 1920,
                GaugeSize = 12,
                OffsetTextX = 2,
                OffsetTextY = 6,
                NormalizedDefaultRectWidth = 0.023,
                NormalizedDefaultRectHeight = 0.04,
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
                FileStream fStream = new FileStream(path, FileMode.Create);
                XmlSerializer xmlReader = new XmlSerializer(newdata.GetType());
                xmlReader.Serialize(fStream, newdata);
                fStream.Close();
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
    }
}
