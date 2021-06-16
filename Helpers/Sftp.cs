using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Renci.SshNet;
using Renci.SshNet.Common;

namespace vaYolo.Helpers {
    public class Sftp {
        private static List<string> lstExt = new()
        {
            "*.png",
            "*.jpg",
            "*.txt",
            "*.cfg",
            "*.names",
            "*.data"
        };

        public static bool Download(ConnectionInfo conn, string SshRemoteFolder, string SshLocalFolder, string filename)
        {
            try
            {
                using (var sftp = new SftpClient(conn))
                {
                    sftp.Connect();
                    sftp.ChangeDirectory(SshRemoteFolder);
                    using (Stream f = File.OpenWrite(Path.Combine(SshLocalFolder, filename))) {
                        // write(String.Format("Downloading {0} from {1} in {2}", 
                        //     filename, 
                        //     SshRemoteFolder, 
                        //     SshLocalFolder));

                        sftp.DownloadFile(filename, f);
                    }

                    sftp.Disconnect();
                }
                return true;
            }
            catch (Exception exc)
            {
                // write (exc.Message);
            }

            return false;
        }

        public static bool Upload(ConnectionInfo conn, string SshRemoteFolder, string SshLocalFolder, string filename)
        {
            try
            {
                using (var sftp = new SftpClient(conn))
                {
                    sftp.Connect();
                    // sftp.ChangeDirectory(SshRemoteFolder);
                    using (Stream f = new FileStream(Path.Combine(SshLocalFolder, filename), FileMode.Open))
                        sftp.UploadFile(f, Path.Combine(SshRemoteFolder, filename).Replace(@"\", "/"), true);

                    sftp.Disconnect();
                }
                return true;
            }
            catch (Exception exc)
            {
                // write (exc.Message);
            }

            return false;
        }


        public static bool Exists(ConnectionInfo conn, string path)
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
                    // write(exc.Message);
                }
            }

            return result;
        }

        public static (bool, List<FileInfo>) Sync(ConnectionInfo conn, string local, string remote)
        {
            List<FileInfo> syncs = new ();
            using (var sftp = new SftpClient(conn))
            {
                try {
                    sftp.Connect();
                    if (!sftp.IsConnected)
                        return (false, syncs);

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
                        syncs.AddRange(sftp.SynchronizeDirectories(local, remote, ext).ToList())
                    );

                }
                catch (Exception exc)
                {
                    // write(exc.Message);
                }


                return (true, syncs);
            }

            return (false, syncs);
        }
    }
}