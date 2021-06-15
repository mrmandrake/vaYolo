using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace vaYolo.Helpers
{
    public static class EnumerableStringExtension
    {
        public static string Save(this IEnumerable<string> en, string path)
        {
            File.WriteAllLines(path, en);
            return path;
        }
    }

    public class VaUtil
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

        public static string GetDatapath(string folder)
        {
            var name = new DirectoryInfo(folder).Name;
            return Path.ChangeExtension(folder, "txt");
        }

        public static string GetTrainListPath(string folder)
        {
            var name = new DirectoryInfo(folder).Name;
            return Path.Combine(folder, name + "_train.txt");
        }

        public static string GetValidListPath(string folder)
        {
            var name = new DirectoryInfo(folder).Name;
            return Path.Combine(folder, name + "_valid.txt");
        }

        public static string GetDataPath(string folder)
        {
            var name = new DirectoryInfo(folder).Name;
            return Path.Combine(folder, new DirectoryInfo(folder).Name, ".data");
        }

        public static string GetNamesPath(string folder)
        {
            var name = new DirectoryInfo(folder).Name;
            return Path.Combine(folder, name + ".names");
        }

        public static string GetCfgPath(string folder)
        {
            return Path.Combine(folder, new DirectoryInfo(folder).Name, ".cfg");
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
    }
}