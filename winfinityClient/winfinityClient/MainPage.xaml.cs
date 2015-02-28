using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Newtonsoft.Json;
using RestSharp;
using winfinityClient.Helpers;
using winfinityClient.Resources;

namespace winfinityClient
{
    public partial class MainPage : PhoneApplicationPage
    {
        bool _isKeyObtained = false;
        UserModel _myModel;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            TiltEffect.TiltableItems.Add(typeof(Grid));
            TiltEffect.TiltableItems.Add(typeof(Button));
            TransitionMod.UseTurnstileTransition(this);
            this.Loaded += MainPage_Loaded;
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            //Get UIDS
            //WebClient keyClient = new WebClient();
            //keyClient.DownloadStringCompleted += keyClient_DownloadStringCompleted;
            //if (!_isKeyObtained)
            //    keyClient.DownloadStringAsync(new Uri("http://cfi.iitm.ac.in/webops/hackathon/hybriddevs/api/tempuser", UriKind.Absolute));
            RestClient myClient = new RestClient("http://cfi.iitm.ac.in/webops/hackathon/hybriddevs/api/tempuser/");
            RestRequest postRequest = new RestRequest();
            postRequest.Method = Method.POST;
            postRequest.RequestFormat = DataFormat.Json;
            try
            {
                myClient.ExecuteAsync(postRequest, postResponse =>
                    {
                        if (postResponse.ResponseStatus == ResponseStatus.Completed)
                        {
                            
                        }
                    });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        void keyClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Dispatcher.BeginInvoke(() =>
                {
                    MessageBox.Show("This is not your fault. Something to do with your internet. " + e.Error.Message, "Connectivity Error", MessageBoxButton.OK);
                });
            }
            else
            {
                _myModel = new UserModel();
                _myModel = JsonConvert.DeserializeObject<UserModel>(e.Result);
                if (_myModel.message == "done")
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                        DeviceUid.Text = _myModel.data[0].key;
                    });
                    _isKeyObtained = true;
                }
                else
                {
                    MessageBox.Show("The server has some issues", "Error", MessageBoxButton.OK);
                }
            }
        }

        private void CreateRoom_Click(object sender, RoutedEventArgs e)
        {
            if (_myModel != null)
            {
                PhoneApplicationService.Current.State["MyUserModel"] = _myModel;
                NavigationService.Navigate(new Uri("/createroom.xaml", UriKind.Relative));
            }
            else
            {
                MessageBox.Show("Error passing var");
            }
        }

        private void RefreshUid_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            WebClient keyClient = new WebClient();
            keyClient.DownloadStringCompleted += keyClient_DownloadStringCompleted;
            keyClient.DownloadStringAsync(new Uri("http://cfi.iitm.ac.in/webops/hackathon/hybriddevs/api/tempuser", UriKind.Absolute));
        }

    }
}