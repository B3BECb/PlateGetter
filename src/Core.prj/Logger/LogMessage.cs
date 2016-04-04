using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PlateGetter.Core.Logger
{
	public class LogMessage
	{
		public string Message { get; private set; }

		public SolidColorBrush TextBrush { get; private set; }
		
		public LogMessage(string message)
		{
			Message = message;
			TextBrush = new SolidColorBrush(Colors.Black);
			TextBrush.Freeze();
		}

		public LogMessage(string message, Color color)
		{
			Message = message;
			TextBrush = new SolidColorBrush(color);
			TextBrush.Freeze();
		}
	}
}
