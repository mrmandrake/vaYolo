using Avalonia;
using Avalonia.Media;

namespace vaYolo.Models
{
    public class VaRect
    {
        public static Point normalizedGaugeDelta;

        public static Point normalizedTextDelta;

        public Rect _Rect { get; set; }
        public uint ObjectClass { get; set; }

        public Rect[] GetGauges()
        {
            return new Rect[]
            {
                new Rect(_Rect.TopLeft, _Rect.TopLeft + normalizedGaugeDelta),
                new Rect(_Rect.BottomRight - normalizedGaugeDelta, _Rect.BottomRight)
            };
        }

        public void Draw(DrawingContext context, Size sz, bool selected = false)
        {
            if (VaManager.MyPens == null)
                return;

            var selIdx = VaManager.MyBrushes.Length - 1;
            var pen = VaManager.MyPens[selected ? selIdx : ObjectClass];
            var brush = VaManager.MyBrushes[selected ? selIdx : ObjectClass];
            var gs = GetGauges();
            var rc = _Rect.MulBySize(sz);
            context.DrawRectangle(pen, rc);
            context.FillRectangle(brush, gs[0].MulBySize(sz));
            context.FillRectangle(brush, gs[1].MulBySize(sz));
            context.DrawText(VaManager.MyBrushes[ObjectClass], 
                rc.TopRight + Settings.Get().TextDelta, 
                VaManager.MyFormattedText[ObjectClass]);
        }

        public bool IsInLowerGauge(Point pt)
        {
            if (GetGauges()[0].Contains(pt))
                return true;

            return false;
        }

        public bool IsInUpperGauge(Point pt)
        {
            if (GetGauges()[1].Contains(pt))
                return true;

            return false;
        }

        internal static void Update(Size size)
        {
            if (Settings.Get() != null)
            {
                VaRect.normalizedGaugeDelta = Settings.Get().GaugeDelta.DivBySize(size);
                VaRect.normalizedTextDelta = Settings.Get().TextDelta.DivBySize(size);
            }
        }
    }
}