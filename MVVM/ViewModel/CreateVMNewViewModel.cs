﻿using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Threading.Tasks;
using Manager.Core;
using Manager.MVVM.ViewModel;

namespace Manager.MVVM.ViewModel
{
    public class CreateVMNewViewModel : BindableBase
    {
        private readonly CreateVMService _createVMService;
        private readonly CreateVMConsoleViewModel _consoleViewModel;

        public CreateVMNewViewModel(CreateVMService createVMService, CreateVMConsoleViewModel consoleViewModel)
        {
            _createVMService = createVMService;
            _consoleViewModel = consoleViewModel;
            CreateVMCommand = new DelegateCommand(async () => await CreateVM(VMName));
            UpdateDiskUsage();
            UpdateConfigCount();
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

        private int _configCount;
        public int ConfigCount
        {
            get => _configCount;
            set => SetProperty(ref _configCount, value);
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
                    _consoleViewModel.AppendToConsole("Начало создания VM...");
                    await _createVMService.CreateVMAsync(vmName, (step, description) =>
                    {
                        UpdateProgress(step, description);
                        _consoleViewModel.AppendToConsole(description);
                    });
                    _consoleViewModel.AppendToConsole("Создание VM завершено.");
                }
                catch (Exception ex)
                {
                    _consoleViewModel.AppendToConsole($"Ошибка при создании VM: {ex.Message}");
                }
                finally
                {
                    // Обновить использование диска после создания VM
                    UpdateDiskUsage();
                    UpdateConfigCount(); // обновить количество конфигов
                }
            }
            else
            {
                _consoleViewModel.AppendToConsole("Некорректное имя VM.");
            }
        }

        public void SetVmDirectory(string selectedDisk)
        {
            try
            {
                _createVMService.SetVmDirectory(selectedDisk);
                _consoleViewModel.AppendToConsole($"Выбран диск: {selectedDisk}");
            }
            catch (Exception ex)
            {
                _consoleViewModel.AppendToConsole($"Ошибка при выборе диска: {ex.Message}");
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

        private void UpdateConfigCount()
        {
            ConfigCount = _createVMService.GetConfigCount();
        }
    }
}
