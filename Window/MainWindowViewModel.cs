
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Dialogs;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive;
using System.Threading.Tasks;
using vaYolo.Helpers;
using vaYolo.Models;


namespace vaYolo.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {          
        public List<Rect> NormalizedRoi = new List<Rect>();
        public string? FolderPath { get; set; } = null;
        public string? DataPath { get; set; }

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
                DataPath = VaUtil.GetDatapath(value);
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
            var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
            img = Bitmap.DecodeToHeight(assets.Open(new Uri("avares://vaYolo/Assets/vayolo.png")), 320,
                        Avalonia.Visuals.Media.Imaging.BitmapInterpolationMode.LowQuality);

            IsWin = OperatingSystem.IsWindows();                
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
            return VaRect.SaveData(DataPath, rects);
        }

        public List<VaRect> LoadData() {
            return VaRect.LoadData(DataPath);
        }
    }
}
