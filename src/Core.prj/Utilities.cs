using PlateGetter.Core.Logger;
using System;
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
				try
				{
					Directory.CreateDirectory(path);
					log.Debug("Created directory " + path);
				}
				catch(Exception ex)
				{
					log.Warn(ex, "Could not create directory " + path);
				}
			}
		}
	}
}
