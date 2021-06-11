using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using System.ComponentModel;
using AvaloniaEdit;
using Avalonia.Interactivity;
using Avalonia.Media;
using vaYolo.ViewModels;

namespace vaYolo.Views
{
    public partial class Console : ReactiveWindow<ConsoleViewModel>
    {
        private TextEditor output
        {
            get => this.Find<TextEditor>("ConsoleOutput");
        }

        private TextBox input
        {
            get => this.Find<TextBox>("InputBlock");
        }

        public static void Show(MainWindow parent)
        {
            var dlg = new Console() { ViewModel = new ConsoleViewModel(parent.ViewModel.FolderPath) };
            if (parent != null)
                dlg.ShowDialog(parent);
            else
                dlg.Show();
        }

        public Console()
        {
            AvaloniaXamlLoader.Load(this);
            this.WhenActivated((d) =>
            {
                input.KeyDown += InputBlock_KeyDown;
                input.Focus();
                output.TextArea.Caret.CaretBrush = Brushes.Transparent;
                ViewModel.Init();
            });        
        }

        protected void OnStartClick(object sender, RoutedEventArgs e) => ViewModel?.Start();

        protected void OnConnectClick(object sender, RoutedEventArgs e) => ViewModel?.Init();

        protected void OnQuitClick(object sender, RoutedEventArgs e) => ViewModel?.Finish();

        protected void OnRefreshClick(object sender, RoutedEventArgs e) => ViewModel?.Refresh();

        protected void OnKillClick(object sender, RoutedEventArgs e) => ViewModel?.Kill();

        protected override void OnClosing(CancelEventArgs e) => ViewModel?.Finish();

        void InputBlock_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) {
                ViewModel.ShowCommand(input.Text);
                input.Text = string.Empty;
                input.Focus();
                output.TextArea.ScrollToLine(output.Document.LineCount);
            }
        }     
    }
}