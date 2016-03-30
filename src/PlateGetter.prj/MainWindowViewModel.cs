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

		private bool _isSaveAllMode = false;

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
		}

		private void OnImageLoaded(object sender, EventArgs e)
		{
			_image = sender as BitmapImage;
			OnPropertyChanged("Image");
			OnPropertyChanged("CurrentPage");
		}

		#endregion


		#region Public methods

		public void OpenFolder()
		{
			ValidatePath("images\\" + _settings.SelectedCountry.PlateName);

			Process.Start(new ProcessStartInfo("explorer.exe", "images"));
		}

		/// <summary>Сохраняет текущее изображение.</summary>
		public void Save()
		{
			_progress = 0;
			OnPropertyChanged("DownloadProgress");

			_imageLoader.LoadNextAsync(_currentPage, _settings.EndPageNumber);

			SaveImage();
		}

		/// <summary>Переходит к следующему изображению.</summary>
		public void NextPage()
		{
			_progress = 0;
			OnPropertyChanged("DownloadProgress");

			_imageLoader.LoadNextAsync(_currentPage, _settings.EndPageNumber);
		}

		/// <summary>Загружает все изображения.</summary>
		public void DownloadAll()
		{			
			Parallel.For(_settings.EndPageNumber, _currentPage, (page) => 
			{
				_progress = 0;
				OnPropertyChanged("DownloadProgress");

				_imageLoader.LoadOne(page);				
			});
		}

		/// <summary>Останавливает загрузку изображения.</summary>
		public void Stop()
		{
			_imageLoader.CancelDownload();
			_progress = 0;
			_isSaveAllMode = false;
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

		private void ValidatePath(string path)
		{
			if(!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
		}

		private bool SaveImage()
		{
			if(_image == null) return false;

			var encoder = new JpegBitmapEncoder();
			encoder.Frames.Add(BitmapFrame.Create(_image as BitmapImage));

			ValidatePath("images\\" + _settings.SelectedCountry.PlateName);

			try
			{
				using(var stream = new FileStream("images\\" + _settings.SelectedCountry.PlateName + "\\foto" + _currentPage + ".jpeg", FileMode.CreateNew))
				{
					encoder.Save(stream);
				}
			}
			catch
			{
				return false;
			}

			return true;
		}

		#endregion
	}
}
