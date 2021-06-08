using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

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
            manager.Render(context);
        }

        public void Add(Point pt)
        {
            manager.Add(pt);
            EditStartPoint = null;
            InvalidateVisual();
        }

        public void Set(Point pt) 
        {
            manager.Set(pt, CurrentObjectClass);
            EditStartPoint = pt;
            InvalidateVisual();
        }

        public void Del(Point pt)
        {
            manager.Del(pt);
            InvalidateVisual();
        }

        public void Move(Point pt)
        {
            if (!EditStartPoint.HasValue)
                return;
                
            manager.Move(pt, EditStartPoint.Value, CurrentObjectClass);
            if (manager.IsEditMode())
                EditStartPoint = pt;

            InvalidateVisual();
        }
    }
}