using System.Linq;
using System.Collections.Generic;
using ReactiveUI;
using vaYolo.Model.Yolo;

namespace vaYolo.ViewModels
{
    public class SetupViewModel : ReactiveObject
    {
        int networkWidth = 416;
        public int NetworkWidth
        {
            get => networkWidth;
            set => this.RaiseAndSetIfChanged(ref networkWidth, value);
        }

        int networkHeight = 416;
        public int NetworkHeight
        {
            get => networkHeight;
            set => this.RaiseAndSetIfChanged(ref networkHeight, value);
        }

        int batchSize = 64;
        public int BatchSize
        {
            get => batchSize;
            set => this.RaiseAndSetIfChanged(ref batchSize, value);
        }

        int maxBatches = 6000;
        public int MaxBatches
        {
            get => maxBatches;
            set => this.RaiseAndSetIfChanged(ref maxBatches, value);
        }

        int subdivision = 16;
        public int Subdivision
        {
            get => subdivision;
            set => this.RaiseAndSetIfChanged(ref subdivision, value);
        }

        int imageDecodeWidth = Settings.Get().ImageDecodeWidth;
        public int ImageDecodeWidth
        {
            get => imageDecodeWidth;
            set => this.RaiseAndSetIfChanged(ref imageDecodeWidth, value);
        }

        int gaugeDim = Settings.Get().GaugeSize;
        public int GaugeDim
        {
            get => gaugeDim;
            set => this.RaiseAndSetIfChanged(ref gaugeDim, value);
        }

        string darknetDir = Settings.Get().SshRemoteDarknet;
        public string DarknetDir
        {
            get => darknetDir;
            set => this.RaiseAndSetIfChanged(ref darknetDir, value);
        }

        string sshServer = Settings.Get().SshServer;
        public string SshServer
        {
            get => sshServer;
            set => this.RaiseAndSetIfChanged(ref sshServer, value);
        }

        int sshPort = Settings.Get().SshPort;
        public int SshPort
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

        public string Folder { get; set; }

        public List<string> Templates { get; set; }

        public SetupViewModel(string folder)
        {
            Templates = Config.Configs.Keys.ToList();
            Folder = folder;
        }

        public void Save()
        {
            Settings.Get().SshServer = SshServer;
            Settings.Get().SshPort = SshPort;
            Settings.Get().SshUsername = sshUsername;
            Settings.Get().SshRemoteDarknet= darknetDir;
            Settings.Get().SshPassword = SshPassword;
            Settings.Get().NetworkHeight = NetworkHeight;
            Settings.Get().NetworkWidth= NetworkWidth;
            Settings.Get().BatchSize = BatchSize;
            Settings.Get().MaxBatches = MaxBatches;
            Settings.Get().Subdivision = Subdivision;
            Settings.Get().Save(Folder);
        }
    }
}
