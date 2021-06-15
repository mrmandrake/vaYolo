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
using vaYolo.Model;

namespace vaYolo.ViewModels
{
    public class ReviewViewModel : ReactiveObject
    {
        public TabItemViewModel[] Tabs { get; set; }

        private List<VaRoi> roiList = new();

        private ObservableCollection<VaRoi> items;

        public ObservableCollection<VaRoi> Items
        {
            get => items;
            set => this.RaiseAndSetIfChanged(ref items, value);
        }

        public ReviewViewModel(string folder)
        {
            var imgs = VaUtil.ListImagesInFolder(folder);
            imgs.ForEach((img) =>
            {
                var datapath = VaUtil.GetDatapath(img);
                if (File.Exists(datapath)) {
                    var rects = VaRect.LoadData(datapath);
                    roiList.AddRange(VaRoi.LoadData(rects, img));
                }
            });

            Items = new ObservableCollection<VaRoi>(roiList);
            List<TabItemViewModel> tabList = new ();
            VaNames.Classes.ForEach((c) => {
                tabList.Add(new TabItemViewModel() {
                    ObjectClass = c.ObjectClass,
                    Header = c.Description
                });
            });

            Tabs = tabList.ToArray();
            Update(Tabs[0].ObjectClass);
        }

        public void Update(int objectClass)
        {
            Items = new ObservableCollection<VaRoi>((from r in roiList where r.ObjectClass == objectClass select r).ToList());
        }
    }
}
