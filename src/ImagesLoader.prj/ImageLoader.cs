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

		#endregion


		#region Events

		public event EventHandler<EventArgs> OnImageLoaded;

		#endregion

		#region .ctor
		/// <summary>Создаёт класс загрузчика изображений.</summary>
		/// <param name="coutryPlateName">Инициалы страны на номерном знаке.</param>
		public ImageDownloader(Country coutryPlateName)
		{
			_currentCountry = coutryPlateName;
		}

		#endregion
			
		
		/*rex1 = re.compile(r'http://img[0-9][1-9].avto-nomer.ru/.{1,30}jpg')
		rex2 = re.compile(r'http://img[0-9][1-9].platesmania.com/.{1,30}/o/.{1,30}jpg')*/
			

		#region Public methods

		public void CancelDownload()
		{
			_cancelationTokenSource?.Cancel();
		}

		public async void LoadOneAsync(int currentPage)
		{
			try
			{
				await LoadOne(currentPage).ConfigureAwait(false);
			}
			catch(Exception exc)
			{

			}
		}

		public async Task LoadOne(int currentPage)
		{
			lock(_syncRoot)
			{
				string regexImage;
				using(var webClient = new WebClient())
				{
					var page = webClient.DownloadString(new Uri("http://platesmania.com/" + _currentCountry.PlateName + "/foto" + currentPage));

					var title = new Regex("<title>(.*)</title>").Matches(page)[0].Groups[1].Value;

					if(string.Compare(title, _currentCountry.FullName, false) != 0)
					{
						currentPage = FindNext(currentPage);
						currentPage--;
						return;
					}

					regexImage = new Regex("<img src=\"?(.*jpg)\"? class=\"img-responsive center-block.*>").Matches(page)[0].Groups[1].Value;					
				}
				
				// Проверка на получение неправильной строки. Пример: \"
				//if(regexImage.Length < 3)
				//{
					currentPage = FindNext(currentPage);
					currentPage--;
					return;
				//}

				// Попытка улучшения качества фото. работает только с platesmania. s - низкое разрешение изображения, o - большое.
				regexImage = new Regex("/./").Replace(regexImage, "/o/");

				//var regexDiscription = new Regex("<img .*alt=\"(.*)\" .*>").Matches(regexImageContainer)[0].Groups[1].Value;
				
				BitmapImage bitmapImage = new BitmapImage();
				bitmapImage.DownloadCompleted += ImageDownloadCompleted;
				bitmapImage.BeginInit();
				bitmapImage.CacheOption = BitmapCacheOption.None;
				bitmapImage.UriSource = new Uri(regexImage, UriKind.Absolute);
				bitmapImage.EndInit();
			}
		}

		#endregion


		#region Private methods

		private void ImageDownloadCompleted(object sender, EventArgs e)
		{
			(sender as BitmapImage)?.Freeze();
			OnImageLoaded.BeginInvoke(sender, e, null, null);
		}

		private int FindNext(int currentPage)
		{
			int foundedPage = currentPage;
			string pageTitle = _currentCountry.FullName;
			string regexImage = "";
			using(var webClient = new WebClient())
			{
				while(pageTitle.ToLower() == _currentCountry.FullName.ToLower() || regexImage.Length < 3) 
				{
					try
					{
						var page = webClient.DownloadString(new Uri("http://platesmania.com/" + _currentCountry.PlateName + "/foto" + foundedPage));

						pageTitle = new Regex("<title>(.*)</title>").Matches(page)[0].Groups[1].Value;

						regexImage = new Regex("<img src=\"?(.*jpg)\"? class=\"img-responsive center-block.*>").Matches(page)[0].Groups[1].Value;						
					}
					catch(Exception exc)
					{
						regexImage = "";
					}
					finally
					{
						foundedPage++;
					}
				}
			}
			
			return foundedPage;
		}

		#endregion
	}
}
