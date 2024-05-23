using System.Collections.Generic;
using System.ComponentModel;

namespace Manager.Core.Models
{
    public class Config
    {
        public List<VM> VMs { get; set; }
        public Schedule Schedule { get; set; }
        public GridConfig Grid { get; set; }
    }

    public class GridConfig : INotifyPropertyChanged
    {
        private int _rows;
        private int _columns;
        private int _cellWidth;
        private int _cellHeight;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public int Rows
        {
            get => _rows;
            set { _rows = value; OnPropertyChanged(nameof(Rows)); }
        }

        public int Columns
        {
            get => _columns;
            set { _columns = value; OnPropertyChanged(nameof(Columns)); }
        }

        public int CellWidth
        {
            get => _cellWidth;
            set { _cellWidth = value; OnPropertyChanged(nameof(CellWidth)); }
        }

        public int CellHeight
        {
            get => _cellHeight;
            set { _cellHeight = value; OnPropertyChanged(nameof(CellHeight)); }
        }
    }
}
