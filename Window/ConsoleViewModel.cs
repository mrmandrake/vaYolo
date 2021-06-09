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

namespace vaYolo.ViewModels
{
    public class ConsoleViewModel : ReactiveObject
    {
        ConnectionInfo conn;
        SshClient sshclient;
        SftpClient sftpclient;

        TextDocument document;
        public TextDocument Document
        {
            get => document;
            set => this.RaiseAndSetIfChanged(ref document, value);
        }
        
        string sshServer = Settings.SshServer;
        public string SshServer
        {
            get => sshServer;
            set => this.RaiseAndSetIfChanged(ref sshServer, value);
        }
        
        string sshPort = Convert.ToString(Settings.SshPort);
        public string SshPort
        {
            get => sshPort;
            set => this.RaiseAndSetIfChanged(ref sshPort, value);
        }
        
        string sshUsername = Settings.SshUsername;
        public string SshUsername
        {
            get => sshUsername;
            set => this.RaiseAndSetIfChanged(ref sshUsername, value);
        }
        string sshPassword = Settings.SshPassword;
        public string SshPassword
        {
            get => sshPassword;
            set => this.RaiseAndSetIfChanged(ref sshPassword, value);
        }
        
        string consoleInput = string.Empty;
        public string ConsoleInput
        {
            get => consoleInput;
            set => this.RaiseAndSetIfChanged(ref consoleInput, value);
        }

        ObservableCollection<string> consoleOutput = new ();

        public ObservableCollection<string> ConsoleOutput
        {
            get => consoleOutput;
            set => this.RaiseAndSetIfChanged(ref consoleOutput, value);
        }

        public ConsoleViewModel() {
            Document = new TextDocument();
        }

        public void write(string str) => Document.Insert(Document.TextLength, str + "\n");

        public bool Init()
        {
            write("Connecting to -> " + SshServer + ":" + SshPort);

            try
            {
                conn = new (SshServer, Convert.ToUInt16(SshPort), 
                    SshUsername,
                    new AuthenticationMethod[] {
                        new PasswordAuthenticationMethod(SshUsername, SshPassword)
                    }
                );

                sshclient = new SshClient(conn);
                sshclient.Connect();
                write("Connected!");
                // return true;
            }
            catch (Exception exc)
            {
                write(exc.Message);
            }

            return true;
        }

        public void Finish()
        {
            if (sshclient != null && 
                sshclient.IsConnected) {
                write("Disconnect...");
                sshclient.Disconnect();
                sshclient.Dispose();
                write("Disconnected!");
            }
        }

        public void RunCommand(string cmd)
        {
            if (sshclient == null) {
                write("client not connected!!!");
                return;
            }

            try {
                using (var _cmd = sshclient.CreateCommand(cmd))
                {
                    var result = _cmd.BeginExecute();
                    using (var reader = new StreamReader(_cmd.ExtendedOutputStream, Encoding.UTF8, true, 1024, true))
                    {
                        while (!result.IsCompleted || !reader.EndOfStream) {
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
            catch (Exception exc) {
                write(exc.Message);
            }   
        }

        public void SftpCopy(string folder) {
            var sftp = new SftpClient(conn);
            string uploadfn = "Renci.SshNet.dll";
            sftp.Connect();
            sftp.ChangeDirectory("/tmp/uploadtest");
            using (var uplfileStream = System.IO.File.OpenRead(uploadfn))
                sftp.UploadFile(uplfileStream, uploadfn, true);

            sftp.Disconnect();
        }

        private bool SyncTo(string dir, string remote, string parentPath)
        {
            using (var sftp = new SftpClient(conn))
            {
                sftp.Connect();
                if (!sftp.IsConnected)
                    return false;

                var destination = Path.Combine(remote, Path.GetRelativePath(parentPath, dir));

                // Try to create a remote directory. If it throws an exception, we will assume
                // for now that the directory already exists. See https://github.com/sshnet/SSH.NET/issues/25
                try {
                    sftp.CreateDirectory(destination);
                }
                catch (SshException exc)
                {
                    // Do nothing, as this is when the directory already exists
                }

                foreach (var file in sftp.SynchronizeDirectories(dir, destination, "*")) {
                    // Log.LogMessage("Updating " + file.Name);
                }

                return true;
            }
        }           
    }
}
