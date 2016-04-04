using PlateGetter.Core;
using PlateGetter.Core.Helpers;
using PlateGetter.Core.Logger;
using System;
using System.ComponentModel;

namespace PlateGetter.Settings
{
	internal sealed class SettingsViewModel
	{
		#region Properties

		public BindingList<Country> Countries { get; private set; }

		public int StartPageNumber { get; set; }

		public int DownloadPages { get; set; }

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

			DownloadPages = _settings.DownloadPages;

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
			_settings.DownloadPages = DownloadPages;
			Log.LogInfo("Pages to download: " + DownloadPages);

			_settings.StartPageNumber = StartPageNumber;
			Log.LogInfo("Start page number: " + StartPageNumber);

			_settings.SelectedCountry = _selectedCountry;
			Log.LogInfo("Selected country " + _selectedCountry);
		}

		#endregion
	}
}