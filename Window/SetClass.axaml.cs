using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using vaYolo.ViewModels;

namespace vaYolo.Views
{
    public partial class SetClass : ReactiveWindow<SetClassViewModel>
    {
        public SetClass()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MainWindow.Instance.SetClass((uint)((ListBox)sender).SelectedIndex);
            Close();
        }
    }
}
