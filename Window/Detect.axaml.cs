using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using vaYolo.Model;
using vaYolo.ViewModels;

namespace vaYolo.Views
{
    public partial class Detect : ReactiveWindow<DetectViewModel>
    {
        public DataGrid DG
        {
            get => this.FindControl<DataGrid>("dataGrid");
        }

        public Detect()
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
            var item = ((TabControl)sender).SelectedItem;
            if (item != null)
            {
                ViewModel.Update(((TabItemViewModel)item).ObjectClass);
                if (DG != null)
                    DG.Items = ViewModel.Items;
            }
                
        }
    }
}
