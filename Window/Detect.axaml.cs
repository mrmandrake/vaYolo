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
    }
}
