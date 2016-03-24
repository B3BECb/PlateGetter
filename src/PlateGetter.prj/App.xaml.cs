using PlateGetter.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PlateGetter
{
	/// <summary>
	/// Логика взаимодействия для App.xaml
	/// </summary>
	public partial class App : Application
	{
		ProgrammSettings _settings;

		App()
		{
			_settings = new ProgrammSettings();
			
			using(var mainForm = new MainWindow(_settings))
			{
				mainForm.ShowDialog();
			}
		}
	}
}
