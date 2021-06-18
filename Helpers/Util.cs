using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace vaYolo.Helpers
{
    public class Util
    {
        public static bool IsJpeg(string imagePath)
        {
            return (Path.GetExtension(imagePath).ToLower() == "jpg") ||
                   (Path.GetExtension(imagePath).ToLower() == "jpeg");
        }

        public static string? AddSuffix(string filename, string suffix = "_res")
        {
            string? fDir = Path.GetDirectoryName(filename);
            string fName = Path.GetFileNameWithoutExtension(filename);
            string fExt = Path.GetExtension(filename);
            return (fDir != null) ? Path.Combine(fDir, String.Concat(fName, suffix, fExt)) : null;
        }

        private static List<string> ValidExtensions = new()
        {
            ".png",
            ".jpg"
        };
        // ".jpg", ".jpeg",
        // ".PNG", ".JPG", ".JPEG"};

        public static List<string> ListImagesInFolder(string dir)
        {
            List<string> result = new();
            try
            {
                foreach (var ext in ValidExtensions)
                    result.AddRange(Directory.EnumerateFiles(dir, "*" + ext).ToList());
            }
            catch (Exception)
            {
            }

            return result;
        }

        private static string PathPrefix(string folder) {
            return Path.Combine(folder, new DirectoryInfo(folder).Name.Replace("vayolo.", ""));
        }

        public static string TrainListPath(string folder) {
            return PathPrefix(folder) + "_train.txt";
        }

        public static string ValidListPath(string folder) {
            return PathPrefix(folder) + "_valid.txt";
        }

        public static string DataPath(string folder)
        {
            return PathPrefix(folder) + ".data";
        }

        public static string NamesPath(string folder) {
            return PathPrefix(folder) + ".names";
        }

        public static string ConfigPath(string folder) {
            return PathPrefix(folder) + ".cfg";
        }

        public static string TxtPath(string imagePath)
        {
            return Path.ChangeExtension(imagePath, ".txt");
        }

        public static string SettingsPath(string folder)
        {
            return Path.Combine(folder, "vayolo.xml");
        }

        public static string ChartPath(string folder)
        {
            return Path.Combine(folder, "chart.png");
        }

        public static string GetTemplatePath(string name)
        {
            var appDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return Path.Combine(appDir, "templates", name);
        }

        public static List<string> ListLabeledInFolder(string dir)
        {
            List<string> result = new();
            try
            {
                var txt = Directory.EnumerateFiles(dir, "*.txt").ToList();
                txt.ForEach((f) =>
                {
                    ValidExtensions.ForEach((ext) =>
                    {
                        var p = Path.ChangeExtension(f, ext);
                        if (File.Exists(p))
                            result.Add(p);
                    });
                });
            }
            catch (Exception)
            {
            }

            return result;
        }

        public static string? GetNextPath(string path)
        {
            var images = Util.ListImagesInFolder(Path.GetDirectoryName(path));
            var idx = images.IndexOf(path);
            if (idx <= 0)
                return null;                

            return images[--idx];
        }

        public static string? GetPrevPath(string path)
        {
             var images = Util.ListImagesInFolder(Path.GetDirectoryName(path));
            var idx = images.IndexOf(path);
            if (idx == images.Count - 1) 
                return null;

            return images[++idx];
        }                
    }
}