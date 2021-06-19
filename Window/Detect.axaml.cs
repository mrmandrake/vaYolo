using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using vaYolo.Model;
using vaYolo.ViewModels;
using vaYolo.Controls;
using ReactiveUI;

namespace vaYolo.Views
{
    public partial class Detect : ReactiveWindow<DetectViewModel>
    {
        public Detect()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            this.WhenActivated((d) => { 
                ViewModel.Detect();
            });
        }
    }
}
