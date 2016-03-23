using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateGetter
{
	/// <summary>Регистратор стран.</summary>
	public static class CountriesRegistrator
	{
		/// <summary>Возвращает список доступных стран.</summary>
		public static List<Country> RegistredCountries { get; private set; }

		static CountriesRegistrator()
		{
			RegistredCountries = new List<Country>();

			RegistredCountries.Add(new Country("Казахстан", "kz"));
		}
	}
}
