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

namespace vaYolo.ViewModels
{
    public class ReviewViewModel : ReactiveObject
    {
        public TabItemViewModel[] Tabs { get; set; }

        private ObservableCollection<VaRoi> items;

        public ObservableCollection<VaRoi> Items
        {
            get => items;
            set => this.RaiseAndSetIfChanged(ref items, value);
        }

        private string Folder { get; set; }

        public ReviewViewModel(string folder)
        {
            List<TabItemViewModel> tabList = new ();
            VaNames.Classes.ForEach((c) => {
                tabList.Add(new TabItemViewModel() {
                    ObjectClass = c.ObjectClass,
                    Header = c.Description
                });
            });

            Tabs = tabList.ToArray();
            Update(Tabs[0].ObjectClass);
            Folder = folder;
        }

        public async void Update(int objectClass)
        {
            List<VaRoi> roiList = new();
            await Task.Run(new Action(() =>
            {
                var imgs = VaUtil.ListImagesInFolder(Folder);
                imgs.ForEach((img) =>
                {
                    var datapath = VaUtil.DataPath(Folder);
                    if (File.Exists(datapath))
                    {
                        var rects = (from r in VaRect.LoadData(datapath) where r.ObjectClass == objectClass select r).ToList();
                        roiList.AddRange(VaRoi.LoadData(rects, img));
                    }
                });

            }));

            Items = new ObservableCollection<VaRoi>(roiList);
        }
    }
}
