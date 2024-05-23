using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Manager.Core.Models;
using Newtonsoft.Json;

namespace Manager.Core
{
    public class SchedulerService
    {
        private Schedule _schedule;
        private Manager.Core.Models.GridConfig _gridConfig;
        private readonly string _configFilePath = @"C:\Files\Manager\Configs\config.json";

        public SchedulerService()
        {
            LoadConfig();
        }

        public List<Schedule> GetSchedules()
        {
            return new List<Schedule> { _schedule };
        }

        public Manager.Core.Models.GridConfig GetGridConfig()
        {
            return _gridConfig;
        }

        public void SaveSchedules(List<Schedule> schedules)
        {
            _schedule = schedules.FirstOrDefault();
            SaveConfig();
        }

        public void SaveGridConfig(Manager.Core.Models.GridConfig gridConfig)
        {
            _gridConfig = gridConfig;
            SaveConfig();
        }

        public void SaveConfig()
        {
            Config currentConfig = null;

            if (File.Exists(_configFilePath))
            {
                var jsonData = File.ReadAllText(_configFilePath);
                var jsonDeserializeSettings = new JsonSerializerSettings
                {
                    DateFormatString = "HH:mm"
                };
                currentConfig = JsonConvert.DeserializeObject<Config>(jsonData, jsonDeserializeSettings);
            }

            if (currentConfig == null)
            {
                currentConfig = new Config();
            }

            currentConfig.Schedule = _schedule;
            currentConfig.Grid = _gridConfig;

            var jsonSerializeSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                DateFormatString = "HH:mm"
            };

            var jsonSerialized = JsonConvert.SerializeObject(currentConfig, jsonSerializeSettings);
            File.WriteAllText(_configFilePath, jsonSerialized);
        }

        public Config LoadConfig()
        {
            if (File.Exists(_configFilePath))
            {
                var jsonData = File.ReadAllText(_configFilePath);
                var jsonDeserializeSettings = new JsonSerializerSettings
                {
                    DateFormatString = "HH:mm"
                };
                var config = JsonConvert.DeserializeObject<Config>(jsonData, jsonDeserializeSettings);
                _schedule = config?.Schedule ?? new Schedule();
                _gridConfig = config?.Grid ?? new Manager.Core.Models.GridConfig();
                return config;
            }
            else
            {
                _schedule = new Schedule();
                _gridConfig = new Manager.Core.Models.GridConfig();
                return new Config { Schedule = _schedule, Grid = _gridConfig };
            }
        }

        public DateTime GetNextRotationTime()
        {
            DateTime now = DateTime.Now;
            List<DateTime> times = new List<DateTime>
            {
                _schedule.DayStart1, _schedule.DayEnd1, _schedule.DayStart2, _schedule.DayEnd2,
                _schedule.NightStart1, _schedule.NightEnd1, _schedule.NightStart2, _schedule.NightEnd2
            };

            DateTime nextRotation = times.Where(t => t > now).OrderBy(t => t).FirstOrDefault();
            return nextRotation != default(DateTime) ? nextRotation : times.OrderBy(t => t).First();
        }

        public string GetCurrentShift()
        {
            DateTime now = DateTime.Now;
            if ((now >= _schedule.DayStart1 && now < _schedule.DayEnd1) ||
                (now >= _schedule.DayStart2 && now < _schedule.DayEnd2))
            {
                return "Day";
            }
            else if ((now >= _schedule.NightStart1 && now < _schedule.NightEnd1) ||
                     (now >= _schedule.NightStart2 && now < _schedule.NightEnd2))
            {
                return "Night";
            }
            else
            {
                return "Unknown";
            }
        }
    }
}
