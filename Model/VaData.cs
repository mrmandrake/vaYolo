using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vaYolo.Helpers;

namespace vaYolo.Model
{
    class VaData
    {
        public int Classes { get; set; }

        public string TrainPath { get; set; }

        public string ValidPath { get; set; }

        public string NamesPath { get; set; }

        public string BackupPath { get; set; }

        public static VaData Create(int classes, string remoteFolder)
        {

            return new VaData()
            {
                Classes = classes,
                BackupPath = remoteFolder,
                TrainPath = VaUtil.TrainListPath(remoteFolder),
                ValidPath = VaUtil.ValidListPath(remoteFolder),
                NamesPath = VaUtil.NamesPath(remoteFolder)
            };
        }

        public string Save(string path)
        {
            return path;
        }
    }
}
