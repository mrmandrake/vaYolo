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
using System;
using Avalonia.Threading;

namespace vaYolo.Views
{
    public partial class Console : ReactiveWindow<ConsoleViewModel>
    {
        private DispatcherTimer timer = new () { Interval = TimeSpan.FromMilliseconds(5000) };
        
        private TextEditor output
        {
            get => this.Find<TextEditor>("ConsoleOutput");
        }

        private TextBox input
        {
            get => this.Find<TextBox>("InputBlock");
        }

        public Console()
        {
            AvaloniaXamlLoader.Load(this);
            this.WhenActivated(async (d) =>
            {
                //input.KeyDown += InputBlock_KeyDown;
                //input.Focus();
                output.TextArea.Caret.CaretBrush = Brushes.Transparent;
                if (!ViewModel.Init())
                {
                    if (await MessageBox.Show(this, "Start train process?", "Training",
                                            MessageBox.MessageBoxButtons.YesNo) == MessageBox.MessageBoxResult.Yes)
                    {
                        ViewModel?.Sync();
                        ViewModel?.Start();
                        StartTimer();
                    }
                }
                else
                    StartTimer();
            });        
        }

        private void StartTimer()
        {
            timer.Tick += (sender, e) => ViewModel.Refresh();
            timer.Start();
        }

        protected void OnStartClick(object sender, RoutedEventArgs e) => ViewModel?.Start();

        protected void OnSyncClick(object sender, RoutedEventArgs e) => ViewModel?.Sync();

        protected void OnConnectClick(object sender, RoutedEventArgs e) => ViewModel?.Init();

        protected void OnQuitClick(object sender, RoutedEventArgs e) => ViewModel?.Finish();

        protected void OnRefreshClick(object sender, RoutedEventArgs e) => ViewModel?.Refresh();

        protected async void OnKillClick(object sender, RoutedEventArgs e)
        {
            if (await MessageBox.Show(this, "Kill current train process?", "Kill Training",
                                    MessageBox.MessageBoxButtons.YesNo) == MessageBox.MessageBoxResult.Yes)
            {
                timer.Stop();
                ViewModel?.Kill();
                Close();
            }
        }

        protected override void OnClosing(CancelEventArgs e) {
            ViewModel?.Finish();
            timer.Stop();
        } 

        void InputBlock_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) {
                ViewModel.Run(input.Text);
                input.Text = string.Empty;
                input.Focus();
                output.TextArea.ScrollToLine(output.Document.LineCount);
            }
        }     
    }
}