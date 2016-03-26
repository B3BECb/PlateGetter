using PlateGetter.Core;
using PlateGetter.Core.Helpers;
using System;
using System.ComponentModel;

namespace PlateGetter.Settings
{
	internal sealed class SettingsViewModel
	{
		#region Properties

		public BindingList<Country> Countries { get; private set; }

		public int StartPageNumber { get; set; }

		public int EndPageNumber { get; set; }

		#endregion


		#region Data

		private ProgrammSettings _settings;

		private Country _selectedCountry;

		#endregion


		#region .ctor

		public SettingsViewModel(ProgrammSettings settings)
		{
			_settings = settings;

			StartPageNumber = _settings.StartPageNumber;

			EndPageNumber = _settings.StartPageNumber - _settings.EndPageNumber;

			Countries = new BindingList<Country>(_settings.CountriesList);
		}

		#endregion


		#region Public methods

		/// <summary>Сохраняет выбранную страну.</summary>
		/// <param name="">Выбранная страна.</param>
		public void SaveSelectedCountry(Country country)
		{
			_selectedCountry = country;
		}

		internal void Apply()
		{
			_settings.EndPageNumber = EndPageNumber;
			_settings.StartPageNumber = StartPageNumber;
			_settings.SelectedCountry = _selectedCountry;
		}

		#endregion
	}
}