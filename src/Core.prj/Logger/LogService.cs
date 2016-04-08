using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace PlateGetter.Core.Logger
{
	public class LogService : IDisposable
	{
		private Timer _logUpdaer = new Timer(500);

		private int _logHash;

		private int _lastMessage;

		private string _logPath = "Images/Log.log";

		public LogService()
		{
			_logUpdaer.Start();
			_logUpdaer.Elapsed += _logUpdaer_Elapsed;
		}

		private void _logUpdaer_Elapsed(object sender, ElapsedEventArgs e)
		{
			var currentHash = Log.Messages.GetHashCode();

			if(_logHash != currentHash)
			{
				_logHash = currentHash;

				for (int i = _lastMessage; i < Log.Messages.Count; i++)
				{
					using (var writer = new StreamWriter(_logPath))
					{
						writer.WriteLine(Log.Messages.Where(elem => elem.Key == i));
					}
				}        

				_lastMessage = Log.Messages.Keys.Last();
			}
		}

		public void Dispose()
		{
			_logUpdaer.Stop();
			_logUpdaer.Dispose();
		}
	}
}
