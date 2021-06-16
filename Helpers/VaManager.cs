using Avalonia;
using Avalonia.Media;
using DynamicData;
using System;
using System.Collections.Generic;
using System.Linq;
using vaYolo.Model;

namespace vaYolo
{
    public partial class VaManager
    {
        public enum EditEnum {
            None,
            EditUpperGauge,
            EditLowerGauge,
        };

        private static readonly Lazy<VaManager> instance = new (() => new VaManager());

        public static VaManager Instance { get { return instance.Value; } }

        private VaManager() {            
        }

        public bool IsEditMode() {
            return EditMode == EditEnum.EditLowerGauge ||
                EditMode == EditEnum.EditUpperGauge;
        }

        public EditEnum EditMode = EditEnum.None;

        public static Pen[] MyPens;

        public static FormattedText[] MyFormattedText; 

        public List<VaRect> Rects = new ();

        private VaRect? SelectedRect;
      
        private VaRect? EditingRect;

        public void Reset() {
            EditingRect = null;
            SelectedRect = null;
            Rects.Clear();

            List<Pen> pens = new() { };
            List<FormattedText> texts = new() { };
            for (int i = 0; i < MyBrushes.Length; i++)
            {
                pens.Add(new List<Pen>() { new Pen(MyBrushes[i]) });
                texts.Add(new List<FormattedText>() {
                    new FormattedText()
                    {
                        Text = i.ToString(),
                        FontSize = 14,
                        Typeface = new Typeface("arial")
                    }
                });
            }

            MyPens = pens.ToArray();
            MyFormattedText = texts.ToArray();
        }

        public void Render(DrawingContext context, Size size) {
            VaRect.Update(size);

            if (SelectedRect != null)
                SelectedRect.Draw(context, size, true);

            if (EditingRect != null)
                EditingRect.Draw(context, size);

            Rects.ForEach((r) => r.Draw(context, size));
        }

        public void Add() {
            if (IsEditMode())
            {
                if (EditingRect != null)
                    Rects.Add(EditingRect);
            }
            else
                if (SelectedRect != null)
                    Rects.Add(SelectedRect);

            SelectedRect = null;
            EditingRect = null;
            EditMode = EditEnum.None;
        }

        public void Set(Point pt, uint objClass) {
            foreach (var r in Rects)
            {
                if (r.IsInUpperGauge(pt))
                {
                    EditMode = EditEnum.EditUpperGauge;
                    SelectedRect = r;
                    break;
                }

                if (r.IsInLowerGauge(pt))
                {
                    EditMode = EditEnum.EditLowerGauge;
                    SelectedRect = r;
                    break;
                }
            }

            var selRects = (from r in Rects where r._Rect.Contains(pt) select r).ToList();

            if (IsEditMode())
            {
                EditingRect = SelectedRect != null ? SelectedRect : GetNewRect(pt, VaRect.DefaultRectSize, objClass);
                SelectedRect = null;                
            }
            else
                if (Settings.Get() != null) {
                    SelectedRect = selRects.Count > 0 ? selRects.First() : GetNewRect(pt, VaRect.DefaultRectSize, objClass);
                    EditingRect = null;
                }
            selRects.ForEach(r => { Rects.Remove(r); });         
        }

        public void Del(Point pt)
        {
            var selRects = (from r in Rects where r._Rect.Contains(pt) select r).ToList();
            selRects.ForEach(r => { Rects.Remove(r); });
        }

        public void Move(Point pt, Point start, uint objClass) {
            switch (EditMode) {
                case EditEnum.EditLowerGauge:
                    if (EditingRect == null)
                        throw new Exception("Editing Rect Exception");

                    EditingRect = new VaRect() {
                        ObjectClass = objClass,
                        _Rect = new Rect(EditingRect._Rect.TopLeft + (pt - start), EditingRect._Rect.BottomRight)
                    };
                break;

                case EditEnum.EditUpperGauge:
                    if (EditingRect == null)
                        throw new Exception("Editing Rect Exception");

                    EditingRect = new VaRect() {
                        ObjectClass = objClass,
                        _Rect = new Rect(EditingRect._Rect.TopLeft , EditingRect._Rect.BottomRight + (pt - start))
                    };
                break;
                default:
                    if (SelectedRect == null)
                        throw new Exception("Moving selected Rect Exception");

                    SelectedRect = GetNewRect(pt, new Size(SelectedRect._Rect.Width, SelectedRect._Rect.Height), SelectedRect.ObjectClass);
                    EditingRect = null;
                break;
            }
        }

        public static VaRect GetNewRect(Point pt, Size sz, uint objClass = 0)
        {
            return new VaRect() {
                ObjectClass = objClass,
                _Rect = pt.RectCenteredIn(sz.Width, sz.Height)
             };
        }
    }
}