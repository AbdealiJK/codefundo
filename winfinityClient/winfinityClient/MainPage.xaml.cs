using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Newtonsoft.Json;
using RestSharp;
using winfinityClient.Helpers;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

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
            RestClient myClient = new RestClient(UriMod.UserUri);
            RestRequest postRequest = new RestRequest { Method = Method.POST, RequestFormat = DataFormat.Json };
            postRequest.AddParameter("type", "create", ParameterType.GetOrPost);
            postRequest.AddParameter("size_x", ScreenSizeMod.XInch, ParameterType.GetOrPost);
            postRequest.AddParameter("size_y", ScreenSizeMod.YInch, ParameterType.GetOrPost);
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

        private void RefreshUid_Tap(object sender, GestureEventArgs e)
        {
            CreateIdLocal();
        }

    }
}