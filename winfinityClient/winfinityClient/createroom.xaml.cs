using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Coding4Fun.Toolkit.Controls;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using winfinityClient.Helpers;
using TiltEffect = Microsoft.Phone.Controls.TiltEffect;

namespace winfinityClient
{
    // ReSharper disable once InconsistentNaming
    public partial class createroom : PhoneApplicationPage
    {
        List<string> _deviceIdList;
        public createroom()
        {
            InitializeComponent();
            //config
            TiltEffect.TiltableItems.Add(typeof(Grid));
            TiltEffect.TiltableItems.Add(typeof(Button));
            TransitionMod.UseTurnstileTransition(this);
            MessageBox.Show("Tap on device to assign UID to them", "Tip", MessageBoxButton.OK);
            _deviceIdList = new List<string>(4);
        }

        private void TwoDevice_Click(object sender, RoutedEventArgs e)
        {
            DevicePanelFour.Visibility = System.Windows.Visibility.Collapsed;
            Device1.Height = 500;
            Device2.Height = 500;
        }

        private void FourDevice_Click(object sender, RoutedEventArgs e)
        {
            DevicePanelFour.Visibility = System.Windows.Visibility.Visible;
            Device1.Height = 200;
            Device2.Height = 200;
        }

        private void Device1_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("This is your device. Your own ID will be used for this device", "Alert", MessageBoxButton.OK);
        }

        private void Device2_Click(object sender, RoutedEventArgs e)
        {
            InputPrompt input2 = new InputPrompt();
            input2.Completed += input2_Completed;
            input2.Title = "Basic Input";
            input2.Message = "I'm a basic input prompt";
            input2.Show();
        }

        void input2_Completed(object sender, PopUpEventArgs<string, PopUpResult> e)
        {
            if (e.Result != string.Empty)
            {
                _deviceIdList[2] = e.Result;
            }
        }

        private void Device3_Click(object sender, RoutedEventArgs e)
        {
            InputPrompt input3 = new InputPrompt();
            input3.Completed += input3_Completed;
            input3.Title = "Basic Input";
            input3.Message = "I'm a basic input prompt";
            input3.Show();
        }

        void input3_Completed(object sender, PopUpEventArgs<string, PopUpResult> e)
        {
            throw new NotImplementedException();
        }

        private void Device4_Click(object sender, RoutedEventArgs e)
        {
            InputPrompt input4 = new InputPrompt();
            input4.Completed += input4_Completed;
            input4.Title = "Basic Input";
            input4.Message = "I'm a basic input prompt";
            input4.Show();
        }

        void input4_Completed(object sender, PopUpEventArgs<string, PopUpResult> e)
        {
            throw new NotImplementedException();
        }
    }
}