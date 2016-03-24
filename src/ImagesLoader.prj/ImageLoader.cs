using PlateGetter.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

		#endregion


		#region Events

		#endregion

		#region .ctor
		/// <summary>Создаёт класс загрузчика изображений.</summary>
		/// <param name="coutryPlateName">Инициалы страны на номерном знаке.</param>
		public ImageDownloader(Country coutryPlateName)
		{
			_currentCountry = coutryPlateName;
		}

		#endregion


		/*
		
			rex1 = re.compile(r'http://img[0-9][1-9].avto-nomer.ru/.{1,30}jpg')
			rex2 = re.compile(r'http://img[0-9][1-9].platesmania.com/.{1,30}/o/.{1,30}jpg')
			
		*/


		#region Public methods

		public BitmapImage LoadOne()
		{
			BitmapImage image = new BitmapImage();
			image.BeginInit();
			image.CacheOption = BitmapCacheOption.OnLoad;
			image.UriSource = new Uri(filePath);
			image.EndInit();
			return image;
		}

		#endregion


		#region Private methods

		#endregion
	}
}
