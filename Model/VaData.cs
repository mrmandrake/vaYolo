using System;
using System.Collections.Generic;
using System.Drawing;
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

        public string Save(string folder)
        {
            List<string> lines = new()
            {
                String.Format("classes = {0}", Classes),
                String.Format("train = {0}", TrainPath),
                String.Format("valid = {0}", ValidPath),
                String.Format("names = {0}", NamesPath),
                String.Format("backup = {0}", BackupPath)
            };

            var path = VaUtil.DataPath(folder);
            File.WriteAllLines(path, lines.ToArray());
            return path;
        }
    }
}