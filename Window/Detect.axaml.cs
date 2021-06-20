using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using vaYolo.Model;
using vaYolo.ViewModels;
using vaYolo.Controls;
using ReactiveUI;
using Avalonia.Media;
using vaYolo.Helpers;
using Avalonia.Input;

namespace vaYolo.Views
{
    public partial class Detect : ReactiveWindow<DetectViewModel>
    {
        AdvancedImageBox ImageBox
        {
            get => this.Find<AdvancedImageBox>("ImageBox");
        }

        public Detect()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            ImageBox.GridColor = Brushes.DarkGray;
            ImageBox.GridColorAlternate = Brushes.DarkGray;

            this.WhenActivated(async (d) => {
                LoadImage();
            });
        }

        protected void OnOpenImageClick(object sender, RoutedEventArgs args) {
            LoadImage();
        }

        protected void OnRetrieveWeights(object sender, RoutedEventArgs args) {
            LoadImage();
        }
        private async void LoadImage() {
            ViewModel.Detect(await Util.GetPath(this));
        }

        protected void OnThresholdChanged(object sender, NumericUpDownValueChangedEventArgs args) {
            ViewModel.UpdateRoi();
        }
    }
}
