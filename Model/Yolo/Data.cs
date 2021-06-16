using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vaYolo.Helpers;

namespace vaYolo.Model.Yolo
{
    class Data
    {
        public int Classes { get; set; }

        public string TrainPath { get; set; }

        public string ValidPath { get; set; }

        public string NamesPath { get; set; }

        public string BackupPath { get; set; }

        public static Data Create(int classes, string remoteFolder)
        {

            return new Data()
            {
                Classes = classes,
                BackupPath = remoteFolder,
                TrainPath = Util.TrainListPath(remoteFolder),
                ValidPath = Util.ValidListPath(remoteFolder),
                NamesPath = Util.NamesPath(remoteFolder)
            };
        }

        public string Save(string path)
        {
            List<string> lines = new()
            {
                String.Format("classes = {0}", Classes),
                String.Format("train = {0}", TrainPath),
                String.Format("valid = {0}", ValidPath),
                String.Format("names = {0}", NamesPath),
                String.Format("backup = {0}", BackupPath)
            };

            File.WriteAllLines(path, lines.ToArray());
            return path;
        }
    }
}