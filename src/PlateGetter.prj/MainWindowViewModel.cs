using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PlateGetter
{
	internal sealed class MainWindowViewModel : INotifyPropertyChanged
	{
		#region Properties

		public int StartPage { get; set; }

		public int StopPage { get; set; }

		public ImageSource Image => _image;

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion


		#region Data

		private ImageSource _image;

		private ProgrammSettings _settings;

		#endregion


		#region .ctor
		
		public MainWindowViewModel(ProgrammSettings settings)
		{
			_settings = settings;
		}

		#endregion


		#region Public methods

		public void NextPage()
		{

		}

		public void Start()
		{

		}

		internal void Settings()
		{
			using(var settingsForm = new Settings.SettingsForm(_settings))
			{
				settingsForm.ShowDialog();
			}
		}

		public void Stop()
		{

		}

		public void Save()
		{

		}

		public void DownloadAll()
		{

		}

		#endregion


		#region Private methods

		private void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion
	}
}
