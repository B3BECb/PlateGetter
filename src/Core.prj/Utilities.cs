using PlateGetter.Core.Logger;
using System.IO;

namespace PlateGetter.Core
{
	public abstract class Utilities
	{
		private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();

		public static void ValidatePath(string path)
		{
			if(!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
				log.Debug("Created directory " + path);
			}
		}
	}
}
