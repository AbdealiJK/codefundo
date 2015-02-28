using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Windows.Graphics.Display;
using Microsoft.Phone.Info;

namespace winfinityClient.Helpers
{
    class ScreenSizeMod
    {
        public static double XPixels = Application.Current.Host.Content.ActualWidth;
        public static double YPixels = Application.Current.Host.Content.ActualHeight;
        public static double ScaleFactor = Application.Current.Host.Content.ScaleFactor;
        public static double DpiX
        {
            get
            {
                object temp;
                DeviceExtendedProperties.TryGetValue("RawDpiX", out temp);
                return (double)temp;
            }
        }
        public static double DpiY
        {
            get
            {
                object temp;
                DeviceExtendedProperties.TryGetValue("RawDpiY", out temp);
                return (double)temp;
            }
        }
        public static double XInch = Application.Current.Host.Content.ActualWidth / DpiX;
        public static double YInch = Application.Current.Host.Content.ActualHeight / DpiY;
    }
}
