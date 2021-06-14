using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Reactive;
using System.Threading.Tasks;
using vaYolo.Helpers;
using vaYolo.Models;


namespace vaYolo.ViewModels
{
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

        private static List<string> ValidExtensions = new() {
            ".png" };
                        // ".jpg", ".jpeg",
                        // ".PNG", ".JPG", ".JPEG"};

        public static List<string> ListImagesInFolder(string dir) 
        {
            List<string> result = new ();
            try {
                foreach (var ext in ValidExtensions)
                    result.AddRange(Directory.EnumerateFiles(dir,  "*" + ext).ToList());
            }
            catch (Exception ) {
            }

            return result;
        } 

        public static string GetDatapath(string imagePath) {
            return Path.ChangeExtension(imagePath, "txt"); 
        }        

        // private List<string> ListImagesInFolder(string? path) {
        //     try {
        //         var dir = Path.GetDirectoryName(path);
        //         var ext = Path.GetExtension(path);
        //         return Directory.EnumerateFiles(dir,  "*" + ext).ToList();
        //     }
        //     catch (Exception ) {

        //     }

        //     return new List<string>();
        // }          
    }
}