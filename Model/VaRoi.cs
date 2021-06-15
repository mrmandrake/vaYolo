using System;
using System.Collections.Generic;
using System.IO;
using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace vaYolo.Models
{
    public class VaRoi
    {
        public Bitmap Roi { get; set; }

        public uint ObjectClass { get; set; }

        public string ImagePath {get; set;}

        public static Avalonia.Media.Imaging.Bitmap? CropBitmap(System.Drawing.Bitmap bmp, Rect rc)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                var cropBmp = bmp.Clone(new System.Drawing.Rectangle((int)rc.X, (int)rc.Y, (int)rc.Width, (int)rc.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                cropBmp.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
                memory.Position = 0;
                return new Avalonia.Media.Imaging.Bitmap(memory);
            }
        }

        public VaRoi(VaRect rect, Bitmap bmp, string imagePath) {
            using (var img = new System.Drawing.Bitmap(imagePath))
            {
                Roi = CropBitmap(img, rect.UnNormalized(bmp.Size));
            }

            // Roi = new CroppedBitmap(bmp, PixelRect.FromRect(rect.UnNormalized(bmp.Size), 1.0));
            ObjectClass = rect.ObjectClass;
            ImagePath = imagePath;
        }

        public static List<VaRoi> LoadData(List<VaRect> rect, string imagePath) {
            List<VaRoi> result = new ();
            using (Bitmap img = new Bitmap(imagePath)) {
                rect.ForEach((r)=> {
                    result.Add(new VaRoi(r, img, imagePath));
                });
            }

            return result;
        }
    }
}