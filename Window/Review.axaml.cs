using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using vaYolo.ViewModels;

namespace vaYolo.Views
{
    public partial class Review : ReactiveWindow<ReviewViewModel>
    {
        public Review()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
