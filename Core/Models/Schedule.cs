using System;
using System.ComponentModel;

namespace Manager.Core.Models
{
    public class Schedule : INotifyPropertyChanged
    {
        private DateTime _dayStart1;
        private DateTime _dayEnd1;
        private DateTime _dayStart2;
        private DateTime _dayEnd2;
        private DateTime _nightStart1;
        private DateTime _nightEnd1;
        private DateTime _nightStart2;
        private DateTime _nightEnd2;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public DateTime DayStart1
        {
            get => _dayStart1;
            set { _dayStart1 = value; OnPropertyChanged(nameof(DayStart1)); }
        }

        public DateTime DayEnd1
        {
            get => _dayEnd1;
            set { _dayEnd1 = value; OnPropertyChanged(nameof(DayEnd1)); }
        }

        public DateTime DayStart2
        {
            get => _dayStart2;
            set { _dayStart2 = value; OnPropertyChanged(nameof(DayStart2)); }
        }

        public DateTime DayEnd2
        {
            get => _dayEnd2;
            set { _dayEnd2 = value; OnPropertyChanged(nameof(DayEnd2)); }
        }

        public DateTime NightStart1
        {
            get => _nightStart1;
            set { _nightStart1 = value; OnPropertyChanged(nameof(NightStart1)); }
        }

        public DateTime NightEnd1
        {
            get => _nightEnd1;
            set { _nightEnd1 = value; OnPropertyChanged(nameof(NightEnd1)); }
        }

        public DateTime NightStart2
        {
            get => _nightStart2;
            set { _nightStart2 = value; OnPropertyChanged(nameof(NightStart2)); }
        }

        public DateTime NightEnd2
        {
            get => _nightEnd2;
            set { _nightEnd2 = value; OnPropertyChanged(nameof(NightEnd2)); }
        }
    }
}
