using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApplication.Shared
{
    public class SeriesData
    {
        public string SeriesDisplayName { get; set; }

        public string SeriesDescription { get; set; }

        public ObservableCollection<Plate> Items { get; set; }
    }

    public class Plate : INotifyPropertyChanged
    {
        public string Category { get; set; }

        private float _number = 0;
        public float Number 
        {
            get
            {
                return _number;
            }
            set
            {
                _number = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs(nameof(Number)));
                }
            }

        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
