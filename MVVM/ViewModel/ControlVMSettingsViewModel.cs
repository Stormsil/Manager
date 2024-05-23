using Manager.Core;
using Manager.Core.Models;
using System;
using System.Linq;
using System.Windows.Input;

namespace Manager.MVVM.ViewModel
{
    public class ControlVMSettingsViewModel : ObservableObject
    {
        private readonly SchedulerService _schedulerService;
        private readonly VMService _vmService;
        private string _daySchedule;
        private string _nightSchedule;
        private Manager.Core.Models.GridConfig _gridConfig;

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

        public Manager.Core.Models.GridConfig GridConfig
        {
            get => _gridConfig;
            set
            {
                _gridConfig = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveSettingsCommand { get; }
        public ICommand OpenConfigCommand { get; }
        public ICommand OpenVMConfigCommand { get; }

        public ControlVMSettingsViewModel(SchedulerService schedulerService, VMService vmService)
        {
            _schedulerService = schedulerService;
            _vmService = vmService;

            LoadSettings();

            SaveSettingsCommand = new RelayCommand(_ => SaveSettings());
            OpenConfigCommand = new RelayCommand(_ => OpenConfig());
            OpenVMConfigCommand = new RelayCommand(_ => OpenVMConfig());
        }

        private void LoadSettings()
        {
            var config = _schedulerService.LoadConfig();
            var schedule = config.Schedule;

            if (schedule != null)
            {
                DaySchedule = $"{schedule.DayStart1:HH:mm},{schedule.DayEnd1:HH:mm},{schedule.DayStart2:HH:mm},{schedule.DayEnd2:HH:mm}";
                NightSchedule = $"{schedule.NightStart1:HH:mm},{schedule.NightEnd1:HH:mm},{schedule.NightStart2:HH:mm},{schedule.NightEnd2:HH:mm}";
            }

            GridConfig = config.Grid ?? new Manager.Core.Models.GridConfig();
        }

        private void SaveSettings()
        {
            var schedule = _schedulerService.GetSchedules().FirstOrDefault();
            if (schedule != null)
            {
                try
                {
                    var dayTimes = DaySchedule.Split(',');
                    var nightTimes = NightSchedule.Split(',');

                    if (dayTimes.Length != 4 || nightTimes.Length != 4)
                    {
                        System.Windows.MessageBox.Show("Некорректное количество значений времени. Должно быть 4 для дня и 4 для ночи.");
                        return;
                    }

                    schedule.DayStart1 = DateTime.ParseExact(dayTimes[0], "HH:mm", null);
                    schedule.DayEnd1 = DateTime.ParseExact(dayTimes[1], "HH:mm", null);
                    schedule.DayStart2 = DateTime.ParseExact(dayTimes[2], "HH:mm", null);
                    schedule.DayEnd2 = DateTime.ParseExact(dayTimes[3], "HH:mm", null);

                    schedule.NightStart1 = DateTime.ParseExact(nightTimes[0], "HH:mm", null);
                    schedule.NightEnd1 = DateTime.ParseExact(nightTimes[1], "HH:mm", null);
                    schedule.NightStart2 = DateTime.ParseExact(nightTimes[2], "HH:mm", null);
                    schedule.NightEnd2 = DateTime.ParseExact(nightTimes[3], "HH:mm", null);

                    _schedulerService.SaveSchedules(new List<Schedule> { schedule });
                }
                catch (FormatException)
                {
                    System.Windows.MessageBox.Show("Неправильный формат времени. Пожалуйста, используйте HH:mm.");
                    return;
                }
            }

            _schedulerService.SaveGridConfig(_gridConfig);
        }

        private void OpenConfig()
        {
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = @"C:\Files\Manager\Configs\config.json",
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка при открытии конфига: {ex.Message}");
            }
        }

        private void OpenVMConfig()
        {
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = @"C:\Files\Manager\Configs\configVM.json",
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка при открытии VM конфига: {ex.Message}");
            }
        }
    }
}
