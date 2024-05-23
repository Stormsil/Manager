using Manager.Core;
using Manager.Core.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace Manager.MVVM.ViewModel
{
    public class ControlVMHomeViewModel : ObservableObject
    {
        private readonly VMService _vmService;
        private readonly SchedulerService _schedulerService;
        private ObservableCollection<VM> _vms;
        private string _currentShift;
        private string _nextRotationTime;
        private string _daySchedule;
        private string _nightSchedule;
        private DispatcherTimer _timer;
        private bool _isAutoRotationEnabled;
        private bool _isRotationInProgress;

        public ObservableCollection<VM> VMs
        {
            get => _vms;
            set
            {
                _vms = value;
                OnPropertyChanged();
            }
        }

        public string CurrentShift
        {
            get => _currentShift;
            set
            {
                _currentShift = value;
                OnPropertyChanged();
            }
        }

        public string NextRotationTime
        {
            get => _nextRotationTime;
            set
            {
                _nextRotationTime = value;
                OnPropertyChanged();
            }
        }

        public string DaySchedule
        {
            get => _daySchedule;
            set
            {
                _daySchedule = value;
                OnPropertyChanged();
            }
        }

        public string NightSchedule
        {
            get => _nightSchedule;
            set
            {
                _nightSchedule = value;
                OnPropertyChanged();
            }
        }

        public bool IsAutoRotationEnabled
        {
            get => _isAutoRotationEnabled;
            set
            {
                _isAutoRotationEnabled = value;
                OnPropertyChanged();
                ToggleRotation();
            }
        }

        public ICommand RefreshCommand { get; }
        public ICommand ArrangeWindowsCommand { get; }
        public ICommand ToggleRotationCommand { get; }

        public ControlVMHomeViewModel(VMService vmService, SchedulerService schedulerService)
        {
            _vmService = vmService;
            _schedulerService = schedulerService;

            VMs = new ObservableCollection<VM>(_vmService.GetVMs());
            RefreshSchedules();
            CurrentShift = _schedulerService.GetCurrentShift();
            NextRotationTime = CalculateTimeToNextRotation();

            RefreshCommand = new RelayCommand(async _ => await RefreshDataAsync());
            ArrangeWindowsCommand = new RelayCommand(_ => ArrangeWindows());
            ToggleRotationCommand = new RelayCommand(_ => ToggleRotation());

            StartTimer();
        }

        private void StartTimer()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(5); // Check every minute
            _timer.Tick += async (sender, args) => await RefreshDataAsync();
            _timer.Start();
        }

        private string CalculateTimeToNextRotation()
        {
            var nextRotation = _schedulerService.GetNextRotationTime();
            var timeSpan = nextRotation - DateTime.Now;
            return $"{(int)timeSpan.TotalMinutes} минут";
        }

        private void RefreshSchedules()
        {
            _schedulerService.LoadConfig();  // Ensure the latest config is loaded
            var schedules = _schedulerService.GetSchedules().FirstOrDefault();
            DaySchedule = FormatScheduleString(schedules.DayStart1, schedules.DayEnd1, schedules.DayStart2, schedules.DayEnd2);
            NightSchedule = FormatScheduleString(schedules.NightStart1, schedules.NightEnd1, schedules.NightStart2, schedules.NightEnd2);
        }

        private string FormatScheduleString(DateTime start1, DateTime end1, DateTime start2, DateTime end2)
        {
            return $"{start1:HH:mm} - {end1:HH:mm}, {start2:HH:mm} - {end2:HH:mm}";
        }

        private async Task RefreshDataAsync()
        {
            var vms = _vmService.GetVMs();
            var currentShift = _schedulerService.GetCurrentShift();

            foreach (var vm in vms)
            {
                vm.OnlineStatus = vm.Shift == currentShift && vm.Status == "Active" ? "Online" : "Offline";
            }

            VMs = new ObservableCollection<VM>(vms);
            RefreshSchedules();  // Ensure schedules are refreshed
            CurrentShift = _schedulerService.GetCurrentShift();
            NextRotationTime = CalculateTimeToNextRotation();

            if (_isAutoRotationEnabled && !_isRotationInProgress)
            {
                _isRotationInProgress = true;
                await _vmService.ManageVMsAsync(CurrentShift);
                _isRotationInProgress = false;
            }
        }

        private void ArrangeWindows()
        {
            _vmService.ArrangeWindows();
        }

        private void ToggleRotation()
        {
            if (_isAutoRotationEnabled)
            {
                _timer.Start();
            }
            else
            {
                _timer.Stop();
            }
        }
    }
}
