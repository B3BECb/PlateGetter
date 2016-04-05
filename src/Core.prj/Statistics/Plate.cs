using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateGetter.Core.Statistics
{
	public class Plate : INotifyPropertyChanged
	{
		public string Category { get; set; }

		private int _number;
		public int Number
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

		public Plate(string plateMask, int foundedPlates = 1)
		{
			_number = foundedPlates;
			Category = plateMask;
		}

		public Plate() { }

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
