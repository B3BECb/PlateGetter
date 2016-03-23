using PlateGetter.Helpers;
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
using System.Windows.Shapes;

namespace PlateGetter.Settings
{
	/// <summary>
	/// Interaction logic for SettingsForm.xaml
	/// </summary>
	public partial class SettingsForm : Window, IDisposable
	{
		SettingsViewModel _viewModel;

		public SettingsForm(ProgrammSettings settings)
		{
			_viewModel = new SettingsViewModel(settings);

			DataContext = _viewModel;

			InitializeComponent();
		}

		public void Dispose()
		{
			
		}

		private void button1_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void button_Click(object sender, RoutedEventArgs e)
		{
			_viewModel.Apply();
			DialogResult = true;
			Close();
		}

		private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			_viewModel.SaveSelectedCountry(((sender as ComboBox).SelectedItem) as Country);
		}
	}
}
