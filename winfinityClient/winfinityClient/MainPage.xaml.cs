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
        UserCreate _myID;
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
            if (!_isKeyObtained)
                CreateIdLocal();
        }

        private void CreateIdLocal()
        {
            RestClient myClient = new RestClient("http://cfi.iitm.ac.in/webops/hackathon/hybriddevs/api/tempuser/");
            RestRequest postRequest = new RestRequest { Method = Method.POST, RequestFormat = DataFormat.Json };
            postRequest.AddParameter("type", "create",ParameterType.GetOrPost);
            try
            {
                myClient.ExecuteAsync(postRequest, postResponse =>
                {
                    if (postResponse.ResponseStatus == ResponseStatus.Completed)
                    {
                        _myID = JsonConvert.DeserializeObject<UserCreate>(postResponse.Content);
                        Dispatcher.BeginInvoke(() =>
                            {
                                DeviceUid.Text = _myID.data.key;
                            });
                        _isKeyObtained = true;
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        private void CreateRoom_Click(object sender, RoutedEventArgs e)
        {
            if (_myID != null)
            {
                PhoneApplicationService.Current.State["MyID"] = _myID;
                NavigationService.Navigate(new Uri("/createroom.xaml", UriKind.Relative));
            }
            else
            {
                MessageBox.Show("Error passing var");
            }
        }

        private void RefreshUid_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            CreateIdLocal();
        }

    }
}