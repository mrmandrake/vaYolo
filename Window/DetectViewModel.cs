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

        public DetectViewModel(string imgPath, string folder)
        {
            Img = new Bitmap(imgPath); 
            ImgPath = imgPath;
            FolderPath = folder;       
        }

        public async void Detect() {
            Title = String.Format("Detecting on {0}, Thresholds: {1} {2} ......", 
                        ImgPath,                     
                        Settings.Get().ScoreThreshold, 
                        Settings.Get().NMSThreshold);

            await Task.Run(new Action(() => {
                var yolo = new YoloWrapper(
                        Util.ConfigPath(FolderPath), 
                        Util.WeightsPath(FolderPath), 0);
                var boxes = yolo.Detect(ImgPath);  
                int[] indexes;
                OpenCvSharp.Dnn.CvDnn.NMSBoxes(
                    from b in boxes select new OpenCvSharp.Rect((int)b.x, (int)b.y, (int)b.w, (int)b.h), 
                    from b in boxes select b.prob, 
                    Settings.Get().ScoreThreshold, 
                    Settings.Get().NMSThreshold, 
                    out indexes);

                Roi = new ObservableCollection<Rect>(from i in indexes select GetRect(boxes[i]));

                Title = String.Format("{0} Objects on {1} Thresholds: {1} ,  {2}", 
                        Roi.Count(),
                        ImgPath,                     
                        Settings.Get().ScoreThreshold, 
                        Settings.Get().NMSThreshold);
            }));
        }
    }
}
