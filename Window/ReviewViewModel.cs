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
    public class ReviewViewModel : ReactiveObject
    {
        public TabItemViewModel[] Tabs { get; set; }

        private ObservableCollection<Roi> items;

        public ObservableCollection<Roi> Items
        {
            get => items;
            set => this.RaiseAndSetIfChanged(ref items, value);
        }

        private string Folder { get; set; }

        public ReviewViewModel(string folder)
        {
            List<TabItemViewModel> tabList = new ();
            Names.Classes.ForEach((c) => {
                tabList.Add(new TabItemViewModel() {
                    ObjectClass = c._Class,
                    Header = c.Description
                });
            });

            Tabs = tabList.ToArray();
            Update(Tabs[0].ObjectClass);
            Folder = folder;
        }

        public async void Update(int objectClass)
        {
            List<Roi> roiList = new();
            await Task.Run(new Action(() =>
            {
                var imgs = Util.ListImagesInFolder(Folder);
                imgs.ForEach((img) =>
                {
                    var datapath = Util.TxtPath(img);
                    if (File.Exists(datapath))
                    {
                        var rects = (from r in VaRect.LoadData(datapath) where r.ObjectClass == objectClass select r).ToList();
                        roiList.AddRange(Roi.LoadData(rects, img));
                    }
                });

            }));

            Items = new ObservableCollection<Roi>(roiList);
        }
    }
}
