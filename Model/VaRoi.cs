using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace vaYolo.Models
{
    public class VaRoi
    {
        public CroppedBitmap Roi { get; set; }

        public uint Class { get; set; }

        public string ImagePath {get; set;}

        public VaRoi(VaRect rect, Bitmap bmp, string imagePath) {
            Roi = new CroppedBitmap(bmp, PixelRect.FromRect(rect.UnNormalized(bmp.Size), 1.0));
            Class = rect.ObjectClass;
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