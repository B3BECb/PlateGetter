using AnalyticVisualizer;
using PlateGetter.Core;
using PlateGetter.Core.Helpers;
using PlateGetter.Core.Logger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PlateGetter.Core.Analytic
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
			plateNumberMask = new Regex("[0-9]").Replace(plateNumberMask, "9");

			var car = new CarInfo(plateNumberOriginal, plateNumberMask, fotoUrl, plateInfo);

			SaveInXmlFormat(car, $"images\\{plateName}\\Analytics\\Statistics.xml");

			Log.LogDebug($"Serialized plate: {plateNumberOriginal}. Plate template: {plateNumberMask}");
		}

		public static void Analize(List<Country> countries)
		{
			Log.LogInfo("Analize started");
			foreach(var country in countries)
			{
				if(File.Exists($"images\\{country.PlateName}\\Analytics\\Statistics.xml"))
				{
					var plates = LoadFromXmlFormat($"images\\{country.PlateName}\\Analytics\\Statistics.xml");
					Log.LogDebug($"Founded statistic for {country}. Total plates:{plates.Count}.");
					
					using(var form = new AnalyticWindow(CountTemplates(plates)))
					{
						if(form.ShowDialog() == true) { }
					}
				}
			}
			Log.LogInfo("Analize finished");
		}

		private static void SaveInXmlFormat(object objGraph, string fileName)
		{
			lock(_syncRoot)
			{
				XmlSerializer xmlFormat = new XmlSerializer(typeof(CarInfo));
				using(Stream fStream = new FileStream(fileName, FileMode.Append, FileAccess.Write, FileShare.None))
				{
					xmlFormat.Serialize(fStream, objGraph);
				}
			}
		}

		private static List<CarInfo> LoadFromXmlFormat(string fileName)
		{
			List<CarInfo> plateInfoList = new List<CarInfo>();
			
			// 1. remove <?xml version="1.0"?>
			// 2. add <?xml version="1.0"?><RootCarInfo><Carinfos>
			// 3. add doc body
			// 4. add </Carinfos></RootCarInfo> 

			PrepareFile(fileName);

			XmlSerializer xmlFormat = new XmlSerializer(typeof(RootCarInfo));
			using(Stream fStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None))
			{
				var a = xmlFormat.Deserialize(fStream);
				foreach(var plate in (a as RootCarInfo).Carinfos)
				{
					plateInfoList.Add(plate);
				}			
			}
			return plateInfoList;			
		}

		/// <summary>
		/// Prepare text in a file.
		/// </summary>
		/// <param name="filePath">Path of the text file.</param>
		/// <param name="searchText">Text to search for.</param>
		/// <param name="replaceText">Text to replace the search text.</param>
		static private void PrepareFile(string filePath)
		{
			StreamReader reader = new StreamReader(filePath);
			string content = reader.ReadToEnd();
			reader.Close();

			content = Regex.Replace(content, "<.xml version=.1.0..>|<RootCarInfo><Carinfos>|<.Carinfos></RootCarInfo>", "");

			StreamWriter writer = new StreamWriter(filePath);
			writer.Write("<?xml version=\"1.0\"?><RootCarInfo><Carinfos>" + content + "</Carinfos></RootCarInfo>");
			writer.Close();
		}

		static private Dictionary<string, int> CountTemplates(List<CarInfo> platesList)
		{
			Dictionary<string, int> countedTemplates = new Dictionary<string, int>();

			//grouping
			var templatesList = platesList.GroupBy(v => v.PlateMask).Where(g => g.Count() > 1).Select(g => g.Key);

			foreach(var groupedTemplate in templatesList)
			{
				countedTemplates.Add(groupedTemplate, platesList.Count(e => e.PlateMask == groupedTemplate));
				platesList.RemoveAll(e => e.PlateMask == groupedTemplate);
			}

			foreach(var template in platesList)
			{
				countedTemplates.Add(template.PlateMask, 1);
			}

			return countedTemplates;
		}
	}
}
