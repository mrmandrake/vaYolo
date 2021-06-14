using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
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

        public static new void Show(Window parent, ReviewViewModel model)
        {
            var dlg = new Review() {
                ViewModel = model            
            };
            
            if (parent != null)
                dlg.ShowDialog(parent);
            else
                dlg.Show();
        }

        public void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
    }
}
