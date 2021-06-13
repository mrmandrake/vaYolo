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

        public static new void Show(Window parent, SetClassViewModel model)
        {
            var dlg = new SetClass() {
                ViewModel = model            
            };
            
            if (parent != null)
                dlg.ShowDialog(parent);
            else
                dlg.Show();
        }
    }
}
