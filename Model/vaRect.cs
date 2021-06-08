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
                new Rect(_Rect.TopLeft, _Rect.TopLeft + Settings.gaugeDelta),
                new Rect(_Rect.BottomRight - Settings.gaugeDelta, _Rect.BottomRight)
            };
        }

        public void Draw(DrawingContext context, Size sz, bool selected = false)
        {
            var selIdx = VaManager.MyBrushes.Length - 1;
            var pen = VaManager.MyPens[selected ? selIdx : ObjectClass];
            var brush = VaManager.MyBrushes[selected ? selIdx : ObjectClass];
            context.DrawRectangle(pen, _Rect.MulBySize(sz));
            var gs = GetGauges();
            //context.FillRectangle(brush, gs[0].MulBySize(sz));
            //context.FillRectangle(brush, gs[1].MulBySize(sz));
            context.DrawText(VaManager.MyBrushes[ObjectClass], 
                             _Rect.TopRight + Settings.textDelta, 
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