using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.IO;

namespace vaYolo.Helpers
{
    public class ImgSaver
    {
        private static System.Drawing.Imaging.ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            System.Drawing.Imaging.ImageCodecInfo[] encoders;
            encoders = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }

        public static void ConvertToJpg(string sourcePath, string destFolder)
        {
            var jpegPath = Path.Combine(destFolder, Path.GetFileNameWithoutExtension(sourcePath) + ".jpg");

            if (!File.Exists(destFolder ))
            {
                Encoder myEncoder = Encoder.Quality;
                var encPars = new System.Drawing.Imaging.EncoderParameters(1);
                var encPar = new System.Drawing.Imaging.EncoderParameter(myEncoder, 100L);
                encPars.Param[0] = encPar;
                var encInfo = GetEncoderInfo("image/jpeg");
                new System.Drawing.Bitmap(sourcePath).Save(jpegPath, encInfo, encPars);
            }
        }

        public static void ConvertToPng(string sourcePath, string destFolder)
        {
            var pngPath = Path.Combine(destFolder, Path.GetFileNameWithoutExtension(sourcePath) + ".png");
            if (!File.Exists(pngPath))
                new System.Drawing.Bitmap(sourcePath).Save(pngPath, GetEncoderInfo("image/png"), null);
        }        
    }
}
