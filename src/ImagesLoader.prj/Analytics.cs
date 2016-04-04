using PlateGetter.Core;
using PlateGetter.Core.Helpers;
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

		public sealed class RootCarInfo
		{
			public CarInfo[] Carinfos { get; set; }

			public RootCarInfo() { }
		}

		private static object _syncRoot = new object();
		
		public static void WriteAnalyticsData(string plateInfo, string fotoUrl, string plateName)
		{
			var plateNumberOriginal = plateInfo.Split(',').FirstOrDefault().ToLower();
			var plateNumberMask = new Regex("[a-z]").Replace(plateNumberOriginal, "X");
			plateNumberMask = new Regex("[1-9]").Replace(plateNumberMask, "9");

			var car = new CarInfo(plateNumberOriginal, plateNumberMask, fotoUrl, plateInfo);

			SaveInXmlFormat(car, $"images\\{plateName}\\Analytics\\Statistics.xml");
		}

		public static void Analize(List<Country> countries)
		{
			foreach(var country in countries)
			{
				var s = new RootCarInfo();
				s.Carinfos = new CarInfo[] { new CarInfo("1", "2", "3", "4") , new CarInfo("5", "6", "7", "8") , new CarInfo("9", "0", "-", "=") };
				if(File.Exists($"images\\{country.PlateName}\\Analytics\\Statistics.xml")) LoadFromXmlFormat($"images\\{country.PlateName}\\Analytics\\Statistics.xml");
					//SaveInXmlFormat(s, $"images\\{country.PlateName}\\Analytics\\Statistics.xml");
			}
		}

		private static void SaveInXmlFormat(object objGraph, string fileName)
		{
			lock(_syncRoot)
			{
				XmlSerializer xmlFormat = new XmlSerializer(typeof(RootCarInfo));
				using(Stream fStream = new FileStream(fileName, FileMode.Append, FileAccess.Write, FileShare.None))
				{
					xmlFormat.Serialize(fStream, objGraph);
				}
			}
		}

		private static List<CarInfo> LoadFromXmlFormat(string fileName)
		{
			lock (_syncRoot)
			{
				List<CarInfo> info = new List<CarInfo>();

				// 1. remove <?xml version="1.0"?>
				// 2. add <?xml version="1.0"?><RootCarInfo><Carinfos>
				// 3. add doc body
				// 4. add </Carinfos></RootCarInfo> 


				XmlSerializer xmlFormat = new XmlSerializer(typeof(RootCarInfo));
				using(Stream fStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None))
				{
					var a = xmlFormat.Deserialize(fStream);
					info.Add(a as CarInfo);
				}
				return info;
			}
		}
	}
}
