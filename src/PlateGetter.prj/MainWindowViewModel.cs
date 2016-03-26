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

		private ImageDownloader _imageLoader;

		private int _currentPage;

		private int _progress;

		#endregion


		#region .ctor

		public MainWindowViewModel(ProgrammSettings settings)
		{
			_settings = settings;
			_currentPage = _settings.StartPageNumber;

			_imageLoader = new ImageDownloader(_settings.SelectedCountry);
			_imageLoader.OnImageLoaded += OnImageLoaded;
			_imageLoader.OnPageSkiped += OnPageSkiped;
		}

		private void OnPageSkiped(object sender, int e)
		{
			_currentPage = e;
			OnPropertyChanged("CurrentPage");
		}

		private void OnImageLoaded(object sender, EventArgs e)
		{
			_image = sender as BitmapImage;
			OnPropertyChanged("Image");
			OnPropertyChanged("CurrentPage");
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
			_imageLoader.LoadOneAsync(_currentPage, _settings.EndPageNumber);
			OnPropertyChanged("CurrentPage");
		}

		/// <summary>Загружает все изображения.</summary>
		public void DownloadAll()
		{

		}

		/// <summary>Останавливает загрузку изображения.</summary>
		public void Stop()
		{
			_imageLoader.CancelDownload();
			_progress = 0;
			OnPropertyChanged("CurrentPage");
		}

		internal void Settings()
		{
			Stop();

			using(var settingsForm = new Settings.SettingsForm(_settings))
			{
				if(settingsForm.ShowDialog() == true)
				{
					OnPropertyChanged("TotalPages");

					_currentPage = _settings.StartPageNumber;
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
