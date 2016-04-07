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

		private int _fileHash;

		private string _logPath = "Images/Log.log";

		private Dictionary<string, LogMessage> _messages = new Dictionary<string, LogMessage>();

		public event EventHandler LogUpdated;

		public LogService()
		{
			_logUpdaer.Start();
			_logUpdaer.Elapsed += _logUpdaer_Elapsed;
		}

		private void _logUpdaer_Elapsed(object sender, ElapsedEventArgs e)
		{
			var currentHash = new FileInfo(_logPath).GetHashCode();

			if(_fileHash != currentHash)
			{
				_fileHash = currentHash;

				var newMessages = GetNewMessages();

				LogUpdated?.Invoke(this, newMessages);
			}
		}

		public void Dispose()
		{
			_logUpdaer.Stop();
			_logUpdaer.Dispose();
		}
	}
}
