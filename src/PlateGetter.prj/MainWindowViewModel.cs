using PlateGetter.ImagesLoader;
using PlateGetter.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System.Diagnostics;
using PlateGetter.Core.Logger;
using PlateGetter.Core.Statistics;

namespace PlateGetter
{
	internal sealed class MainWindowViewModel : INotifyPropertyChanged, IDisposable
	{
		#region Properties

		public string Country => _settings.SelectedCountry.FullName;

		public ImageSource Image => _image;

		public int DownlodedImages => _imageLoader.DownloadedImages;

		public int TotalPages => _settings.DownloadPages;

		public int DownloadProgress => _progress;

		#endregion

		#region Events

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion
		
		#region Data

		private ImageSource _image;

		private ProgrammSettings _settings;

		private ImageDownloader _imageLoader;

		private LogWindow _logWindow = new LogWindow();

		private int _currentPage;

		private int _progress;

		#endregion

		#region .ctor

		public MainWindowViewModel(ProgrammSettings settings)
		{
			_settings = settings;
			_currentPage = _settings.StartPageNumber;

			_imageLoader = new ImageDownloader(_settings.SelectedCountry);
			_imageLoader.OnDownloadProgressChanged += OnDownloadProgressChanged;
			_imageLoader.OnImageLoaded += OnImageLoaded;
			_imageLoader.OnPageSkiped += OnPageSkiped;
		}

		private void OnDownloadProgressChanged(object sender, int e)
		{
			_progress = e;
			OnPropertyChanged("DownloadProgress");
		}

		private void OnPageSkiped(object sender, int e)
		{
			_currentPage = e;
			OnPropertyChanged("CurrentPage");
			OnPropertyChanged("DownlodedImages");
		}

		private void OnImageLoaded(object sender, EventArgs e)
		{
			_image = sender as BitmapImage;
			OnPropertyChanged("Image");
			OnPropertyChanged("DownlodedImages");
		}

		#endregion

		#region Public methods

		public void OpenFolder()
		{
			Utilities.ValidatePath("images\\" + _settings.SelectedCountry.PlateName);

			Process.Start(new ProcessStartInfo("explorer.exe", "images"));
		}

		public void AnalizeData()
		{
			Analytics.Analize(_settings.CountriesList);
		}

		/// <summary>Сохраняет текущее изображение.</summary>
		public void Save()
		{
			_progress = 0;
			OnPropertyChanged("DownloadProgress");
			
			_imageLoader.LoadNextAsync(_currentPage);

			_imageLoader.SaveImage(_image as BitmapImage, _currentPage);

			OnPropertyChanged("DownlodedImages");
		}

		/// <summary>Переходит к следующему изображению.</summary>
		public void NextPage()
		{
			_progress = 0;
			OnPropertyChanged("DownloadProgress");
			OnPropertyChanged("DownlodedImages");

			_imageLoader.LoadNextAsync(_currentPage);
		}

		/// <summary>Загружает все изображения.</summary>
		public void DownloadAll()
		{
			Task.Factory.StartNew(() => _imageLoader.DownloadAll(_currentPage - _settings.DownloadPages, _settings.DownloadPages));
		}

		/// <summary>Останавливает загрузку изображения.</summary>
		public void Stop()
		{
			_imageLoader.CancelDownload();
			_progress = 0;
			Log.LogInfo("Load cancalled");
		}

		public void Settings()
		{
			Stop();

			using(var settingsForm = new Settings.SettingsForm(_settings))
			{
				if(settingsForm.ShowDialog() == true)
				{
					OnPropertyChanged("TotalPages");

					_currentPage = _settings.StartPageNumber;
					_imageLoader.CurrentCountry = _settings.SelectedCountry;
					OnPropertyChanged("CurrentPage");
					OnPropertyChanged("Country");
				}				
			}
		}

		public void ShowLog()
		{			
			if(_logWindow.ShowActivated)
			{
				_logWindow.Show();
				Log.LogInfo("Лог открыт");
			}		
			else
			{
				_logWindow.Hide();
				Log.LogInfo("Лог закрыт");
			}
		}

		public void Dispose()
		{
			_logWindow.Dispose();
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
