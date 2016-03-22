using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PlateGetter
{
	class MainWindowViewModel : INotifyPropertyChanged
	{
		#region Properties

		public int StartPage { get; set; }

		public int StopPage { get; set; }

		public ImageSource Image => _image;

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion


		#region Data

		private ImageSource _image;

		#endregion


		#region .ctor

		public MainWindowViewModel()
		{

		}

		#endregion


		#region Public methods

		public void NextPage()
		{

		}

		public void Start()
		{

		}

		public void Stop()
		{

		}

		public void Save()
		{

		}

		public void DownloadAll()
		{

		}

		#endregion


		#region Private methods

		private void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion
	}
}
