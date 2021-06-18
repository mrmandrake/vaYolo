using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using vaYolo.ViewModels;
using Avalonia.Interactivity;
using Avalonia.Media;
using vaYolo.Helpers;
using vaYolo.Model.Yolo;
using System.Linq;

namespace vaYolo.Views
{
    public class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public static MainWindow Instance { get; private set; }

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
            Instance = this;
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
                LoadProject();
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
            if (Draw)
            {
                Draw = false;
                Ctrl.Add();
            }
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

        private bool LoadFirstImage(string dir) {
            var imgs = Util.ListImagesInFolder(dir);
            if (imgs.Count > 0) {
                LoadImageByPath(imgs[0]);
                return true;
            }

            return false;
        }

        private async void LoadProject() {
            var folder = await new OpenFolderDialog() {
                Title = "Open Folder Dialog"
            }.ShowAsync(this);

            if (folder == null) {
                Notify("No folder selected!", "Loading content");
                return;
            }

            LoadProject(folder);
        }

        private void LoadProject(string folder) {
            Settings.Load(folder);
            Names.Load(folder);
            ViewModel.FolderPath = folder;
            File.Delete(Util.ChartPath(folder));

            if (!LoadFirstImage(folder))
                Notify("No Images found!!", "Loading content");
        }

        private async void CreateProject() {
            var folder = await new OpenFolderDialog() {
                Title = "Select Folder Dialog"
            }.ShowAsync(this);

            if (folder == null) {
                Notify("No folder selected!", "Loading content");
                return;
            }

            LoadProject(folder);
        }

        private async void ImportImages() {
            var lst = await new OpenFileDialog() {
                Title = "Select Images to import Dialog",
                AllowMultiple = true
            }.ShowAsync(this);

            int cnt = 0;
            lst.ToList().ForEach((f) => {
                try {
                    ImgSaver.ConvertToPng(f, ViewModel.FolderPath);
                    cnt++;
                }
                catch (Exception exc) {
                    Notify(String.Format("Error: {0} not Copied in  {1}", f, ViewModel.FolderPath));
                }
            });

            Notify(String.Format("{0} Images imported", cnt));
            LoadProject(ViewModel.FolderPath);
        }        

        public void SetClass(uint objectClass)
        {
            Ctrl.CurrentObjectClass = objectClass;
            Notify("Current object class:" + objectClass);
        }

        protected async override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            uint classVal = 0;
            if (classDigit.TryGetValue(e.Key, out classVal))
            {
                SetClass(classVal);
                Notify("Set Class " + classVal);
            }
            else
                switch (e.Key)
                {
                    case Key.Left:
                        LoadImageByPath(Util.GetNextPath(ViewModel.ImagePath));
                        break;

                    case Key.Right:
                        LoadImageByPath(Util.GetPrevPath(ViewModel.ImagePath));
                        break;

                    case Key.O:
                        LoadImage();
                        break;

                    case Key.F:
                        LoadProject();
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

            var path = ViewModel.SaveData(Manager.Instance.Rects);
            Saved = true;
        }        

        private void DataSavedCheck(bool reset = true) {
            if (Ctrl != null)
            {
                if (!Saved &&
                    Manager.Instance.Rects != null &&
                    Manager.Instance.Rects.Count > 0)
                    SaveData();

                Saved = false;

                if (reset)
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

            await ViewModel.LoadImage(imagePath);
            if (ViewModel.Img != null)
                Manager.Instance.Rects = ViewModel.LoadData();

            if (Settings.Get().MaximizeAfterLoad)
                MaximizeAfterLoad();
        }

        private async void DeleteCurrentImage()
        {
            if (ViewModel == null)
                return;

            if (await MessageBox.Show(this, "Really delete current image?", "Delete Request", MessageBox.MessageBoxButtons.YesNo) == MessageBox.MessageBoxResult.Yes)
                ViewModel.DeleteImage();

            LoadFirstImage(ViewModel.FolderPath);
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

            await ViewModel.LoadImage(await GetPath());
            if (ViewModel.Img != null)
                Manager.Instance.Rects = ViewModel.LoadData();

            if (Settings.Get().MaximizeAfterLoad)
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
            DataSavedCheck();
            new Setup() { ViewModel = new SetupViewModel(ViewModel.FolderPath) }.Show(this);
        }

        public void ShowConsoleTrain()
        {
            DataSavedCheck();

            new Console() {
                ViewModel = new ConsoleViewModel(ViewModel.FolderPath)
            }.Show(this);
        }                    

        public void ShowSetClass()
        {
            Names.Load(ViewModel.FolderPath);
            new SetClass() {
                ViewModel = new SetClassViewModel(Names.GetNames())
            }.ShowDialog(this);
        }

        public void ShowReview()
        {
            DataSavedCheck(false);
            new Review()
            {
                ViewModel = new ReviewViewModel(ViewModel.FolderPath)
            }.ShowDialog(this);
        }        

        public override void Render(DrawingContext context)
        {
            base.Render(context);
        }


#region menuclick_handlers
        public void OnNativeLoadProjectClicked(object sender, EventArgs args) => LoadProject();
        public void OnLoadProjectClicked(object sender, RoutedEventArgs args) => LoadProject();

        public void OnNativeCreateProjectClicked(object sender, EventArgs args) => CreateProject();
        public void OnCreateProjectClicked(object sender, RoutedEventArgs args) => CreateProject();

        public void OnNativeImportImagesClicked(object sender, EventArgs args) => ImportImages();
        public void OnImportImagesClicked(object sender, RoutedEventArgs args) => ImportImages();


        public void OnNativePrevImageClicked(object sender, EventArgs args) => 
            LoadImageByPath(Util.GetPrevPath(ViewModel.ImagePath));
        public void OnPrevImageClicked(object sender, RoutedEventArgs args) => 
            LoadImageByPath(Util.GetPrevPath(ViewModel.ImagePath));

        public void OnNativeNextImageClicked(object sender, EventArgs args) => 
            LoadImageByPath(Util.GetNextPath(ViewModel.ImagePath));
        public void OnNextImageClicked(object sender, RoutedEventArgs args) => 
            LoadImageByPath(Util.GetNextPath(ViewModel.ImagePath));

        public void OnNativeAboutClicked(object sender, EventArgs args) => ShowAbout();
        public void OnAboutClicked(object sender, RoutedEventArgs args) => ShowAbout();

        public void OnNativeSetClassClicked(object sender, EventArgs args) => ShowSetClass();
        public void OnSetClassClicked(object sender, RoutedEventArgs args) => ShowSetClass();

        public void OnNativeReviewClicked(object sender, EventArgs args) => ShowReview();
        public void OnReviewClicked(object sender, RoutedEventArgs args) => ShowReview();

        public void OnNativeDeleteImageClicked(object sender, EventArgs args) => DeleteCurrentImage();
        public void OnDeleteImageClicked(object sender, RoutedEventArgs args) => DeleteCurrentImage();

        public void OnNativeSetupClicked(object sender, EventArgs args) => ShowSetupTrain();
        public void OnSetupClicked(object sender, RoutedEventArgs args) => ShowSetupTrain();

        public void OnNativeConsoleClicked(object sender, EventArgs args) => ShowConsoleTrain();
        public void OnConsoleClicked(object sender, RoutedEventArgs args) => ShowConsoleTrain();
#endregion
    }
}
