using System.Windows;
using System.ComponentModel;
using System;
using System.Windows.Documents;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace PlateGetter.Core.Logger
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class LogWindow : Window, IDisposable
	{
		private bool _isDisposing;

		private static NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();
		
		public LogWindow()
		{
			InitializeComponent(); 
		}

		private void Window_Closing(object sender, CancelEventArgs e)
		{
			if(!_isDisposing)
			{
				e.Cancel = true;
				Hide();
			}
		}

		public void Dispose()
		{
			_isDisposing = true;
			Close();
		}	
	}
}
