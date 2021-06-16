
using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using vaYolo.Helpers;
using vaYolo.Model;


namespace vaYolo.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {          
        public List<Rect> NormalizedRoi = new List<Rect>();
        public string? FolderPath { get; set; } = null;
        public string? TxtPath { get; set; }

        private string title = "vaYolo Labeling toolkit - mrMandrake";
        public string Title {
            get => title;
            set => this.RaiseAndSetIfChanged(ref title, value);
        }

        private string? _imagePath;
        public string? ImagePath
        {
            get { return _imagePath; }
            set
            {
                _imagePath = value;
                TxtPath = Util.TxtPath(value);
            }
        }

        private Bitmap? img;
        public Bitmap? Img
        {
            get => img;
            private set => this.RaiseAndSetIfChanged(ref img, value);
        }

        public bool isWin = false;
        public bool IsWin
        {
            get => isWin;
            private set => this.RaiseAndSetIfChanged(ref isWin, value);
        }

        public MainWindowViewModel()
        {
            LoadLogo();
            IsWin = OperatingSystem.IsWindows();                
        }

        private void LoadLogo()
        {
            try {
                var assetLoader = AvaloniaLocator.Current.GetService<IAssetLoader>();
                img = Bitmap.DecodeToWidth(assetLoader.Open(new Uri("avares://vaYolo/Assets/vayolo2.png")), 1440,
                            Avalonia.Visuals.Media.Imaging.BitmapInterpolationMode.LowQuality);
            }
            catch (Exception exc) {

            }
        }

        public void Add(Rect rc)
        {
            NormalizedRoi.Add(rc);
        }

        public async Task LoadImage(string? imagePath)
        {
            if (imagePath != null)
            {
                ImagePath = imagePath;
                FolderPath = Path.GetDirectoryName(imagePath);
                Title = "vaYolo - " + ImagePath;

                await Task.Run(
                    () => { 
                        try
                        {
                            Img = Bitmap.DecodeToWidth(
                                    File.OpenRead(ImagePath), 
                                    Settings.Get().ImageDecodeWidth,
                                    Avalonia.Visuals.Media.Imaging.BitmapInterpolationMode.LowQuality);
                        }
                        catch (Exception exc)
                        {
                        }
                    });

                //if (!VaUtil.IsJpeg(imagePath)) 
                //    ConvertImageToJpeg(imagePath);
            }
        }

        private void ConvertImageToJpeg(string imagePath) {
            try {
                JpegSaver.Convert(imagePath);
            }
            catch (Exception exc) {
                System.Diagnostics.Debug.WriteLine(exc.Message);                
            }
        }

        public string? SaveData(List<VaRect> rects) {
            return VaRect.SaveData(TxtPath, rects);
        }

        public List<VaRect> LoadData() {
            return VaRect.LoadData(TxtPath);
        }
    }
}
