using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Newtonsoft.Json;
using RestSharp;
using winfinityClient.Helpers;

namespace winfinityClient
{
    public partial class Playground : PhoneApplicationPage
    {
        UserCreate _myId;
        RoomAddUser _myRoom;
        public Playground()
        {
            InitializeComponent();
            TransitionMod.UseTurnstileTransition(this);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _myId = (UserCreate)PhoneApplicationService.Current.State["MyID"];
            GetBaseProps();
        }

        private void GetBaseProps()
        {
            RestClient myClient = new RestClient(UriMod.UserUri);
            RestRequest getRequest = new RestRequest { Method = Method.GET, RequestFormat = DataFormat.Json };
            getRequest.AddParameter("key", _myId.data.key, ParameterType.GetOrPost);
            try
            {
                myClient.ExecuteAsync(getRequest, getResponse =>
                {
                    if (getResponse.ResponseStatus == ResponseStatus.Completed)
                    {
                        _myId = JsonConvert.DeserializeObject<UserCreate>(getResponse.Content);
                        Dispatcher.BeginInvoke(() =>
                        {
                            ImageField.Source = new BitmapImage(new Uri(_myId.data.rooms[0].shared_file, UriKind.Absolute));
                        });
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }


    }
}