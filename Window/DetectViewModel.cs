using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using System.Reactive;
using ReactiveUI;
using Renci.SshNet;
using System.IO;
using AvaloniaEdit.Document;
using Renci.SshNet.Common;
using Avalonia.Media.Imaging;
using System.Text.RegularExpressions;
using vaYolo.Helpers;
using vaYolo.Model;
using vaYolo.Model.Yolo;

namespace vaYolo.ViewModels
{
    public class DetectViewModel : ReactiveObject
    {
        private Bitmap? img;
        public Bitmap? Img
        {
            get => img;
            private set => this.RaiseAndSetIfChanged(ref img, value);
        }

        private string Folder { get; set; }

        public DetectViewModel(string folder)
        {
        }
    }
}
