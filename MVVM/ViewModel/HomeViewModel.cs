using System.Diagnostics;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;

namespace Manager.MVVM.ViewModel
{
    class HomeViewModel : BindableBase
    {
        public ICommand OpenGitHubCommand { get; }
        public ICommand OpenSpaceProxyCommand { get; }
        public ICommand OpenGoogleSheetsCommand { get; }
        public ICommand OpenDiskDVmsCommand { get; }
        public ICommand OpenDiskCVmsCommand { get; }
        public ICommand OpenConfigsCommand { get; }
        public ICommand OpenHddBlankCommand { get; }
        public ICommand OpenSyncFolderCommand { get; }
        public ICommand OpenAida64Command { get; }
        public ICommand OpenDaemonToolsCommand { get; }

        public HomeViewModel()
        {
            OpenGitHubCommand = new DelegateCommand(OpenGitHub);
            OpenSpaceProxyCommand = new DelegateCommand(OpenSpaceProxy);
            OpenGoogleSheetsCommand = new DelegateCommand(OpenGoogleSheets);
            OpenDiskDVmsCommand = new DelegateCommand(OpenDiskDVms);
            OpenDiskCVmsCommand = new DelegateCommand(OpenDiskCVms);
            OpenConfigsCommand = new DelegateCommand(OpenConfigs);
            OpenHddBlankCommand = new DelegateCommand(OpenHddBlank);
            OpenSyncFolderCommand = new DelegateCommand(OpenSyncFolder);
            OpenAida64Command = new DelegateCommand(OpenAida64);
            OpenDaemonToolsCommand = new DelegateCommand(OpenDaemonTools);
        }

        private void OpenGitHub()
        {
            Process.Start(new ProcessStartInfo("https://github.com/Stormsil?tab=repositories") { UseShellExecute = true });
        }

        private void OpenSpaceProxy()
        {
            Process.Start(new ProcessStartInfo("https://panel.spaceproxy.net/") { UseShellExecute = true });
        }

        private void OpenGoogleSheets()
        {
            Process.Start(new ProcessStartInfo("https://docs.google.com/spreadsheets/d/1CJC8VjXbftoKg99AD7wGVslkFTitOgtX3X6uuMKAANc/edit?usp=sharing") { UseShellExecute = true });
        }

        private void OpenDiskDVms()
        {
            Process.Start(new ProcessStartInfo("explorer.exe", "D:\\Disk D VMs") { UseShellExecute = true });
        }

        private void OpenDiskCVms()
        {
            Process.Start(new ProcessStartInfo("explorer.exe", "C:\\Disk C VMs") { UseShellExecute = true });
        }

        private void OpenConfigs()
        {
            Process.Start(new ProcessStartInfo("explorer.exe", "C:\\Files\\VM Files\\Configs") { UseShellExecute = true });
        }

        private void OpenHddBlank()
        {
            Process.Start(new ProcessStartInfo("explorer.exe", "C:\\Files\\VM Files\\HDD_BLANK") { UseShellExecute = true });
        }

        private void OpenSyncFolder()
        {
            Process.Start(new ProcessStartInfo("explorer.exe", "C:\\Files\\Sync") { UseShellExecute = true });
        }

        private void OpenAida64()
        {
            Process.Start(new ProcessStartInfo("C:\\Program Files (x86)\\AIDA64\\aida64.exe") { UseShellExecute = true });
        }

        private void OpenDaemonTools()
        {
            Process.Start(new ProcessStartInfo("C:\\Program Files\\DAEMON Tools Lite\\DTLauncher.exe") { UseShellExecute = true });
        }
    }
}
