using System;
using System.Collections.Generic;

namespace PlateGetter.Core.Statistics
{
	public class CountryStatistic
	{
		public List<Plate> Templates { get; }
		public Dictionary<char, int> Letters { get; }

		public CountryStatistic(List<Plate> templates, Dictionary<char, int> letters)
		{
			Templates = templates;
			Letters = letters;
		}
	}
}
