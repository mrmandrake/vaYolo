using Avalonia;
using Avalonia.Media;

namespace vaYolo.Models
{
    public class VaRect
    {
        public Rect _Rect { get; set; }
        public uint ObjectClass { get; set; }
        public Rect[] GetGauges()
        {
            return new Rect[]
            {
                new Rect(_Rect.TopLeft, _Rect.TopLeft + new Point(10, 10)),
                new Rect(_Rect.BottomRight - new Point(10, 10), _Rect.BottomRight)
            };
        }

        public void Draw(DrawingContext context, bool selected = false)
        {
            var selIdx = VaManager.MyBrushes.Length - 1;
            var pen = VaManager.MyPens[selected ? selIdx : ObjectClass];
            var brush = VaManager.MyBrushes[selected ? selIdx : ObjectClass];
            context.DrawRectangle(pen, _Rect);
            var gs = GetGauges();
            context.FillRectangle(brush, gs[0]);
            context.FillRectangle(brush, gs[1]);
            context.DrawText(VaManager.MyBrushes[ObjectClass], 
                _Rect.TopRight + new Point(2, -6), 
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
    }
}