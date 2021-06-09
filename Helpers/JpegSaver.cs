using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.IO;

namespace vaYolo.Helpers
{
    public class JpegSaver
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

        public static void Convert(string imagePath)
        {
            Encoder myEncoder = Encoder.Quality;
            var encPars = new System.Drawing.Imaging.EncoderParameters(1);
            var encPar = new System.Drawing.Imaging.EncoderParameter(myEncoder, 100L);
            encPars.Param[0] = encPar;
            new System.Drawing.Bitmap(imagePath)
                    .Save(Path.Combine(Path.GetDirectoryName(imagePath), Path.GetFileNameWithoutExtension(imagePath) + ".jpg"),
                          GetEncoderInfo("image/jpeg"),
                          encPars);
        }

    }
}
