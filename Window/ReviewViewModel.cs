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
using vaYolo.Models;

namespace vaYolo.ViewModels
{
    public class TabItemViewModel
    {
        public string Header { get; set; }
        public bool IsEnabled { get; set; } = true;
    }

    public class ReviewViewModel : ReactiveObject
    {
        public TabItemViewModel[] Tabs { get; set; }

        private ObservableCollection<VaRoi> items;

        public ObservableCollection<VaRoi> Items
        {
            get { return items; }
            set { items = value; }
        }

        public ReviewViewModel(string folder)
        {
            List<VaRoi> roiList = new ();
            VaUtil.ListImagesInFolder(folder).ForEach((img) =>
            {
                var datapath = VaUtil.GetDatapath(img);
                if (File.Exists(datapath)) {
                    var rects = VaRect.LoadData(datapath);
                    roiList.AddRange(VaRoi.LoadData(rects, img));
                }
            });

            Items = new ObservableCollection<VaRoi>(roiList);
            List<TabItemViewModel> tabList = new ();
            VaNames.GetNames().ToList().ForEach((c) => {
                tabList.Add(new TabItemViewModel() {
                    Header = c
                });
            });

            Tabs = tabList.ToArray();
        }
    }
}
