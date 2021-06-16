using System;
using System.Collections.ObjectModel;
using ReactiveUI;
using System.IO;
using AvaloniaEdit.Document;
using Avalonia.Media.Imaging;
using System.Text.RegularExpressions;
using vaYolo.Helpers;
using vaYolo.Model;
using vaYolo.Model.Yolo;
using Microsoft.VisualBasic.FileIO;
using vaYolo.Ext;
using System.Threading.Tasks;

namespace vaYolo.ViewModels
{
    public class ConsoleViewModel : ReactiveObject
    {
        TextDocument document;
        public TextDocument Document
        {
            get => document;
            set => this.RaiseAndSetIfChanged(ref document, value);
        }

        string sshServer = Settings.Get().SshServer;
        public string SshServer
        {
            get => sshServer;
            set => this.RaiseAndSetIfChanged(ref sshServer, value);
        }

        string sshPort = Convert.ToString(Settings.Get().SshPort);
        public string SshPort
        {
            get => sshPort;
            set => this.RaiseAndSetIfChanged(ref sshPort, value);
        }

        string sshUsername = Settings.Get().SshUsername;
        public string SshUsername
        {
            get => sshUsername;
            set => this.RaiseAndSetIfChanged(ref sshUsername, value);
        }
        string sshPassword = Settings.Get().SshPassword;
        public string SshPassword
        {
            get => sshPassword;
            set => this.RaiseAndSetIfChanged(ref sshPassword, value);
        }

        string sshLocalFolder;
        public string? SshLocalFolder
        {
            get => sshLocalFolder;
            set => this.RaiseAndSetIfChanged(ref sshLocalFolder, value);
        }

        string sshRemoteFolder;
        public string SshRemoteFolder
        {
            get => sshRemoteFolder;
            set => this.RaiseAndSetIfChanged(ref sshRemoteFolder, value);
        }

        string sshDarknet = Settings.Get().SshRemoteDarknet;
        public string SshDarknet
        {
            get => sshDarknet;
            set => this.RaiseAndSetIfChanged(ref sshDarknet, value);
        }

        string sshScreenName;
        public string SshScreenName
        {
            get => sshScreenName;
            set => this.RaiseAndSetIfChanged(ref sshScreenName, value);
        }

        Bitmap? chart;
        public Bitmap? Chart
        {
            get => chart;
            set => this.RaiseAndSetIfChanged(ref chart, value);
        }

        string consoleInput = string.Empty;
        public string ConsoleInput
        {
            get => consoleInput;
            set => this.RaiseAndSetIfChanged(ref consoleInput, value);
        }

        int screenPid;
        public int ScreenPid
        {
            get => screenPid;
            set => this.RaiseAndSetIfChanged(ref screenPid, value); 
        }

        public string FolderName { get; set; }

        public string ConfigTemplate { get; set; } = "yolo v4 tiny custom";

        ObservableCollection<string> consoleOutput = new();

        public ObservableCollection<string> ConsoleOutput
        {
            get => consoleOutput;
            set => this.RaiseAndSetIfChanged(ref consoleOutput, value);
        }

        public ConsoleViewModel(string? folder)
        {
            Document = new TextDocument();

            if (folder != null)
            {
                SshLocalFolder = folder;
                FolderName = new DirectoryInfo(folder).Name;
                SshScreenName = "vayolo." + FolderName;
                SshRemoteFolder = Path.Combine(Settings.Get().SshRemote, SshScreenName).Replace('\\', '/');
            }
        }

        public void write(string str) {
            Document.Insert(Document.TextLength, str + "\n");
        } 

        public bool Init()
        {
            write(String.Format("> CONNECTING to -> {0}:{1}....", SshServer, SshPort));
            write(String.Format("Username:{0}", SshUsername));
            write(String.Format("Local Folder:{1} ", SshLocalFolder));
            write(String.Format("Remote Folder:{2}", SshRemoteFolder));

            if (!Ssh.Init(sshServer, Convert.ToUInt16(sshPort), 
                sshUsername, sshPassword)) {
                    write("...ERROR NOT CONNECTED!");
                    return false;
            }                

            write("...CONNECTED!");
            ScreenPid = GetScreenPid();            
            write("Remote Folder " + (!Sftp.Exists(Ssh.Connection, SshRemoteFolder) ? "NOT" : "") + " exist!");
            return true;
        }

        public bool Sync()
        {
            write("Sync...");
            DeleteSupportFiles(sshLocalFolder);

            var (s1, s2) = Sftp.Sync(Ssh.Connection, SshLocalFolder, SshRemoteFolder);
            if (s1)
                s2.ForEach((f) => write("Uploading " + f));
            write("...Sync complete");
            return s1;
        }

        private void DeleteSupportFiles(string sshLocalFolder)
        {
            File.Delete(Util.ChartPath(sshLocalFolder));
            File.Delete(Util.DataPath(sshLocalFolder));
            File.Delete(Util.TrainListPath(sshLocalFolder));
            File.Delete(Util.ValidListPath(sshLocalFolder));
            File.Delete(Util.ConfigPath(sshLocalFolder));
        }

        public void Finish()
        {
            write("> DISCONNECTING...");            
            Ssh.Finish();
            write("...DISCONNECTED!");

        }    

        private int GetScreenPid()
        {
            int pid = -1;
            string re = String.Format("[0123456789]+(.{0})", sshScreenName);
            string? output = Ssh.GetCmdResult("screen -list");
            if (output != null) {
                Match m = Regex.Match(output, re, RegexOptions.IgnoreCase);

                if (m.Success) {
                    pid = Convert.ToInt32(m.Value.Substring(0, m.Value.IndexOf('.')));                    
                    if (pid > 0) 
                        write(String.Format("Found job active with pid {0}", pid));
                }

                if (pid <= 0)
                    write("No active job found");
            }
            else
                write("ERROR Listing jobs actives");

            return pid;
        }

        private void LaunchTrain()
        {
            string setfolder = String.Format("cd {0} && ", SshRemoteFolder);
            string screencmd = String.Format("screen -d -m -L -S {0} ", SshScreenName);
            string timecmd = "/usr/bin/time --verbose ";
            string darknetcmd = String.Format("{0} detector -map -dont_show train ", SshDarknet);
            string dataPath = String.Format("{0}/{1}.data ", SshRemoteFolder, FolderName);
            string cfgPath = String.Format("{0}/{1}.cfg ", SshRemoteFolder, FolderName);
            string redirectStd = " 2>&1";
            var cmd = setfolder + screencmd + timecmd + darknetcmd + dataPath + cfgPath + redirectStd;
            //string test = "ping 127.0.0.1 &2>&1";
            // var cmd = setfolder + screencmd + test;
            write("> RUNNING " + cmd);
            write(Ssh.Run(cmd));
        }

        private void GetLogTail(string remote, int lines = 30) {
            write(Ssh.Run(String.Format("tail -n {0} {1} ", lines, Path.Combine(SshRemoteFolder,"screenlog.0").Replace('\\', '/'))));
        }

        private void GetChart()
        {
            if (Sftp.Download(Ssh.Connection, sshRemoteFolder, sshLocalFolder, "chart.png")) {
                try {
                    var chartPath = Util.ChartPath(sshLocalFolder);

                    if (new FileInfo(chartPath).Length > 0)
                        Chart = Bitmap.DecodeToWidth(File.OpenRead(chartPath),
                                1920, Avalonia.Visuals.Media.Imaging.BitmapInterpolationMode.LowQuality);

                    File.Delete(chartPath);
                }
                catch (Exception exc) {
                    write(exc.Message);
                }
            }
        }

        public void Start() {
            CreateTrain();
            CreateValid();
            CreateData();
            CreateConfig();
            LaunchTrain();
            ScreenPid = GetScreenPid();
        }

        private void CreateConfig()
        {
            var cfgPath = Config.FromTemplate(ConfigTemplate, Settings.Get().GetSetup()).Save(Util.ConfigPath(sshLocalFolder));
            Sftp.Upload(Ssh.Connection, SshRemoteFolder, SshLocalFolder, Path.GetFileName(cfgPath));
        }

        private void CreateData()
        {
            var dataPath = Data.Create(Names.Classes.Count, sshRemoteFolder).Save(Util.DataPath(sshLocalFolder));
            Sftp.Upload(Ssh.Connection, SshRemoteFolder, SshLocalFolder, Path.GetFileName(dataPath));
        }

        private void CreateTrain()
        {
            var trainListPath = Util.ListLabeledInFolder(SshLocalFolder).Rebase(SshLocalFolder, SshRemoteFolder).Save(Util.TrainListPath(sshLocalFolder));
            Sftp.Upload(Ssh.Connection, SshRemoteFolder, SshLocalFolder, Path.GetFileName(trainListPath));
        }

        private void CreateValid()
        {
            var validListPath = Util.ListLabeledInFolder(SshLocalFolder).Rebase(SshLocalFolder, SshRemoteFolder).Save(Util.ValidListPath(sshLocalFolder));
            Sftp.Upload(Ssh.Connection, SshRemoteFolder, SshLocalFolder, Path.GetFileName(validListPath));
        }

        public void Refresh() {
            Document = new TextDocument();
            GetLogTail(SshRemoteFolder);
            GetChart();
        } 

        public void Kill() {
            write("> KILLING pid:" + ScreenPid);
            Ssh.Kill(ScreenPid);
            write("... KILLED!");
            ScreenPid = -1;
        }

        public void Run(string cmd) {
            write(Ssh.Run(cmd));
        }
    }
}
