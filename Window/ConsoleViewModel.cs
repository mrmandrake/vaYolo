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

namespace vaYolo.ViewModels
{
    public class ConsoleViewModel : ReactiveObject
    {
        private List<string> lstExt = new()
        {
            "*.png",
            "*.jpg",
            "*.txt",
            "*.cfg",
            "*.names",
            "*.data"
        };

        ConnectionInfo conn;
        SshClient sshclient;
        SftpClient sftpclient;

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
                SshScreenName = "vayolo." + new DirectoryInfo(folder).Name;
                SshRemoteFolder = Path.Combine(Settings.Get().SshRemote, SshScreenName);
            }
        }

        public void write(string str) => Document.Insert(Document.TextLength, str + "\n");

        public bool Init()
        {
            write("Connecting to -> " + SshServer + ":" + SshPort);
            write("Additional info:");
            write("Username:" + SshUsername);
            write("Local Folder:" + SshLocalFolder);
            write("Remote Folder:" + SshRemoteFolder);

            try
            {
                conn = new(SshServer, Convert.ToUInt16(SshPort),
                    SshUsername,
                    new AuthenticationMethod[] {
                        new PasswordAuthenticationMethod(SshUsername, SshPassword)}) {
                    Timeout = new TimeSpan(0, 0, 5)
                };

                sshclient = new SshClient(conn);
                sshclient.Connect();
                write("Connected!");
            }
            catch (Exception exc)
            {
                write(exc.Message);
            }

            ScreenPid = GetScreenPid();
            if (ScreenPid > 0) 
                write(String.Format("Found screen active with pid {0}", ScreenPid));
            else
                write("No active screen found");


            write("Remote Folder " + (!SftpExists(SshRemoteFolder) ? "NOT" : "") + " exist!");
            SftpSyncTo(SshLocalFolder, SshRemoteFolder);
            return true;
        }

        public void Finish()
        {
            if (sshclient != null &&
                sshclient.IsConnected)
            {
                write("Disconnect...");
                sshclient.Disconnect();
                write("Disconnected!");
            }
        }

        public void ShowCommand(string cmd)
        {
            if (sshclient == null)
            {
                write("client not connected!!!");
                return;
            }

            try
            {
                using (var _cmd = sshclient.CreateCommand(cmd))
                {
                    var result = _cmd.BeginExecute();
                    using (var reader = new StreamReader(_cmd.ExtendedOutputStream, Encoding.UTF8, true, 1024, true))
                    {
                        while (!result.IsCompleted || !reader.EndOfStream)
                        {
                            string line = reader.ReadLine();
                            if (line != null)
                                write(line);
                        }

                        if (_cmd.Result != null)
                            write(_cmd.Result);
                    }

                    _cmd.EndExecute(result);
                }
            }
            catch (Exception exc)
            {
                write(exc.Message);
            }
        }

        public string? GetCommandResult(string cmd)
        {
            if (sshclient == null)
            {
                write("client not connected!!!");
                return string.Empty;
            }

            try
            {
                using (var _cmd = sshclient.CreateCommand(cmd))
                {
                    var asres = _cmd.BeginExecute();
                    using (var reader = new StreamReader(_cmd.ExtendedOutputStream, Encoding.UTF8, true, 1024, true))
                    {
                        while (!asres.IsCompleted || !reader.EndOfStream)
                            reader.ReadLine();

                        return _cmd.Result;
                    }

                    _cmd.EndExecute(asres);
                }
            }
            catch (Exception exc)
            {
                write(exc.Message);
            }

            return string.Empty;
        }

        public bool SftpGet(string filename)
        {
            try
            {
                using (var sftp = new SftpClient(conn))
                {
                    sftp.Connect();
                    sftp.ChangeDirectory(SshRemoteFolder);
                    using (Stream f = File.OpenWrite(Path.Combine(SshLocalFolder, filename))) {
                        write(String.Format("Downloading {0} from {1} in {2}", 
                            filename, 
                            SshRemoteFolder, 
                            SshLocalFolder));

                        sftp.DownloadFile(filename, f);
                    }

                    sftp.Disconnect();
                }
                return true;
            }
            catch (Exception exc)
            {
                write (exc.Message);
            }

            return false;
        }

        public bool SftpExists(string path)
        {
            bool result = false;
            using (var sftp = new SftpClient(conn))
            {
                try
                {
                    sftp.Connect();
                    result = sftp.Exists(path);
                }
                catch (Exception exc)
                {
                    write(exc.Message);
                }
            }

            return result;
        }

        private bool SftpSyncTo(string local, string remote)
        {
            using (var sftp = new SftpClient(conn))
            {
                try {
                    sftp.Connect();
                    if (!sftp.IsConnected)
                        return false;

                    // Try to create a remote directory. If it throws an exception, we will assume
                    // for now that the directory already exists. See https://github.com/sshnet/SSH.NET/issues/25
                    try {
                        sftp.CreateDirectory(remote);
                    }
                    catch (SshException exc)
                    {
                        // write(exc.Message);
                    }

                    lstExt.ForEach((ext) =>
                    {
                        foreach (var file in sftp.SynchronizeDirectories(local, remote, ext))
                            write("Updating " + file.Name);
                    });

                }
                catch (Exception exc)
                {
                    write(exc.Message);
                }


                return true;
            }

            return false;
        }

        private int GetScreenPid()
        {
            string re = String.Format("[0123456789]+(.{0})", sshScreenName);
            string output = GetCommandResult("screen -list");
            Match m = Regex.Match(output, re, RegexOptions.IgnoreCase);

            if (m.Success)
                return Convert.ToInt32(m.Value.Substring(0, m.Value.IndexOf('.')));

            return -1;
        }

        private void LaunchTrain(string remote, string name, string darknetPath)
        {
            string setfolder = String.Format("cd {0} &&", remote);
            string screencmd = String.Format("screen -d -m -L -S {0} ", name);
            string timecmd = "/usr/bin/time --verbose ";
            string test = "ping 127.0.0.1 &2>&1";
            string darknetcmd = String.Format("{0} detector -map -dont_show train", darknetPath);
            string dataPath = String.Format("{0}/setok.data", remote);
            string cfgPath = String.Format("{0}/setok.cfg", remote);
            string redirectStd = "2>&1";
            // ShowCommand(setfolder + screencmd + timecmd + darknetcmd + dataPath + cfgPath + redirectStd);
            ShowCommand(setfolder + screencmd + test);
        }

        private void GetLogTail(string remote) => ShowCommand("tail -n 50 " + Path.Combine(SshRemoteFolder,"screenlog.0"));

        private void GetChart()
        {
            if (SftpGet("chart.png")) {
                try {
                    Chart = Bitmap.DecodeToWidth(File.OpenRead(Path.Combine(sshLocalFolder,"chart.png")),
                            1920, Avalonia.Visuals.Media.Imaging.BitmapInterpolationMode.LowQuality);
                }
                catch (Exception exc) {
                    write(exc.Message);
                }
            }
        }

        public void Start() {
            LaunchTrain(SshRemoteFolder, sshScreenName, sshDarknet);
        }

        public void Refresh() {
            GetLogTail(SshRemoteFolder);
            GetChart();
        } 

        public void Kill() {
            write("Killing PID:" + ScreenPid);
            if (ScreenPid > 0) 
                ShowCommand("kill " + ScreenPid);
            else
                write("no valid pid to kill");
        }
    }
}
