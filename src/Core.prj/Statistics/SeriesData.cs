using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateGetter.Core.Statistics
{
	public class SeriesData
	{
		public string SeriesDisplayName { get; set; }

		public string SeriesDescription { get; set; }

		public ObservableCollection<Plate> Items { get; set; }
	}
}
