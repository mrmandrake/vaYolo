using Avalonia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vaYolo.Ext
{
    public static class PointExtension
    {
        public static Point DivBySize(this Point pt, Size sz)
        {
            return new Point(pt.X / sz.Width, pt.Y / sz.Height);
        }

        public static Point MulBySize(this Point pt, Size sz)
        {
            return new Point(pt.X * sz.Width, pt.Y * sz.Height);
        }

        public static Rect RectCenteredIn(this Point pt, double w, double h)
        {
            return new Rect(pt - new Point(w / 2, h / 2), new Size(w, h));
        }
    }
}
