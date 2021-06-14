using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace vaYolo
{
    public class VaImage : Image
    {
        public uint CurrentObjectClass {get; set;} = 0;        

        private Point? EditStartPoint;

        private VaManager manager = VaManager.Instance;

        public VaImage() {
        }

        public void Reset() {
            manager.Reset();
        }

        public override void Render(DrawingContext context)
        {
            base.Render(context);
            manager.Render(context, DesiredSize);
        }

        public void Add()
        {
            manager.Add();
            EditStartPoint = null;
            InvalidateVisual();
        }

        public void Set(Point pt)
        {
            var normPt = pt.DivBySize(DesiredSize);
            manager.Set(normPt, CurrentObjectClass);
            EditStartPoint = normPt;
            InvalidateVisual();
        }

        public void Del(Point pt)
        {
            var normPt = pt.DivBySize(DesiredSize);
            manager.Del(normPt);
            InvalidateVisual();
        }

        public void Move(Point pt)
        {
            if (!EditStartPoint.HasValue)
                return;

            var normPt = pt.DivBySize(DesiredSize);
            manager.Move(normPt, EditStartPoint.Value, CurrentObjectClass);
            if (manager.IsEditMode())
                EditStartPoint = normPt;

            InvalidateVisual();
        }
    }
}