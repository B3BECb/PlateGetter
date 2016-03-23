using PlateGetter.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PlateGetter
{
	internal sealed class MainWindowViewModel : INotifyPropertyChanged
	{
		#region Properties
		
		public ImageSource Image => _image;

		public int CurrentPage => _currentPage;

		public int TotalPages => _settings.EndPageNumber;

		public int DownloadProgress => _progress;

		#endregion



		#region Events

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		
		#region Data

		private ImageSource _image;

		private ProgrammSettings _settings;

		private CancellationTokenSource _cancelationTokenSource;

		private int _currentPage;

		private int _progress;

		#endregion


		#region .ctor

		public MainWindowViewModel(ProgrammSettings settings)
		{
			_settings = settings;
			_currentPage = _settings.StartPageNumber - 1;
		}

		#endregion


		#region Public methods

		/// <summary>Сохраняет текущее изображение.</summary>
		public void Save()
		{

		}

		/// <summary>Переходит к следующему изображению.</summary>
		public void NextPage()
		{

		}

		/// <summary>Загружает все изображения.</summary>
		public void DownloadAll()
		{

		}

		/// <summary>Останавливает загрузку изображения.</summary>
		public void Stop()
		{
			_cancelationTokenSource?.Cancel();
			_progress = 0;
		}

		internal void Settings()
		{
			Stop();

			using(var settingsForm = new Settings.SettingsForm(_settings))
			{
				if(settingsForm.ShowDialog() == true)
				{
					OnPropertyChanged("TotalPages");

					_currentPage = _settings.StartPageNumber - 1;
					OnPropertyChanged("CurrentPage");
				}
			}
		}

		#endregion


		#region Private methods
		
		private void OnPropertyChanged(string propertyName)
		{
			PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion
	}
}
