using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Manager.MVVM.ViewModel
{
    public class CreateVMRandomMacViewModel : BindableBase
    {
        private string _macAddress;

        public CreateVMRandomMacViewModel()
        {
            GetMacAddressCommand = new DelegateCommand(OnGetMacAddress);
        }

        public ICommand GetMacAddressCommand { get; }

        public string MacAddress
        {
            get => _macAddress;
            set => SetProperty(ref _macAddress, value);
        }

        private async void OnGetMacAddress()
        {
            MacAddress = "Fetching...";
            await Task.Run(() => RunExeAndFetchMacAddress());
        }

        private void RunExeAndFetchMacAddress()
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "C:/Files/Manager/Mac Address/HEXWARE-Random-IntelMac.exe",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process process = Process.Start(startInfo))
                {
                    process.WaitForExit();
                    if (process.ExitCode == 0)
                    {
                        // Создание STA-потока для получения текста из буфера обмена
                        var staThread = new Thread(
                            () =>
                            {
                                try
                                {
                                    MacAddress = System.Windows.Clipboard.GetText();
                                }
                                catch (Exception ex)
                                {
                                    MacAddress = $"Error accessing clipboard: {ex.Message}";
                                }
                            });
                        staThread.SetApartmentState(ApartmentState.STA);
                        staThread.Start();
                        staThread.Join();
                    }
                    else
                    {
                        MacAddress = "Failed to fetch MAC address.";
                    }
                }
            }
            catch (Exception ex)
            {
                MacAddress = $"Error: {ex.Message}";
            }
        }
    }
}
