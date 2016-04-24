using System.Windows;
using System.ComponentModel;
using System;
using System.Windows.Documents;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Timers;
using System.IO;
using NLog;

namespace PlateGetter.Core.Logger
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class LogWindow : Window, IDisposable
	{
		private bool _isDisposing;

		private static NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();

		private Timer Timer;

		public LogWindow()
		{
			Timer = new Timer(1000);
			Timer.Elapsed += OnElapsed;
			InitializeComponent(); 
		}

		private void OnElapsed(object sender, ElapsedEventArgs e)
		{
			//NLog.Targets.
		}

		public new void Show()
		{
			Timer?.Start();
			base.Show();
		}

		private void Window_Closing(object sender, CancelEventArgs e)
		{
			if(!_isDisposing)
			{
				e.Cancel = true;
				Hide();
			}
			Timer?.Stop();
		}

		public void Dispose()
		{
			_isDisposing = true;
			Timer.Elapsed -= OnElapsed;
			Timer.Dispose();
			Close();
		}	
	}
}
