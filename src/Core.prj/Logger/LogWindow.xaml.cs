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

		public LogWindow()
		{
			Log.OnLogged += OnLogged;

			InitializeComponent(); 
		}

		private void OnLogged(object sender, LogMessage e)
		{
			Dispatcher.Invoke(() =>
			{
				_txtLog.Inlines.Add(new Run(e.Message) { Foreground = e.TextBrush});
				_txtLog.Inlines.Add(new LineBreak());

			});
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
