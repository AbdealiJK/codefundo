using System;
using System.Windows;
using System.Windows.Controls;
using Windows.Networking.Proximity;
using Windows.Phone.Speech.Synthesis;
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
        long nfcId;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            TiltEffect.TiltableItems.Add(typeof(Grid));
            TiltEffect.TiltableItems.Add(typeof(Button));
            TransitionMod.UseTurnstileTransition(this);
            this.Loaded += MainPage_Loaded;
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            ProximityDevice device = ProximityDevice.GetDefault();
            if (device != null && nfcId != 0)
            {
                device.StopPublishingMessage(nfcId);
            }
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
                        PhoneApplicationService.Current.State["MyID"] = _myID;
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

        private void JoinRoom_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Playground.xaml?ismaster=" + "false", UriKind.Relative));
        }

        private void Speak_Click(object sender, RoutedEventArgs e)
        {
            SpeechSynthesizer synth = new SpeechSynthesizer();
            char[] tospeak = _myID.data.key.ToCharArray(0, 6);
            string stringtospeak = "";
            for (int i = 0; i < 6; ++i)
            {
                stringtospeak += tospeak[i];
                stringtospeak += " ";
            }
            synth.SpeakTextAsync(stringtospeak);
        }

        private void NFCSend_Click(object sender, RoutedEventArgs e)
        {
            ProximityDevice device = ProximityDevice.GetDefault();
            // Make sure NFC is supported
            if (device != null)
            {
                nfcId = device.PublishMessage("Windows.SampleMessageType", _myID.data.key);
            }
        }

    }
}