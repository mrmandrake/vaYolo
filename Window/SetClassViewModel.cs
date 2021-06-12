using System;
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

namespace vaYolo.ViewModels
{
    public class SetClassViewModel : ReactiveObject
    {
        private ObservableCollection<string> items;

        public ObservableCollection<string> Items {
            get { return items; }
            set { items = value;}
        }

        public SetClassViewModel(IEnumerable<string> names)
        {
            Items = new ObservableCollection<string>(names);
        }
    }
}
