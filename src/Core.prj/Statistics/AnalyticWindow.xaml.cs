using De.TorstenMandelkow.MetroChart;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace PlateGetter.Core.Statistics
{
	/// <summary>
	/// Interaction logic for AnalyticWindow.xaml
	/// </summary>
	public partial class AnalyticWindow : Window, IDisposable
	{
		public AnalyticWindow(Dictionary<string, CountryStatistic> countriesTemplates)
		{
			InitializeComponent();
			
			int col = 0;

			foreach(var country in countriesTemplates)
			{
				grid.Children.Add(new DoughnutChart()
				{ ChartTitle = country.Key,
					ChartSubTitle = "Распределение номеров для " + country.Key,
					Name = country.Key.Replace(' ', '_'),
					BorderThickness = new Thickness(1,0,1,0),
					BorderBrush = new SolidColorBrush(Colors.LightGray)
				});

				var insertedChild = grid.Children[grid.Children.Count - 1] as DoughnutChart;
				
				Grid.SetColumn(insertedChild, col);
				grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

				insertedChild.Series.Add(new ChartSeries() { SeriesTitle = country.Key, DisplayMember = "Category", ValueMember = "Number" });

				foreach(var plate in country.Value.Templates)
				{
					insertedChild.Series.Last().Items.Add(plate);
				}

				//-----

				grid.Children.Add(new StackedBarChart()
				{
					ChartTitle = country.Key,
					ChartSubTitle = "Распределение символов для " + country.Key,
					Name = country.Key.Replace(' ', '_'),
					BorderThickness = new Thickness(1, 0, 1, 0),
					BorderBrush = new SolidColorBrush(Colors.LightGray)
				});

				var insertedChild2 = grid.Children[grid.Children.Count - 1] as StackedBarChart;

				Grid.SetColumn(insertedChild2, col++);
				grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

				Grid.SetRow(insertedChild2, 1);
				grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });

				insertedChild2.Series.Add(new ChartSeries() { SeriesTitle = country.Key, DisplayMember = "Category", ValueMember = "Number" });

				foreach(var plate in country.Value.Letters)
				{
					insertedChild2.Series.Last().Items.Add(plate);
				}
			}
		}

		public void Dispose()
		{
			
		}
	}
}
