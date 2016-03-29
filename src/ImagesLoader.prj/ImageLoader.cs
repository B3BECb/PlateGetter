using PlateGetter.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace PlateGetter.ImagesLoader
{
	public class ImageDownloader
	{
		#region Properties

		#endregion


		#region Data

		private Country _currentCountry;

		private object _syncRoot = new object();

		private CancellationTokenSource _cancelationTokenSource = new CancellationTokenSource();

		private int _totalPlates;

		#endregion


		#region Events

		public event EventHandler<EventArgs> OnImageLoaded;

		public event EventHandler<int> OnPageSkiped;

		public event EventHandler<int> OnDownloadProgressChanged;

		#endregion

		#region .ctor
		/// <summary>Создаёт класс загрузчика изображений.</summary>
		/// <param name="coutryPlateName">Инициалы страны на номерном знаке.</param>
		public ImageDownloader(Country coutryPlateName)
		{
			_currentCountry = coutryPlateName;
		}

		#endregion


		#region Public methods

		public void CancelDownload()
		{
			_cancelationTokenSource?.Cancel();
		}

		public async void LoadNextAsync(int currentPage, int totalPlates)
		{
			_totalPlates = totalPlates;

			if(_cancelationTokenSource.IsCancellationRequested) _cancelationTokenSource = new CancellationTokenSource();

			try
			{
				await LoadNext(currentPage, _cancelationTokenSource.Token).ConfigureAwait(false);
			}
			catch(Exception exc)
			{

			}
		}

		public async void LoadOneAsync(int page)
		{
			if(_cancelationTokenSource.IsCancellationRequested) _cancelationTokenSource = new CancellationTokenSource();

			try
			{
				await LoadOne(page, _cancelationTokenSource.Token).ConfigureAwait(false);
			}
			catch(Exception exc)
			{

			}
		}

		public async Task LoadNext(int currentPage, CancellationToken token)
		{
			string regexImage = "";

			while(regexImage == "" && currentPage > _totalPlates && !token.IsCancellationRequested)
			{
				regexImage = await GetImageLink(currentPage--);
				OnPageSkiped.BeginInvoke(this, currentPage, null, null);
			}

			// Попытка улучшения качества фото. работает только с platesmania. s - низкое разрешение изображения, o - большое.
			regexImage = new Regex("/./").Replace(regexImage, "/o/");

			BitmapImage bitmapImage = new BitmapImage();
			bitmapImage.DownloadCompleted += ImageDownloadCompleted;
			bitmapImage.DownloadProgress += DownloadProgress;
			bitmapImage.BeginInit();
			bitmapImage.CacheOption = BitmapCacheOption.None;
			bitmapImage.UriSource = new Uri(regexImage, UriKind.Absolute);
			bitmapImage.EndInit();
		}

		public async Task LoadOne(int page, CancellationToken token)
		{
			string regexImage = await GetImageLink(page);

			if(regexImage == "")
			{
				OnPageSkiped.BeginInvoke(this, page, null, null);
				return;
			}

			// Попытка улучшения качества фото. работает только с platesmania. s - низкое разрешение изображения, o - большое.
			regexImage = new Regex("/./").Replace(regexImage, "/o/");

			using(var client = new WebClient())
			{
				client.DownloadFileCompleted += ImageDownloadCompleted;
				client.DownloadProgressChanged += LoadProgressChanged;
				await client.DownloadFileTaskAsync(regexImage, "images\\foto" + page + ".jpeg");
			}				
		}

		#endregion


		#region Private methods

		private void DownloadProgress(object sender, DownloadProgressEventArgs e) => OnDownloadProgressChanged.BeginInvoke(this, e.Progress, null, null);

		private void LoadProgressChanged(object sender, DownloadProgressChangedEventArgs e) => OnDownloadProgressChanged.BeginInvoke(this, e.ProgressPercentage, null, null);

		private void ImageDownloadCompleted(object sender, EventArgs e)
		{
			(sender as BitmapImage)?.Freeze();
			OnImageLoaded.BeginInvoke(sender, e, null, null);
		}

		private async Task<string> GetImageLink(int currentPage)
		{
			string pageTitle = _currentCountry.FullName;
			string regexImage = "";
			string page = "";

			try
			{
				using(var webClient = new WebClient())
				{
					page = await webClient.DownloadStringTaskAsync(new Uri("http://platesmania.com/" + _currentCountry.PlateName + "/foto" + currentPage)).ConfigureAwait(false);
				}

				pageTitle = new Regex("<title>(.*)</title>").Matches(page)[0].Groups[1].Value;

				if(pageTitle.ToLower() == _currentCountry.FullName.ToLower()) return "";

				regexImage = new Regex("<img src=\"?(.*jpg)\"? class=\"img-responsive center-block.*>").Matches(page)[0].Groups[1].Value;

				if(regexImage.Length < 3) return "";

				return regexImage;
			}
			catch
			{
				return "";
			}			
		}

		#endregion
	}
}
