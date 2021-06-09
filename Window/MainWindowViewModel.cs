
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
using vaYolo.Models;

namespace vaYolo.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {          
        public List<Rect> NormalizedRoi = new List<Rect>();
        public string? FolderPath { get; set; }
        public string? DataPath { get; set; }
        private string? _imagePath;
        public string? ImagePath
        {
            get { return _imagePath; }
            set
            {
                _imagePath = value;
                DataPath = Path.ChangeExtension(value, "txt");
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
                await Task.Run(
                    () =>
                    {
                        Img = Bitmap.DecodeToWidth(File.OpenRead(ImagePath), 1440,
                        Avalonia.Visuals.Media.Imaging.BitmapInterpolationMode.LowQuality);
                    });
            }

        }

        private string? AddSuffix(string filename, string suffix = "_res")
        {
            string? fDir = Path.GetDirectoryName(filename);
            string fName = Path.GetFileNameWithoutExtension(filename);
            string fExt = Path.GetExtension(filename);
            return (fDir != null) ? Path.Combine(fDir, String.Concat(fName, suffix, fExt)) : null;
        }

        public string? SaveData(List<VaRect> rects, Size imgSize)
        {
            if (DataPath == null)
                return null;

            using (var streamWr = new StreamWriter(DataPath))
            {
                var txtWriter = new VaTxtWriter(streamWr, " ");
                rects.ForEach((r) =>
                {
                    txtWriter.WriteField(r.ObjectClass.ToString());
                    txtWriter.WriteField(r._Rect.Center.X / imgSize.Width);
                    txtWriter.WriteField(r._Rect.Center.Y / imgSize.Height);
                    txtWriter.WriteField(r._Rect.Width / imgSize.Width);
                    txtWriter.WriteField(r._Rect.Height / imgSize.Height);
                    txtWriter.NextRecord();
                });
            }

            return DataPath;
        }

        public List<VaRect> LoadData(Size imgSize)
        {
            List<VaRect> rects = new();

            try {
                if ((DataPath != null) || (File.Exists(DataPath)))
                {
                    using (var streamWr = new StreamReader(DataPath))
                    {
                        var txtReader = new VaTxtReader(streamWr, " ");

                        while (txtReader.NextRecord()) {
                            uint objClass = txtReader.ReadField<uint>();
                            var cX = txtReader.ReadField<double>() * imgSize.Width;
                            var cY = txtReader.ReadField<double>() * imgSize.Height;
                            var w = txtReader.ReadField<double>() * imgSize.Width;
                            var h = txtReader.ReadField<double>() * imgSize.Height;

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
            }

            return rects;
        }
    }
}
