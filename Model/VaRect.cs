using Avalonia;
using Avalonia.Media;
using System;
using System.IO;
using System.Collections.Generic;
using vaYolo.Ext;
using vaYolo.Helpers;

namespace vaYolo.Model
{
    public class VaRect
    {
        const double k = 16.0 / 416.0;

        const double k2 = 17.0 / 416.0;

        public static Size DefaultRectSize = new Size(k2, k2);

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

        public bool Undersized()
        {
            return (_Rect.Width < k) || (_Rect.Height < k);
        }

        public Rect UnNormalized(Size sz)
        {
            return _Rect.MulBySize(sz);

        }

        public void Draw(DrawingContext context, Size sz, bool selected = false)
        {
            if (Manager.MyPens == null)
                return;

            var pen = selected ? Manager.SelectedPen : Manager.MyPens[ObjectClass];
            var brush = selected ? Manager.SelectedBrush : Manager.MyBrushes[ObjectClass];

            if (Undersized())
            {
                pen = Manager.ErrorPen;
                brush = Manager.ErrorBrush;
            }


            var gs = GetGauges();
            var rc = UnNormalized(sz);
            context.DrawRectangle(pen, rc);
            context.FillRectangle(brush, gs[0].MulBySize(sz));
            context.FillRectangle(brush, gs[1].MulBySize(sz));
            context.DrawText(Manager.MyBrushes[ObjectClass], 
                rc.TopRight + Settings.Get().TextDelta, 
                Manager.MyFormattedText[ObjectClass]);
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
                VaRect.normalizedGaugeDelta = Settings.Get().GaugePoint.DivBySize(size);
                VaRect.normalizedTextDelta = Settings.Get().TextDelta.DivBySize(size);
            }
        }
        
        public static string? SaveData(string datapath, List<VaRect> rects)
        {
            if (datapath == null)
                return null;

            using (var streamWr = new StreamWriter(datapath))
            {
                var txtWriter = new TxtWriter(streamWr, " ");
                rects.ForEach((r) =>
                {
                    txtWriter.WriteField(r.ObjectClass.ToString());
                    txtWriter.WriteField(r._Rect.Center.X);
                    txtWriter.WriteField(r._Rect.Center.Y);
                    txtWriter.WriteField(r._Rect.Width);
                    txtWriter.WriteField(r._Rect.Height);
                    txtWriter.NextRecord();
                });
            }

            return datapath;
        }

        public static List<VaRect> LoadData(string datapath)
        {
            List<VaRect> rects = new();

            try {
                if ((datapath != null) || (File.Exists(datapath)))
                {
                    using (var streamWr = new StreamReader(datapath))
                    {
                        var txtReader = new TxtReader(streamWr, " ");

                        while (txtReader.NextRecord()) {
                            uint objClass = txtReader.ReadField<uint>();
                            var cX = txtReader.ReadField<double>();
                            var cY = txtReader.ReadField<double>();
                            var w = txtReader.ReadField<double>();
                            var h = txtReader.ReadField<double>();

                            rects.Add(new VaRect() {
                                _Rect = new Rect(
                                            new Point(cX - w / 2, cY - h / 2),
                                            new Point(cX + w / 2, cY + h / 2)),
                                ObjectClass = objClass
                            });
                        }                
                    }
                }
            }
            catch (Exception exc) {
                System.Diagnostics.Debug.WriteLine(exc.Message);                
            }

            return rects;
        }
    }
}