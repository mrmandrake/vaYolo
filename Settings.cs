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
        public int ImageDecodeWidth { get; set; } = 1920;

        public Point GaugeDelta { get; set; } = new Point(12, 12);

        public Point TextDelta { get; set; } = new Point(2, 6);

        public Size NormalizedDefaultRectSize { get; set; }= new Size(0.023, 0.04);

        public bool MaximizeAfterLoad { get; set; } = false;

        public string SshServer { get; set; } = "10.171.76.76";

        public uint SshPort { get; set; } = 22;

        public string SshUsername { get; set; } = "manovella";

        public string SshPassword { get; set; } = "";

        public string SshRemoteYolo { get; set; } = "/home/manovella/darknet";

        private static Settings? instance = null;

        public static Settings? Get()
        {
            return instance;
        }

        public static void Load(string folder)
        {
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

        private static Settings? CreateNew(string path)
        {
            Settings? data = null;
            try
            {
                var newdata = new Settings()
                {
                    ImageDecodeWidth = 1920,
                    GaugeDelta = new Point(12, 12),
                    TextDelta = new Point(2, 6),
                    NormalizedDefaultRectSize = new Size(0.023, 0.04),
                    MaximizeAfterLoad = false,
                    SshServer = "10.171.69.69",
                    SshPort = 22,
                    SshUsername = "manovella",
                    SshPassword = "",
                    SshRemoteYolo = "/home/manovella/darknet"
                };


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
