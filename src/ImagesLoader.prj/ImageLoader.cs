using PlateGetter.Core;
using PlateGetter.Core.Helpers;
using PlateGetter.Core.Log;
using System;
using System.Collections.Generic;
using System.IO;
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
		#region Data

		private CancellationTokenSource _cancelationTokenSource = new CancellationTokenSource();

		private object _syncRoot = new object();

		private int _downlodedImages;

		#endregion

		#region Properties

		public Country CurrentCountry { get; set; }

		public int DownloadedImages => _downlodedImages;

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
			CurrentCountry = coutryPlateName;
		}

		#endregion

		#region Public methods

		public void CancelDownload()
		{
			_cancelationTokenSource?.Cancel();
		}

		public async void LoadNextAsync(int startPage, int endPage)
		{
			_downlodedImages = 0;

			if(_cancelationTokenSource.IsCancellationRequested) _cancelationTokenSource = new CancellationTokenSource();

			try
			{
				await LoadNext(startPage, endPage, _cancelationTokenSource.Token).ConfigureAwait(false);
			}
			catch(Exception exc)
			{

			}
		}
		
		public async Task LoadNext(int page, int endPage, CancellationToken token)
		{
			string regexImage = "";
						
			while(regexImage == "" && page > endPage && !token.IsCancellationRequested)
			{
				regexImage = await GetImageLinkAsync(page--);
				OnPageSkiped.BeginInvoke(this, page, null, null);
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

		public async void LoadOneAsync(int page, CancellationToken token)
		{
			string regexImage = await GetImageLinkAsync(page);

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
				await client.DownloadFileTaskAsync(regexImage, "images\\" + CurrentCountry.PlateName + "\\foto" + page + ".jpeg");
				_downlodedImages++;
			}				
		}		

		public async Task DownloadAll(int startPage, int stopPage)
		{
			_downlodedImages = 0;
			while(_downlodedImages < stopPage)
			{
				await Task.Factory.StartNew(() => LoadOneAsync(startPage--, _cancelationTokenSource.Token)).ConfigureAwait(false);
			}
		}
		
		public bool SaveImage(BitmapImage image, int page)
		{
			if(image == null) return false;

			var encoder = new JpegBitmapEncoder();
			encoder.Frames.Add(BitmapFrame.Create(image));

			Utilities.ValidatePath("images\\" + CurrentCountry.PlateName);

			try
			{
				using(var stream = new FileStream("images\\" + CurrentCountry.PlateName + "\\foto" + page + ".jpeg", FileMode.CreateNew))
				{
					encoder.Save(stream);
				}
			}
			catch
			{
				return false;
			}

			_downlodedImages++;

			return true;
		}

		#endregion

		#region Private methods

		private void DownloadProgress(object sender, DownloadProgressEventArgs e) => OnDownloadProgressChanged.BeginInvoke(this, e.Progress, null, null);

		private void LoadProgressChanged(object sender, DownloadProgressChangedEventArgs e) => OnDownloadProgressChanged.BeginInvoke(this, e.ProgressPercentage, null, null);

		private void ImageDownloadCompleted(object sender, EventArgs e)
		{
			(sender as BitmapImage)?.Freeze();
			OnImageLoaded.BeginInvoke(sender, e, null, null);
			Task.Factory.StartNew(() => Log.LogDebug("Image loaded"));
		}

		private string GetImageLink(int currentPage)
		{
			string pageTitle = CurrentCountry.FullName;
			string regexImage = "";
			string page = "";

			try
			{
				using(var webClient = new WebClient())
				{
					page = webClient.DownloadString(new Uri("http://platesmania.com/" + CurrentCountry.PlateName + "/foto" + currentPage));
				}

				pageTitle = new Regex("<title>(.*)</title>").Matches(page)[0].Groups[1].Value;

				if(pageTitle.ToLower() == CurrentCountry.FullName.ToLower())
				{
					Task.Factory.StartNew(() => Log.LogWarning($"Page {currentPage} not found"));
					return "";
				}

				regexImage = new Regex("<img src=\"?(.*jpg)\"? class=\"img-responsive center-block.*>").Matches(page)[0].Groups[1].Value;

				if(regexImage.Length < 3)
				{
					Task.Factory.StartNew(() => Log.LogWarning($"Page {currentPage} has incorect format"));
					return "";
				}

				Task.Factory.StartNew(() =>
				{
					Utilities.ValidatePath("images\\" + CurrentCountry.PlateName + "\\Analytics");
					var regexDiscription = new Regex("<img .*alt=\"(.*)\" .*>").Matches(page)[0].Groups[1].Value;

					Analytics.WriteAnalyticsData(regexDiscription, regexImage, CurrentCountry.PlateName);
				});

				Task.Factory.StartNew(() => Log.LogDebug("Founded image " + regexImage));

				return regexImage;
			}
			catch(Exception exc)
			{
				Task.Factory.StartNew(() => Log.LogError(nameof(GetImageLink), exc));
				return "";
			}
		}

		private async Task<string> GetImageLinkAsync(int currentPage)
		{
			return await Task.Factory.StartNew(() => GetImageLink(currentPage));
		}

		#endregion
	}
}
