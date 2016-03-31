using PlateGetter.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PlateGetter.ImagesLoader
{
	public class PageSearcher
	{
		#region Data

		public string PlateName { get; set; }

		#endregion
		public Dictionary<string, string> FindePlatePages(int amount)
		{
			Dictionary<string, string> foundedImages = new Dictionary<string, string>();

			using(var client = new WebClient())
			{
				int currentPage = 0;

				while(foundedImages.Count < amount)
				{
					var page = client.DownloadString(new Uri("http://platesmania.com/" + PlateName + "/gallery-" + currentPage++));

					var imageContainers = new Regex("<div class=\"panel - body\"><div class=\"row\"><a href=.*class=\"img - responsive center - block\"")
						.Matches(page);

					foreach(var container in imageContainers.Cast<string>())
					{
						var foto = new Regex("<a.*nomer(.*)\">").Matches(container)[0].Groups[1].Value;
						var imagelink = new Regex("<img src=\"(.*)\" class").Matches(container)[0].Groups[1].Value;
						foundedImages.Add(foto, imagelink);
					}
				}
			}

			return foundedImages;
		}
	}
}
