using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Threading;
using Coding4Fun.Toolkit.Controls;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Newtonsoft.Json;
using RestSharp;
using winfinityClient.Helpers;
using System.Threading.Tasks;

namespace winfinityClient
{
    public partial class Playground : PhoneApplicationPage
    {
        UserCreate _myId;
        //RoomAddUser _myRoom;
        BitmapImage _targetImage;
        DispatcherTimer _timer;
        EventResult _result;
        double ImgHeight;
        double ImgWidth;

        Point _center;

        public Playground()
        {
            InitializeComponent();
            TransitionMod.UseTurnstileTransition(this);
            _center = new Point(ScreenSizeMod.XPixels / 2.0, ScreenSizeMod.YPixels / 2.0);
            _timer = new DispatcherTimer();
            _timer.Interval = new TimeSpan(0, 0, 4);
            _timer.Tick += _timer_Tick;
        }

        void _timer_Tick(object sender, EventArgs e)
        {
            RestClient client = new RestClient(UriMod.EventUri);
            RestRequest request = new RestRequest();
            request.Method = Method.GET;
            request.AddParameter("room_id", _myId.data.rooms[0].room_id, ParameterType.GetOrPost);
            request.AddParameter("user_key", _myId.data.key, ParameterType.GetOrPost);
            try
            {
                client.ExecuteAsync(request, response =>
                {
                    if (response.ResponseStatus == ResponseStatus.Completed)
                    {
                        _result = JsonConvert.DeserializeObject<EventResult>(response.Content);
                        Dispatcher.BeginInvoke(() =>
                        {
                            ToastPrompt toast = new ToastPrompt();
                            toast.Message = "Polling complete " + _result.data.x1 + "," + _result.data.x2;
                            toast.MillisecondsUntilHidden = 1000;
                            toast.Show();
                        });
                        AltMoveImage(_result.data);
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        private void moveImage(BoundBox box)
        {
            CompositeTransform old = ImageField.RenderTransform as CompositeTransform;
            var translate = new CompositeTransform
            {
                TranslateX = (box.x2 - box.x1) * box.x1 / ScreenSizeMod.XPixels,
                TranslateY = (box.y2 - box.y1) * box.y1 / ScreenSizeMod.YPixels
            };

            ImageField.RenderTransform = ComposeScaleTranslate(old, translate);
        }

        private void AltMoveImage(BoundBox box)
        {
            if (!BehaviorRoot.hasfirstbox)
            {
                BehaviorRoot.hasfirstbox = true;
                BehaviorRoot.firstbox = box;
                //if (_myId.data.position != 0)
                //    BehaviorRoot.bbox.Pan(box.x1, box.y1);
            }
            BehaviorRoot.bbox = box;
            CompositeTransform old = ImageField.RenderTransform as CompositeTransform;
            double MoveX = -(box.x1) / (box.x2 - box.x1) * ScreenSizeMod.XPixels;
            double MoveY = (box.y1) / (box.y2 - box.y1) * ScreenSizeMod.YPixels;
            old.TranslateX = MoveX;
            old.TranslateY = MoveY;
            ImageField.RenderTransform = old;
        }

        private static CompositeTransform ComposeScaleTranslate(CompositeTransform fst, CompositeTransform snd)
        {
            // See http://stackoverflow.com/a/19439099/388010 on why this works
            return new CompositeTransform
            {
                ScaleX = fst.ScaleX * snd.ScaleX,
                ScaleY = fst.ScaleY * snd.ScaleY,
                CenterX = fst.CenterX,
                CenterY = fst.CenterY,
                TranslateX = snd.TranslateX + snd.ScaleX * fst.TranslateX + (snd.ScaleX - 1) * (fst.CenterX - snd.CenterX),
                TranslateY = snd.TranslateY + snd.ScaleY * fst.TranslateY + (snd.ScaleY - 1) * (fst.CenterY - snd.CenterY),
            };
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
                            _targetImage = new BitmapImage(new Uri(_myId.data.rooms[0].shared_file, UriKind.Absolute));
                            _targetImage.CreateOptions = BitmapCreateOptions.BackgroundCreation;
                            Task.Delay(2000);
                            BehaviorRoot.CurrentID = _myId;
                            ImageField.Source = _targetImage;
                            BehaviorRoot.ImgHeight = 2592;//_targetImage.PixelHeight;
                            BehaviorRoot.ImgWidth = 1456;// _targetImage.PixelWidth;
                            ImgWidth = 1456;//_targetImage.PixelWidth;
                            ImgHeight = 2592; //_targetImage.PixelHeight;
                            ToastPrompt toast = new ToastPrompt
                            {
                                Message = "Base props " + ImgHeight + " " + ImgWidth,
                                MillisecondsUntilHidden = 1000
                            };
                            AdminRegister();
                            toast.Show();
                            _timer.Start();
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

        private void AdminRegister()
        {
            BoundBox box = new BoundBox();
            bool isWidthFit = !((double)ImgHeight / ImgWidth > (double)ScreenSizeMod.YPixels / ScreenSizeMod.XPixels);
            if (isWidthFit)
            {
                box.x1 = 0;
                box.x2 = ImgWidth;
                box.y1 = 0;
                box.y2 = +(ScreenSizeMod.YPixels / ScreenSizeMod.XPixels * ImgWidth);
            }
            else
            {
                box.y1 = 0;
                box.y2 = ImgHeight;
                box.x1 = 0;
                box.x2 = +(ScreenSizeMod.XPixels / ScreenSizeMod.YPixels * ImgHeight);
            }

            if (_myId.data.position == 0)
            {
                RestClient client = new RestClient(UriMod.EventUri);
                RestRequest request = new RestRequest();
                request.Method = Method.POST;
                request.AddParameter("room_id", _myId.data.rooms[0].room_id, ParameterType.GetOrPost);
                request.AddParameter("user_key", _myId.data.key, ParameterType.GetOrPost);
                request.AddParameter("bbox_x1", box.x1, ParameterType.GetOrPost);
                request.AddParameter("bbox_x2", box.x2, ParameterType.GetOrPost);
                request.AddParameter("bbox_y1", box.y1, ParameterType.GetOrPost);
                request.AddParameter("bbox_y2", box.y2, ParameterType.GetOrPost);
                try
                {
                    client.ExecuteAsync(request, response =>
                    {
                        if (response.ResponseStatus == ResponseStatus.Completed)
                        {
                            Dispatcher.BeginInvoke(() =>
                            {
                                ToastPrompt toast = new ToastPrompt();
                                toast.Message = "Admin registered";
                                toast.MillisecondsUntilHidden = 1000;
                                toast.Show();
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
}