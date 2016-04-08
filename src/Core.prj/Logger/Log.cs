using PlateGetter.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Documents;
using System.Windows.Media;

namespace PlateGetter.Core.Logger
{
	public static class Log
	{
		public static Dictionary<int, LogMessage> Messages = new Dictionary<int, LogMessage>();

		public static event EventHandler<LogMessage> OnLogged;
		
		public static void LogError(string message, Exception exc)
		{
			var runElement = new LogMessage($"{message}. Exeption: {exc}", Colors.Red);
			OnLogged?.BeginInvoke(null, runElement, null, null);
		}

		public static void LogWarning(string message)
		{			
			var runElement = new LogMessage(message, Colors.DarkOrange);
			OnLogged?.BeginInvoke(null, runElement, null, null);
		}

		public static void LogDebug(string message)
		{
			var runElement = new LogMessage(message, Colors.DarkBlue);
			OnLogged?.BeginInvoke(null, runElement, null, null);
		}

		public static void LogInfo(string message)
		{
			var runElement = new LogMessage(message);
			OnLogged?.BeginInvoke(null, runElement, null, null);
		}

		public static void LogInfo(string message, Color color)
		{
			var runElement = new LogMessage(message, color);
			OnLogged?.BeginInvoke(null, runElement, null, null);
		}
	}
}
