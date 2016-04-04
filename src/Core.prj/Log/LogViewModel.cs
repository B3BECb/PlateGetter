using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace PlateGetter.Core.Log
{
	class LogViewModel : INotifyPropertyChanged
	{
		public LogMessage Runs { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;

		internal LogViewModel()
		{
			Log.OnLogged += OnLogged;
		}
		
		private void OnLogged(object sender, LogMessage e)
		{
			Runs = e;
			OnPropertyChanged(nameof(Runs));
		}

		#region Private methods

		private void OnPropertyChanged(string propertyName)
		{
			PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion
	}
}
