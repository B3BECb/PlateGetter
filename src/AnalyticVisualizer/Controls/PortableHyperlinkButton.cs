using System;
using System.Diagnostics;

#if NETFX_CORE

using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Markup;

#else

using System.Windows;
using System.Windows.Controls;

#endif

#if SILVERLIGHT
using System.Windows.Browser;
#endif
namespace AnalyticVisualizer
{
	public class PortableHyperLinkButton : Button
	{
		public static readonly DependencyProperty TextProperty =
			DependencyProperty.Register("Text", typeof(string), typeof(PortableHyperLinkButton),
										new PropertyMetadata(null));

		public static readonly DependencyProperty UrlProperty =
			DependencyProperty.Register("Url", typeof(string), typeof(PortableHyperLinkButton),
										new PropertyMetadata(null));

		public string Url
		{
			get { return (string)GetValue(UrlProperty); }
			set { SetValue(UrlProperty, value); }
		}

		public PortableHyperLinkButton()
		{
			this.Click += HyperLinkButton_Click;

#if NETFX_CORE
			this.DefaultStyleKey = typeof(PortableHyperLinkButton);
#elif SILVERLIGHT
			this.DefaultStyleKey = typeof(PortableHyperLinkButton);
#else

#endif
		}

		static PortableHyperLinkButton()
		{
#if NETFX_CORE

#elif SILVERLIGHT

#else
			DefaultStyleKeyProperty.OverrideMetadata(typeof(PortableHyperLinkButton), new FrameworkPropertyMetadata(typeof(PortableHyperLinkButton)));
#endif
		}

		void HyperLinkButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
#if NETFX_CORE
				Windows.System.Launcher.LaunchUriAsync(new Uri(Url));
#elif SILVERLIGHT
				HtmlPage.Window.Navigate(new Uri(Url), "_blank");
#else
				Process.Start(Url);
#endif
			}
			catch (Exception)
			{
			}
		}
	}
}
