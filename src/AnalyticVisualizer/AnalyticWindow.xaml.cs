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

namespace AnalyticVisualizer
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class AnalyticWindow : Window, IDisposable
	{
		private AnalyticViewModel _viewModel = new AnalyticViewModel();

		public AnalyticWindow(Dictionary<string, int> templates)
		{
			DataContext = _viewModel;

			InitializeComponent();
		}

		private void ShellView_Loaded_1(object sender, RoutedEventArgs e)
		{
			Matrix m = PresentationSource.FromVisual(Application.Current.MainWindow).CompositionTarget.TransformToDevice;
			double dx = m.M11;
			double dy = m.M22;
		
			ScaleTransform s = (ScaleTransform)mainGrid.LayoutTransform;
			s.ScaleX = 1 / dx;
			s.ScaleY = 1 / dy;
		}

		public void Dispose()
		{
			//throw new NotImplementedException();
		}
	}
}
