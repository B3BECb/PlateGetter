using PlateGetter.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateGetter.Core
{
	public sealed class ProgrammSettings
	{
		#region Properties

		/// <summary>Возвращает и задаёт выбранную страну.</summary>
		public Country SelectedCountry { get; set; }

		/// <summary>Возвращает и задаёт номер начальной страницы с фотографией.</summary>
		public int StartPageNumber { get; set; }

		/// <summary>Возвращает и задаёт номер конечной страницы с фотографией.</summary>
		public int EndPageNumber { get; set; }

		/// <summary>Возвращает и задаёт список доступных стран.</summary>
		public List<Country> CountriesList => CountriesRegistrator.RegistredCountries;

		#endregion


		#region Data

		#endregion


		#region .ctor
		
		public ProgrammSettings()
		{
			SelectedCountry = CountriesList.FirstOrDefault();

			StartPageNumber = 1000;

			EndPageNumber = 0;
		}

		#endregion

	}
}
