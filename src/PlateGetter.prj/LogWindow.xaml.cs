using PlateGetter.Core;
using System.Windows;
using System.ComponentModel;
using System;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Threading;
using PlateGetter.Core.Helpers;

namespace PlateGetter
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class LogWindow : Window, IDisposable
	{
		private bool _isDisposing;

		public LogWindow()
		{
			InitializeComponent();

			Log.OnLogged += OnLogged;
		}

		private void OnLogged(object sender, LogMessage e)
		{
			Dispatcher.Invoke(
			() =>
			{
				var element = new Run(e.Message) { Foreground = e.Brush };
				textBlock.Inlines.Add(element);
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
