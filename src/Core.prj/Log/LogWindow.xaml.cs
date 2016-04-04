using System.Windows;
using System.ComponentModel;
using System;
using System.Windows.Documents;
using System.Collections.Generic;
using System.Windows.Controls;

namespace PlateGetter.Core.Log
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class LogWindow : Window, IDisposable
	{
		private bool _isDisposing;

		private LogViewModel _viewModel = new LogViewModel();

		public LogWindow()
		{
			DataContext = _viewModel;

			_viewModel.PropertyChanged += PropertyChanged;

			InitializeComponent(); 
		}

		private void PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			Dispatcher.Invoke(() =>
			{
				var runs = (sender as LogViewModel).Runs;
				_panel.Children.Add(new Label() { Content = runs.Message, Foreground = runs.Brush});
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
