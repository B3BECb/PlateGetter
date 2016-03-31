using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PlateGetter.ImagesLoader
{
	public class Analytics
	{
		[Serializable]
		public sealed class CarInfo
		{
			public string PlateNumber { get; set; }

			public string PlateMask { get; set; }

			public string PlateFotoUrl { get; set; }

			public string TotalPlateInfo { get; set; }

			public CarInfo()
			{

			}

			public CarInfo(string plateNumber, string plateMask, string plateFotoUrl, string totalPlateInfo)
			{
				PlateNumber		= plateNumber;
				PlateMask		= plateMask;
				PlateFotoUrl	= plateFotoUrl;
				TotalPlateInfo	= totalPlateInfo;
			}
		}
		
		public static void WriteAnalyticsData(string plateInfo, string fotoUrl, string plateName)
		{
			var plateNumberOriginal = plateInfo.Split(',').FirstOrDefault().ToLower();
			var plateNumberMask = new Regex("[a-z]").Replace(plateNumberOriginal, "X");
			plateNumberMask = new Regex("[1-9]").Replace(plateNumberMask, "9");

			var car = new CarInfo(plateNumberOriginal, plateNumberMask, fotoUrl, plateInfo);

			SaveInXmlFormat(car, $"images\\{plateName}\\Analytics\\Statistics.xml");
		}

		private static void SaveInXmlFormat(object objGraph, string fileName)
		{
			XmlSerializer xmlFormat = new XmlSerializer(typeof(CarInfo));///////////////////////////////////////////!
			using(Stream fStream = new FileStream(fileName, FileMode.Append, FileAccess.Write, FileShare.None))
			{
				xmlFormat.Serialize(fStream, objGraph);
			}
		}
	}
}
