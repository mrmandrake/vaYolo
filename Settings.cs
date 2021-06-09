using System;
using System.Collections.Generic;
using Avalonia.Media;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;

namespace vaYolo
{
    public class Settings
    {
        public static int ImageDecodeWidth { get; set; } = 1920;

        public static Point GaugeDelta { get; set; } = new Point(12, 12);

        public static Point TextDelta { get; set; } = new Point(2, 6);

        public static Size NormalizedDefaultRectSize { get; set; }= new Size(0.023, 0.04);

        public static bool MaximizeAfterLoad { get; set; } = false;

        public static string SshServer { get; set; } = "10.171.76.76";

        public static uint SshPort { get; set; } = 22;

        public static string SshUsername { get; set; } = "manovella";

        public static string SshPassword { get; set; } = "";

        public static string SshRemoteYolo { get; set; } = "/home/manovella/darknet";
    }
}
