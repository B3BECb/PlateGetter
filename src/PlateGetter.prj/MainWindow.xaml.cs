using PlateGetter.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PlateGetter
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, IDisposable
	{
		private MainWindowViewModel _viewModel;

		public MainWindow(ProgrammSettings settings)
		{			
			_viewModel = new MainWindowViewModel(settings);
			DataContext = _viewModel;

			InitializeComponent();
		}

		public void Dispose()
		{
			
		}

		private void OnSettingsClick(object sender, RoutedEventArgs e)
		{
			_viewModel.Settings();
		}

		private void button_Click(object sender, RoutedEventArgs e)
		{
			_viewModel.Save();
		}

		private void button1_Click(object sender, RoutedEventArgs e)
		{
			_viewModel.NextPage();
		}

		private void button3_Click(object sender, RoutedEventArgs e)
		{
			_viewModel.DownloadAll();
		}

		private void button4_Click(object sender, RoutedEventArgs e)
		{
			_viewModel.Stop();
		}

		private void button2_Click(object sender, RoutedEventArgs e)
		{
			_viewModel.OpenFolder();
		}
	}
}
