using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using System.Reactive;
using ReactiveUI;
using Avalonia.Media.Imaging;
using Darknet;
using Avalonia;
using static Darknet.YoloWrapper;
using vaYolo.Helpers;
using System.IO;

namespace vaYolo.ViewModels
{
    public class DetectViewModel : ReactiveObject
    {
        string title;

        string Title {
            get => title;
            set => this.RaiseAndSetIfChanged(ref title, value);
        }

        bbox_t[] boxes;

        public bbox_t[] Boxes
        {
            get => boxes;
            set => this.RaiseAndSetIfChanged(ref boxes, value);
        }

        float scoreThreshold;

        public float ScoreThreshold
        {
            get => scoreThreshold;
            set => this.RaiseAndSetIfChanged(ref scoreThreshold, value);
        }

        float nmsThreshold;

        public float NMSThreshold
        {
            get => nmsThreshold;
            set => this.RaiseAndSetIfChanged(ref nmsThreshold, value);
        }

        ObservableCollection<Rect> roi;

        public ObservableCollection<Rect> Roi {
            get => roi;
            set => this.RaiseAndSetIfChanged(ref roi, value);
        }
        
        string FolderPath;
        string ImgPath;

        private Bitmap? img;
        public Bitmap? Img
        {
            get => img;
            private set => this.RaiseAndSetIfChanged(ref img, value);
        }

        private string Folder { get; set; }

        private static Rect GetRect(bbox_t box) {
            return new Rect(box.x, box.y, box.w, box.h);
        }

        public DetectViewModel(string folder)
        {
            FolderPath = folder;       
        }

        public async void Detect(string imgPath) {
            Img = new Bitmap(imgPath);
            ImgPath = imgPath;

            Title = String.Format("Detecting on {0}, Thresholds: {1} {2} ......", 
                        ImgPath,                     
                        Settings.Get().ScoreThreshold, 
                        Settings.Get().NMSThreshold);

            await Task.Run(new Action(() => {
                var yolo = new YoloWrapper(
                        Util.ConfigPath(FolderPath), 
                        Util.WeightsPath(FolderPath), 0);
                Boxes = yolo.Detect(ImgPath);

                UpdateRoi();
                UpdateTitle();
            }));
        }

        private void UpdateRoi()
        {
            int[] indexes;
            OpenCvSharp.Dnn.CvDnn.NMSBoxes(
                from b in Boxes select new OpenCvSharp.Rect((int)b.x, (int)b.y, (int)b.w, (int)b.h),
                from b in Boxes select b.prob,
                ScoreThreshold, NMSThreshold,
                out indexes);

            Roi = new ObservableCollection<Rect>(from i in indexes select GetRect(Boxes[i]));
        }

        private void UpdateTitle()
        {
            Title = String.Format("{0} Objects on {1} Thresholds: {1} ,  {2}",
                        Roi.Count(), ImgPath,
                        ScoreThreshold, NMSThreshold);
        }
    }
}
