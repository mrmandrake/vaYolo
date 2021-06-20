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
            this.WhenActivated(async (d) => {
                ViewModel.Detect(await Util.GetPath(this));
                ImageBox.GridColor = Brushes.Black;
                ImageBox.GridColorAlternate = Brushes.DarkGray;
            });
        }
    }
}
