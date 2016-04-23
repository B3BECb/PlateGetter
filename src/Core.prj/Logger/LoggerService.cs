using System;
using System.Linq;

namespace PlateGetter.Core.Logger
{
	public class LoggerService : IDisposable
	{
		private static NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();

		public LoggerService()
		{
			var config  = new NLog.Config.LoggingConfiguration();
			
			config.AddTarget(new NLog.Targets.FileTarget() { Name="FileLogger", FileName = "log.log" });
			config.AddRuleForAllLevels(config.AllTargets.FirstOrDefault());

			NLog.LogManager.Configuration = config;
		}

		public void Dispose()
		{
			Log.Trace("----Shutdown----");
			NLog.LogManager.Shutdown();
		}
	}
}
