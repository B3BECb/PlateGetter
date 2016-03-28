﻿using PlateGetter.ImagesLoader;
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

			if(_isSaveAllMode) SaveImage();
		}

		#endregion


		#region Public methods

		/// <summary>Сохраняет текущее изображение.</summary>
		public void Save()
		{			
			_imageLoader.LoadOneAsync(_currentPage, _settings.EndPageNumber);

			SaveImage();
		}

		/// <summary>Переходит к следующему изображению.</summary>
		public void NextPage()
		{
			_imageLoader.LoadOneAsync(_currentPage, _settings.EndPageNumber);
		}

		/// <summary>Загружает все изображения.</summary>
		public async void DownloadAll()
		{
			_isSaveAllMode = !_isSaveAllMode;

			for(int i = _currentPage; i > _settings.EndPageNumber; i--)
			{
				//_imageLoader.LoadOneAsync(_currentPage, _settings.EndPageNumber);
				await _imageLoader.LoadOne(i, CancellationToken.None).ConfigureAwait(false);
			}

			_isSaveAllMode = !_isSaveAllMode;
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

			ValidatePath("images");

			try
			{
				using(var stream = new FileStream("images\\foto" + _currentPage + ".jpeg", FileMode.CreateNew))
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
