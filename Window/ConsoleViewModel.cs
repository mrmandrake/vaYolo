﻿using System;
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

            if (Ssh.Init(sshServer, 
                    Convert.ToUInt16(sshPort), 
                    sshUsername, 
                    sshPassword))
                    write("Connected!");

            ScreenPid = GetScreenPid();            
            write("Remote Folder " + (!Sftp.Exists(Ssh.Connection, SshRemoteFolder) ? "NOT" : "") + " exist!");
            var (s1, s2) = Sftp.Sync(Ssh.Connection, SshLocalFolder, SshRemoteFolder);
            if (s1)
                s2.ForEach((f) => write("updaload file" + f));
            return true;
        }

        public void Finish()
        {
            write("Disconnect...");            
            Ssh.Finish();
            write("Disconnected!");

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
                        write(String.Format("Found screen active with pid {0}", pid));
                    else
                        write("No active screen found");
                }
                else
                    write("Error analyzing re");
            }

            return pid;
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
            // var cmd = setfolder + screencmd + timecmd + darknetcmd + dataPath + cfgPath + redirectStd;
            var cmd = setfolder + screencmd + test;
            write("run " + cmd);
            write(Ssh.Run(cmd));
        }

        private void GetLogTail(string remote, int lines = 30) {
            write(Ssh.Run(String.Format("tail -n {0} {1} ", lines, Path.Combine(SshRemoteFolder,"screenlog.0"))));
        }

        private void GetChart()
        {
            if (Sftp.Download(Ssh.Connection, sshRemoteFolder, sshLocalFolder, "chart.png")) {
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
            ScreenPid = GetScreenPid();
        }

        public void Refresh() {
            Document = new TextDocument();
            GetLogTail(SshRemoteFolder);
            GetChart();
        } 

        public void Kill() {
            write("killing pid:" + ScreenPid);
            Ssh.Kill(ScreenPid);
            write("killed");
            ScreenPid = -1;
        }

        public void Run(string cmd) {
            write(Ssh.Run(cmd));
        }
    }
}
