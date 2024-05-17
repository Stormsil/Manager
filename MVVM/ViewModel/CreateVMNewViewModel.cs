using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Threading.Tasks;
using Manager.Core;

namespace Manager.MVVM.ViewModel
{
    public class CreateVMNewViewModel : BindableBase
    {
        private readonly CreateVMService _createVMService;

        public CreateVMNewViewModel(CreateVMService createVMService)
        {
            _createVMService = createVMService;
            CreateVMCommand = new DelegateCommand(async () => await CreateVM(VMName));
            UpdateDiskUsage();
        }

        private string _selectedDisk;
        public string SelectedDisk
        {
            get => _selectedDisk;
            set
            {
                if (SetProperty(ref _selectedDisk, value))
                {
                    var cleanedDisk = value?.Replace(":", "").Trim();
                    SetVmDirectory(cleanedDisk);
                }
            }
        }

        private string _vmName;
        public string VMName
        {
            get => _vmName;
            set => SetProperty(ref _vmName, value);
        }

        private double _progressValue;
        public double ProgressValue
        {
            get => _progressValue;
            set => SetProperty(ref _progressValue, value);
        }

        private string _progressText;
        public string ProgressText
        {
            get => _progressText;
            set => SetProperty(ref _progressText, value);
        }

        private string _diskCInfo;
        public string DiskCInfo
        {
            get => _diskCInfo;
            set => SetProperty(ref _diskCInfo, value);
        }

        private double _diskCProgress;
        public double DiskCProgress
        {
            get => _diskCProgress;
            set => SetProperty(ref _diskCProgress, value);
        }

        private string _diskDInfo;
        public string DiskDInfo
        {
            get => _diskDInfo;
            set => SetProperty(ref _diskDInfo, value);
        }

        private double _diskDProgress;
        public double DiskDProgress
        {
            get => _diskDProgress;
            set => SetProperty(ref _diskDProgress, value);
        }

        private bool _isProgressBarVisible;
        public bool IsProgressBarVisible
        {
            get => _isProgressBarVisible;
            set => SetProperty(ref _isProgressBarVisible, value);
        }

        public DelegateCommand CreateVMCommand { get; }

        public async Task CreateVM(string vmName)
        {
            if (!string.IsNullOrWhiteSpace(vmName))
            {
                try
                {
                    ProgressValue = 0;
                    IsProgressBarVisible = true;
                    await _createVMService.CreateVMAsync(vmName, UpdateProgress);
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    // Обновить использование диска после создания VM
                    UpdateDiskUsage();
                }
            }
            else
            {
            }
        }

        public void SetVmDirectory(string selectedDisk)
        {
            try
            {
                _createVMService.SetVmDirectory(selectedDisk);
            }
            catch (Exception ex)
            {
            }
        }

        private void UpdateProgress(double step, string description)
        {
            ProgressValue += step;
            ProgressText = description;
        }

        private void UpdateDiskUsage()
        {
            var diskUsage = _createVMService.GetDiskUsage();
            DiskCProgress = diskUsage.CDiskUsagePercentage;
            DiskCInfo = diskUsage.CDiskUsageInfo;
            DiskDProgress = diskUsage.DDiskUsagePercentage;
            DiskDInfo = diskUsage.DDiskUsageInfo;
        }
    }
}
