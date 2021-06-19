using System;
using System.IO;
using System.Text;
using ReactiveUI;
using Renci.SshNet;

namespace vaYolo.Helpers
{
    public class Ssh
    {
        static ConnectionInfo? sshConn { get; set; } = null;
        static SshClient sshclient { get; set; } = null;

        public static ConnectionInfo? Connection
        {
            get { 
                if (sshConn == null) 
                    sshConn = new(Settings.Get().SshServer, 
                                Convert.ToUInt16(Settings.Get().SshPort),
                                Settings.Get().SshUsername,
                                new AuthenticationMethod[] {
                                    new PasswordAuthenticationMethod(Settings.Get().SshUsername, 
                                        Settings.Get().SshPassword)}) {
                    Timeout = new TimeSpan(0, 0, 5)
                };
                
                return sshConn; 
            }
        }

        public static bool Init()
        {
            try
            {
                sshclient = new SshClient(Connection);
                sshclient.Connect();
                return true;

            }
            catch (Exception exc)
            {
                // write(exc.Message);
            }

            return false;
        }

        public static void Finish()
        {
            if (sshclient != null &&
                sshclient.IsConnected)
                sshclient.Disconnect();
        }

        public static string Run(string cmd)
        {
            if (sshclient == null || !sshclient.IsConnected)
                return string.Empty;

            string str = "";
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
                                str += line;
                        }

                        if (_cmd.Result != null)
                            str += _cmd.Result;
                    }

                    _cmd.EndExecute(result);
                }
            }
            catch (Exception exc)
            {
                // write(exc.Message);
            }

            return str;
        }

        public static string? GetCmdResult(string cmd)
        {
            if (sshclient == null || !sshclient.IsConnected)
                return string.Empty;

            try
            {
                using (var _cmd = sshclient.CreateCommand(cmd))
                {
                    var asres = _cmd.BeginExecute();
                    using (var reader = new StreamReader(_cmd.ExtendedOutputStream, Encoding.UTF8, true, 1024, true))
                    {
                        while (!asres.IsCompleted || !reader.EndOfStream)
                            reader.ReadLine();

                        _cmd.EndExecute(asres);
                        return _cmd.Result;
                    }
                }
            }
            catch (Exception exc)
            {
                // write(exc.Message);
            }

            return string.Empty;
        }
        public static void Kill(int pid)
        {
            Run("kill " + pid);
        }
    }
}