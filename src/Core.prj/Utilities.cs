using PlateGetter.Core.Logger;
using System.IO;

namespace PlateGetter.Core
{
	public abstract class Utilities
	{
		public static void ValidatePath(string path)
		{
			if(!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
				Log.LogDebug("Created directory " + path);
			}
		}
	}
}
