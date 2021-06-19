using Avalonia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vaYolo.Ext
{
    public static class RectExtension
    {
        public static Rect MulBySize(this Rect r, Size sz)
        {
            return new Rect(r.TopLeft.MulBySize(sz), new Size(r.Width * sz.Width, r.Height * sz.Height));
        }
    }
}
