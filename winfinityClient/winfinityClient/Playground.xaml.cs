using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Threading;
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
        BitmapImage _targetImage;

        Point _center;

        public Playground()
        {
            InitializeComponent();
            TransitionMod.UseTurnstileTransition(this);
            _center = new Point(ScreenSizeMod.XPixels / 2.0, ScreenSizeMod.YPixels / 2.0);
            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //_myId = (UserCreate)PhoneApplicationService.Current.State["MyID"];
            //GetBaseProps();
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
                            _targetImage = new BitmapImage(new Uri(_myId.data.rooms[0].shared_file, UriKind.Absolute));
                            //BehaviorRoot.CurrentID = _myId;
                            //BehaviorRoot.CurrentRoom = _myRoom;
                            if (_myId.data.position == 0)
                                ImageField.Source = _targetImage;
                            //BehaviorRoot.ImgHeight = _targetImage.PixelHeight;
                            //BehaviorRoot.ImgWidth = _targetImage.PixelWidth;
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