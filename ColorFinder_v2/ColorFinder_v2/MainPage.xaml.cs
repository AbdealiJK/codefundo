using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using ColorFinder_v2.Resources;
using System.Windows.Media.Animation;
using System.Windows.Media;

namespace ColorFinder_v2
{
	public partial class MainPage : PhoneApplicationPage
	{
		
		// Constructor
		public MainPage()
		{
			InitializeComponent();

			// Sample code to localize the ApplicationBar
			//BuildLocalizedApplicationBar();
		}

		private void Clr1_Click(object sender, RoutedEventArgs e)
		{
			this.NavigationService.Navigate(new Uri("/Clr1in.xaml", UriKind.Relative));
			//clr1.Begin();
			
		}

		private void Clr2_Click(object sender, RoutedEventArgs e)
		{
			this.NavigationService.Navigate(new Uri("/Clr2in.xaml", UriKind.Relative));
			//clr2.Begin();
			
		}

		private void cmp_Click(object sender, RoutedEventArgs e)
		{
			int b1 = (int)PhoneApplicationService.Current.State["c1b"];
			int g1 = (int)PhoneApplicationService.Current.State["c1g"];
			int r1 = (int)PhoneApplicationService.Current.State["c1r"];
			int b2 = (int)PhoneApplicationService.Current.State["c2b"];
			int g2 = (int)PhoneApplicationService.Current.State["c2g"];
			int r2 = (int)PhoneApplicationService.Current.State["c2r"];
			double c1, m1, y1, c2, m2, y2;
			c1 = 1 - ((double)r1 / 255);
			m1 = 1 - ((double)g1 / 255);
			y1 = 1 - ((double)b1 / 255);
			c2 = 1 - ((double)r2 / 255);
			m2 = 1 - ((double)g2 / 255);
			y2 = 1 - ((double)b2 / 255);
			double cx, mx, yx;
			cx = Math.Abs(c2 - c1);
			mx = Math.Abs(m2 - m1);
			yx = Math.Abs(y2 - y1);
			double rx, bx, gx;
			rx = (int)((1 - cx) * 255);
			gx = (int)((1 - mx) * 255);
			bx = (int)((1 - yx) * 255);
			txt.Fill = new SolidColorBrush(Color.FromArgb(255,(byte)rx,(byte)gx,(byte)bx));
			//double d = Math.Sqrt((r2-r1)^2+(b2-b1)^2+(g2-g1)^2);
			//double p = 100.0 - (d/(3*(255)^2));
			//txt.Text = Convert.ToString(p);
	}

		private void DClr1_Click(object sender, RoutedEventArgs e)
		{
			int b1 = (int)PhoneApplicationService.Current.State["c1b"];
			int g1 = (int)PhoneApplicationService.Current.State["c1g"];
			int r1 = (int)PhoneApplicationService.Current.State["c1r"];
			rect1.Fill = new SolidColorBrush(Color.FromArgb(255, (byte)r1, (byte)g1, (byte)b1));

		}

		private void DClr2_Click(object sender, RoutedEventArgs e)
		{
			int b2 = (int)PhoneApplicationService.Current.State["c2b"];
			int g2 = (int)PhoneApplicationService.Current.State["c2g"];
			int r2 = (int)PhoneApplicationService.Current.State["c2r"];
			rect2.Fill = new SolidColorBrush(Color.FromArgb(255, (byte)r2, (byte)g2, (byte)b2));
		}
	}
}