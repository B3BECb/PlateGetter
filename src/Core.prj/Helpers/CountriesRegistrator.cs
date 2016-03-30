using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateGetter.Core.Helpers
{
	/// <summary>Регистратор стран.</summary>
	public static class CountriesRegistrator
	{
		/// <summary>Возвращает список доступных стран.</summary>
		public static List<Country> RegistredCountries { get; private set; }

		static CountriesRegistrator()
		{
			RegistredCountries = new List<Country>();

			RegistredCountries.Add(new Country("lithuania", "lt"));
			RegistredCountries.Add(new Country("poland", "pl"));
			RegistredCountries.Add(new Country("bulgaria", "bg"));
			RegistredCountries.Add(new Country("turkey", "tr"));
			RegistredCountries.Add(new Country("estonia", "ee"));
			RegistredCountries.Add(new Country("hungary", "hu"));
			RegistredCountries.Add(new Country("moldova", "md"));
			RegistredCountries.Add(new Country("czech republic", "cz"));

		}
	}
}
