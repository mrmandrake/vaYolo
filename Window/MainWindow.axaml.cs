using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Platform;
using Avalonia.ReactiveUI;
using ReactiveUI;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using vaYolo;
using vaYolo.ViewModels;
using Avalonia.Interactivity;
using Avalonia.Media;

namespace vaYolo.Views
{
    public class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public Dictionary<Key, uint> classDigit = new()
        {
            { Key.D0, 0 },
            { Key.D1, 1 },
            { Key.D2, 2 },
            { Key.D3, 3 },
            { Key.D4, 4 },
            { Key.D5, 5 },
            { Key.D6, 6 },
            { Key.D7, 7 },
            { Key.D8, 8 },
            { Key.D9, 9 }
        };

        private WindowNotificationManager _notificationArea;

        public void Notify(string msg, string title = "vaYolo")
        {
            _notificationArea.Show(
                new Avalonia.Controls.Notifications.Notification(title, msg,
                Avalonia.Controls.Notifications.NotificationType.Information)
            );
        }

        private bool Saved { get; set; } = true;

        public VaImage Ctrl
        {
            get { return this.Find<VaImage>("SegImage"); }
        }

        public MainWindow()
        {
            InitializeComponent();
            this.AttachDevTools();
            DataContext = new MainWindowViewModel();

            _notificationArea = new WindowNotificationManager(this)
            {
                Position = NotificationPosition.BottomRight,
                Margin = new Thickness(0, 0, 30, 100),
                MaxItems = 5
            };

            this.WhenActivated((d) => { 
                HasSystemDecorations = true;
                // WindowState = WindowState.Maximized; 
            });
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        protected bool Draw { get; set; } = false;

        protected void ImageClickDown(object sender, PointerPressedEventArgs e)
        {
            Draw = true;
            var props = e.GetCurrentPoint((Avalonia.Controls.Image)sender).Properties;
            var pos = e.GetCurrentPoint((Avalonia.Controls.Image)sender).Position;
            if (props.IsLeftButtonPressed)
                Ctrl.Set(pos);

            if (props.IsRightButtonPressed)
                Ctrl.Del(pos);
        }

        protected void ImageClickUp(object sender, PointerReleasedEventArgs e)
        {
            Draw = false;
            Ctrl.Add();
        }

        protected void ImageClickMove(object sender, PointerEventArgs e)
        {
            if (Draw)
                Ctrl.Move(e.GetCurrentPoint((Avalonia.Controls.Image)sender).Position);
        }

        public async Task<string?> GetPath()
        {
            OpenFileDialog dialog = new OpenFileDialog() { AllowMultiple = false };
            dialog.Filters.Add(new FileDialogFilter() { Name = "Images", Extensions = { "jpg;png" } });
            string[] result = await dialog.ShowAsync(this);
            return (result != null && result.Length > 0) ? result[0] : null;
        }

        private List<string> GetImagesInFolder(string? path) {
            try {
                var dir = Path.GetDirectoryName(path);
                var ext = Path.GetExtension(path);
                return Directory.EnumerateFiles(dir,  "*" + ext).ToList();
            }
            catch (Exception ) {

            }

            return new List<string>();
        }

        private List<string> GetImagesInFolder(string dir, string ext) {
            try {
                return Directory.EnumerateFiles(dir,  "*" + ext).ToList();
            }
            catch (Exception ) {
            }

            return new List<string>();
        }        

        private bool TryLoadFirstImage(string dir, List<string> extList) {
            foreach (var e in extList) {
                var imgs = GetImagesInFolder(dir, e);
                if (imgs.Count > 0) {
                    LoadImageByPath(imgs[0]);
                    return true;
                }
            }

            return false;
        }

        private async void LoadFolder() {
            var folder = await new OpenFolderDialog() {
                Title = "Open Folder Dialog"
            }.ShowAsync(this);

            if (!TryLoadFirstImage(folder, new List<string>() {
                ".png", ".jpg", ".jpeg",
                ".PNG", ".JPG", ".JPEG"}))
                Notify("No Images found!!", "Loading content");
        }

        private string? GetNextPath()
        {
            var images = GetImagesInFolder(ViewModel.ImagePath);
            var idx = images.IndexOf(ViewModel.ImagePath);
            if (idx <= 0) {
                Notify("First Image", "Load Prev Image");
                return null;                
            }

            return images[--idx];
        }

        private string? GetPrevPath()
        {
            var images = GetImagesInFolder(ViewModel.ImagePath);
            var idx = images.IndexOf(ViewModel.ImagePath);
            if (idx == images.Count - 1) {
                Notify("Last Image", "Load Next Image");
                return null;
            }                

            return images[++idx];
        }                

        protected async override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            uint classVal = 0;
            if (classDigit.TryGetValue(e.Key, out classVal))
            {
                Ctrl.CurrentObjectClass = classVal;
                Notify("Set Class " + classVal);
            }
            else
                switch (e.Key)
                {
                    case Key.Left:
                        LoadImageByPath(GetNextPath());
                        break;

                    case Key.Right:
                        LoadImageByPath(GetPrevPath());
                        break;

                    case Key.O:
                        LoadImage();
                        break;

                    case Key.F:
                        LoadFolder();
                        break;

                    case Key.S:
                        SaveData();
                        break;

                    case Key.Escape:
                        if (await MessageBox.Show(this, "Really quit from vaYolo?", "Quit Request",
                                MessageBox.MessageBoxButtons.YesNo) == MessageBox.MessageBoxResult.Yes)
                            this.Close();
                        break;
                }
        }

        private void SaveData()
        {
            if (ViewModel == null)
                return;

            var path = ViewModel.SaveData(VaManager.Instance.Rects, Ctrl.DesiredSize);
            Saved = true;
        }        

        private void DataSavedCheck() {
            if (Ctrl != null)
            {
                if (!Saved &&
                    VaManager.Instance.Rects != null &&
                    VaManager.Instance.Rects.Count > 0)
                    SaveData();

                Ctrl.Reset();
            }            
        }

        private async void LoadImageByPath(string? imagePath)
        {
            if (imagePath == null)
                return;

            if (ViewModel == null)
                return;

            DataSavedCheck();
            Saved = false;

            await ViewModel.LoadImage(imagePath);
            if (ViewModel.Img != null)
                VaManager.Instance.Rects = ViewModel.LoadData(ViewModel.Img.Size);

            if (Settings.MaximizeAfterLoad)
                MaximizeAfterLoad();
        }

        private void MaximizeAfterLoad()
        {
            this.WindowState = WindowState.Maximized;
            Notify(ViewModel.ImagePath + " Loaded!", "Loaded Image");
        }

        private async void LoadImage()
        {
            if (ViewModel == null)
                return;

            DataSavedCheck();
            Saved = false;

            await ViewModel.LoadImage(await GetPath());
            if (ViewModel.Img != null)
                VaManager.Instance.Rects = ViewModel.LoadData(ViewModel.Img.Size);

            if (Settings.MaximizeAfterLoad)
                MaximizeAfterLoad();
        }

        private void SetCurrentClass(uint c)
        {
            Ctrl.CurrentObjectClass = c;
            Notify("Current class:" + c);
        }

        public async void ShowAbout()
        {
            await MessageBox.Show(this,
                "vaYolo 0.0.1 version\n\n\n" + 
                "Another Yolo Image segmentation tool\n\n" +
                "Optimized for MacOS / Astropad / Ipad and Pencil\n",
                "image tool segmentation",
                    MessageBox.MessageBoxButtons.Ok);
        }

        public void ShowSetupTrain()
        {
            Setup.Show(this);
        }

        public void ShowConsoleTrain()
        {
            Setup.Show(this);
        }                    

        public void ShowSetClass()
        {
            Setup.Show(this);
        }

        public override void Render(DrawingContext context)
        {
            base.Render(context);
        }


        #region menuclick_handlers
        public void OnNativeLoadFolderClicked(object sender, EventArgs args) => LoadFolder();
        public void OnLoadFolderClicked(object sender, RoutedEventArgs args) => LoadFolder();

        public void OnNativePrevImageClicked(object sender, EventArgs args) => LoadImageByPath(GetPrevPath());
        public void OnPrevImageClicked(object sender, RoutedEventArgs args) => LoadImageByPath(GetPrevPath());

        public void OnNativeNextImageClicked(object sender, EventArgs args) => LoadImageByPath(GetNextPath());
        public void OnNextImageClicked(object sender, RoutedEventArgs args) => LoadImageByPath(GetNextPath());

        public void OnNativeAboutClicked(object sender, EventArgs args) => ShowAbout();
        public void OnAboutClicked(object sender, RoutedEventArgs args) => ShowAbout();

        public void OnNativeSetClassClicked(object sender, EventArgs args) => ShowSetClass();
        public void OnSetClassClicked(object sender, RoutedEventArgs args) => ShowSetClass();

        public void OnNativeReviewClicked(object sender, EventArgs args) => ShowSetClass();
        public void OnReviewClicked(object sender, RoutedEventArgs args) => ShowSetClass();


        public void OnNativeSetupClicked(object sender, EventArgs args) => ShowSetupTrain();
        public void OnSetupClicked(object sender, RoutedEventArgs args) => ShowSetupTrain();

        public void OnNativeConsoleClicked(object sender, EventArgs args) => ShowConsoleTrain();
        public void OnConsoleClicked(object sender, RoutedEventArgs args) => ShowConsoleTrain();

#endregion
    }
}
