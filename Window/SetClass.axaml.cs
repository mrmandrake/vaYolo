using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace vaYolo.Views
{
    public partial class SetClass : Window
    {
        public SetClass()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public static new void Show(Window parent)
        {
            var dlg = new SetClass();
            if (parent != null)
                dlg.ShowDialog(parent);
            else
                dlg.Show();
        }
    }
}
