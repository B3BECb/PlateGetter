using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PlateGetter.Core.Helpers
{
	public class LogMessage
	{
		public string Message { get; private set; }

		public SolidColorBrush Brush { get; private set; }

		public LogMessage(string message)
		{
			Message = message;
			Brush = new SolidColorBrush(Colors.Black);
		}

		public LogMessage(string message, Color color)
		{
			Message = message;
			Brush = new SolidColorBrush(color);
		}
	}
}
