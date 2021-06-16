using System;
using System.Collections.Generic;
using System.IO;
using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace vaYolo.Model
{
    public class Roi
    {
        public Bitmap _Roi { get; set; }

        public uint ObjectClass { get; set; }

        public string ImagePath {get; set;}

        public static Avalonia.Media.Imaging.Bitmap? CropBitmap(System.Drawing.Bitmap bmp, Rect rc)
        {
            try
            {
                using (MemoryStream memory = new MemoryStream())
                {
                    var cropBmp = bmp.Clone(new System.Drawing.Rectangle((int)rc.X, (int)rc.Y, (int)rc.Width, (int)rc.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                    cropBmp.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
                    memory.Position = 0;
                    return new Avalonia.Media.Imaging.Bitmap(memory);
                }
            }
            catch (Exception exc)
            {
            }

            return null;
        }

        public Roi(VaRect rect, System.Drawing.Bitmap bmp, string imagePath) {
            _Roi = CropBitmap(bmp, rect.UnNormalized(new Size(bmp.Width, bmp.Height)));
            ObjectClass = rect.ObjectClass;
            ImagePath = imagePath;
        }

        public static List<Roi> LoadData(List<VaRect> rect, string imagePath) {
            List<Roi> result = new ();
            using (var img = new System.Drawing.Bitmap(imagePath)) {
                rect.ForEach((r)=> {
                    result.Add(new Roi(r, img, imagePath));
                });
            }

            return result;
        }
    }
}