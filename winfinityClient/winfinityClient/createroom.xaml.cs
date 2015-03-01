using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using Windows.Phone.Speech.Recognition;
using Coding4Fun.Toolkit.Controls;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using Newtonsoft.Json;
using RestSharp;
using winfinityClient.Helpers;
using TiltEffect = Microsoft.Phone.Controls.TiltEffect;

namespace winfinityClient
{
    // ReSharper disable once InconsistentNaming
    public partial class createroom : PhoneApplicationPage
    {
        string[] _deviceIdList;
        int _noDevices;
        UserCreate _myID;
        RoomCreate _myRoom;
        RoomAddUser _myWorkspace;

        public createroom()
        {
            InitializeComponent();
            //config
            TiltEffect.TiltableItems.Add(typeof(Grid));
            TiltEffect.TiltableItems.Add(typeof(Button));
            TransitionMod.UseTurnstileTransition(this);
            MessageBox.Show("Tap on device to assign UID to them", "Tip", MessageBoxButton.OK);
            {
                _deviceIdList = new string[4];
                for (int i = 0; i < 4; i++)
                {
                    _deviceIdList[i] = string.Empty;
                }
            }
            _noDevices = 4;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _myID = (UserCreate)PhoneApplicationService.Current.State["MyID"];
            Device1.Content = _myID.data.key;
            _deviceIdList[0] = _myID.data.key;
        }

        private void TwoDevice_Click(object sender, RoutedEventArgs e)
        {
            DevicePanelFour.Visibility = Visibility.Collapsed;
            Device1.Height = 400;
            Device2.Height = 400;
            _noDevices = 2;
            TwoDevice.Background = new SolidColorBrush((Color)Application.Current.Resources["PhoneAccentColor"]);
            FourDevice.Background = new SolidColorBrush(Colors.Transparent);
            if (isInputComplete())
                DoneSetup.IsEnabled = true;
        }

        private void FourDevice_Click(object sender, RoutedEventArgs e)
        {
            DevicePanelFour.Visibility = Visibility.Visible;
            Device1.Height = 200;
            Device2.Height = 200;
            _noDevices = 4;
            FourDevice.Background = new SolidColorBrush((Color)Application.Current.Resources["PhoneAccentColor"]);
            TwoDevice.Background = new SolidColorBrush(Colors.Transparent);
            if (isInputComplete())
                DoneSetup.IsEnabled = true;
        }

        private void Device1_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("This is your device. Your own ID will be used for this device", "Alert", MessageBoxButton.OK);
        }

        private void Device2_Click(object sender, RoutedEventArgs e)
        {
            InputPrompt input2 = new InputPrompt();
            input2.Completed += input2_Completed;
            input2.Title = "Setup device 2";
            input2.Message = "Enter the UID of device 2";
            input2.InputScope = new InputScope { Names = { new InputScopeName() { NameValue = InputScopeNameValue.Number } } };
            input2.Show();
        }

        void input2_Completed(object sender, PopUpEventArgs<string, PopUpResult> e)
        {
            if (e.Result != string.Empty)
            {
                Device2.Background = new SolidColorBrush(Colors.Green);
                _deviceIdList[1] = e.Result;
                Device2.Content = e.Result;
                if (isInputComplete())
                    DoneSetup.IsEnabled = true;
            }
        }

        private void Device3_Click(object sender, RoutedEventArgs e)
        {
            InputPrompt input3 = new InputPrompt();
            input3.Completed += input3_Completed;
            input3.Title = "Setup Device 3";
            input3.Message = "Enter the UID of device 3";
            input3.InputScope = new InputScope { Names = { new InputScopeName() { NameValue = InputScopeNameValue.Number } } };
            input3.Show();
        }

        void input3_Completed(object sender, PopUpEventArgs<string, PopUpResult> e)
        {
            if (e.Result != string.Empty)
            {
                Device3.Background = new SolidColorBrush(Colors.Green);
                _deviceIdList[2] = e.Result;
                Device3.Content = e.Result;
                if (isInputComplete())
                    DoneSetup.IsEnabled = true;
            }
        }

        private void Device4_Click(object sender, RoutedEventArgs e)
        {
            InputPrompt input4 = new InputPrompt();
            input4.Completed += input4_Completed;
            input4.Title = "Setup Device 4";
            input4.Message = "Enter the UID of device 4";
            input4.InputScope = new InputScope { Names = { new InputScopeName() { NameValue = InputScopeNameValue.Number } } };
            input4.Show();
        }

        void input4_Completed(object sender, PopUpEventArgs<string, PopUpResult> e)
        {
            if (e.Result != string.Empty)
            {
                Device4.Background = new SolidColorBrush(Colors.Green);
                _deviceIdList[3] = e.Result;
                Device4.Content = e.Result;
            }
            if (isInputComplete())
                DoneSetup.IsEnabled = true;
        }

        private void DoneSetup_Click(object sender, RoutedEventArgs e)
        {
            int nullCounter = 0;
            for (int i = 0; i < _noDevices; i++)
            {
                if (_deviceIdList[i] == string.Empty || _deviceIdList[i] == null)
                    ++nullCounter;
            }

            if (nullCounter == 0)
            {
                //Setup workspace
                CreateNewRoom();


            }
            else
            {
                Dispatcher.BeginInvoke(() => { MessageBox.Show("Enter Device IDs for all devices"); });
            }
        }

        void photoGet_Completed(object sender, PhotoResult e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
                return;
            }
            if (e.ChosenPhoto == null)
            {
                Dispatcher.BeginInvoke(() => { MessageBox.Show("Null image"); });
                return;
            }

            //upload image
            RestClient client = new RestClient(UriMod.RoomUri);
            RestRequest request = new RestRequest { Method = Method.POST };
            request.AddParameter("type", "file", ParameterType.GetOrPost);
            request.AddParameter("room_id", _myRoom.data.room_id, ParameterType.GetOrPost);
            request.AddParameter("key", _myID.data.key, ParameterType.GetOrPost);
            request.AddFile("file", e.ChosenPhoto.ToByteArray(), e.OriginalFileName);
            try
            {
                client.ExecuteAsync(request, response =>
                {
                    if (response.ResponseStatus == ResponseStatus.Completed)
                    {
                        _myWorkspace = JsonConvert.DeserializeObject<RoomAddUser>(response.Content);
                        PhoneApplicationService.Current.State["MyWS"] = _myWorkspace;

                        //show output
                        Dispatcher.BeginInvoke(() =>
                        {
                            ToastPrompt toast = new ToastPrompt
                            {
                                Message = "Image Added",
                                MillisecondsUntilHidden = 1000
                            };
                            toast.Show();
                            if (SystemTray.ProgressIndicator != null)
                                SystemTray.ProgressIndicator.IsVisible = false;
                            SystemTray.IsVisible = false;
                            NavigationService.Navigate(new Uri("/Playground.xaml?ismaster=" + "true", UriKind.Relative));
                        });
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            //Show SysTray
            SystemTray.ProgressIndicator = new ProgressIndicator();
            SystemTray.ProgressIndicator.IsIndeterminate = true;
            SystemTray.ProgressIndicator.Text = "Uploading Image";
            SystemTray.ProgressIndicator.IsVisible = true;
            SystemTray.IsVisible = true;

            //Navigate to image workarea
            MessageBox.Show("Please wait until the image is done uploading");
        }

        private void CreateNewRoom()
        {
            RestClient client = new RestClient(UriMod.RoomUri);
            RestRequest request = new RestRequest { Method = Method.POST };
            request.AddParameter("type", "create", ParameterType.GetOrPost);
            request.AddParameter("configuration", (_noDevices == 2) ? "2" : "3", ParameterType.GetOrPost);
            request.AddParameter("key", _myID.data.key, ParameterType.GetOrPost);
            try
            {
                client.ExecuteAsync(request, response =>
                {
                    if (response.ResponseStatus == ResponseStatus.Completed)
                    {
                        _myRoom = JsonConvert.DeserializeObject<RoomCreate>(response.Content);
                        Dispatcher.BeginInvoke(() =>
                        {
                            RoomTitle.Text = "room id " + _myRoom.data.room_id.ToString();
                        });
                        switch (_noDevices)
                        {
                            case 2:
                                AddUserToRoom((string)Device2.Content, _myRoom, 1);
                                break;
                            case 4:
                                AddUserToRoom((string)Device2.Content, _myRoom, 1);
                                AddUserToRoom((string)Device3.Content, _myRoom, 2);
                                AddUserToRoom((string)Device4.Content, _myRoom, 3);
                                break;
                        }
                        Dispatcher.BeginInvoke(() =>
                        {
                            ImageUpload.IsEnabled = true;
                        });
                    }
                });
            }
            catch (Exception e)
            {
                Dispatcher.BeginInvoke(() => { MessageBox.Show(e.Message); });
            }
        }

        private void AddUserToRoom(string id, RoomCreate room, int position)
        {
            RestClient client = new RestClient(UriMod.RoomUri);
            RestRequest request = new RestRequest { Method = Method.POST };
            request.AddParameter("type", "add", ParameterType.GetOrPost);
            request.AddParameter("room_id", room.data.room_id, ParameterType.GetOrPost);
            request.AddParameter("new_key", id, ParameterType.GetOrPost);
            request.AddParameter("key", _myID.data.key, ParameterType.GetOrPost);
            request.AddParameter("position", position.ToString(), ParameterType.GetOrPost);
            try
            {
                client.ExecuteAsync(request, response =>
                {
                    if (response.ResponseStatus == ResponseStatus.Completed)
                    {
                        Dispatcher.BeginInvoke(() =>
                        {
                            ToastPrompt toast = new ToastPrompt
                            {
                                Message = "User added",
                                MillisecondsUntilHidden = 2000
                            };
                            toast.Show();
                        });
                        _myWorkspace = JsonConvert.DeserializeObject<RoomAddUser>(response.Content);
                    }
                });
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void ImageUpload_Click(object sender, RoutedEventArgs e)
        {
            PhotoChooserTask photoGet = new PhotoChooserTask();
            photoGet.ShowCamera = true;
            photoGet.Completed += photoGet_Completed;
            photoGet.Show();
        }

        private bool isInputComplete()
        {
            for (int i = 0; i < _noDevices; i++)
            {
                if (_deviceIdList[i] == string.Empty || _deviceIdList[i] == null)
                    return false;
            }
            return true;
        }

        private async void Device2_DoubleTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            SpeechRecognizerUI mic = new SpeechRecognizerUI();
            mic.Settings.ExampleText = "Hold your device near phone which creates the room";
            mic.Settings.ListenText = "Recognition active";
            mic.Settings.ShowConfirmation = true;
            SpeechRecognitionUIResult res = await mic.RecognizeWithUIAsync();
            if (res.RecognitionResult.TextConfidence == SpeechRecognitionConfidence.Medium || res.RecognitionResult.TextConfidence == SpeechRecognitionConfidence.High)
            {
                if (res.RecognitionResult.Text != string.Empty)
                {
                    string result = res.RecognitionResult.Text.Replace('.','\0');
                    Device2.Background = new SolidColorBrush(Colors.Green);
                    _deviceIdList[1] = result;
                    Device2.Content = result;
                    if (isInputComplete())
                        DoneSetup.IsEnabled = true;
                }
            }
        }

        private async void Device3_DoubleTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            SpeechRecognizerUI mic = new SpeechRecognizerUI();
            mic.Settings.ExampleText = "Hold your device near phone which creates the room";
            mic.Settings.ListenText = "Recognition active";
            mic.Settings.ShowConfirmation = true;
            SpeechRecognitionUIResult res = await mic.RecognizeWithUIAsync();
            if (res.RecognitionResult.TextConfidence == SpeechRecognitionConfidence.Medium || res.RecognitionResult.TextConfidence == SpeechRecognitionConfidence.High)
            {
                if (res.RecognitionResult.Text != string.Empty)
                {
                    string result = res.RecognitionResult.Text.Replace('.','\0');
                    Device3.Background = new SolidColorBrush(Colors.Green);
                    _deviceIdList[2] = result;
                    Device3.Content = result;
                    if (isInputComplete())
                        DoneSetup.IsEnabled = true;
                }
            }
        }

        private async void Device4_DoubleTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
             SpeechRecognizerUI mic = new SpeechRecognizerUI();
            mic.Settings.ExampleText = "Hold your device near phone which creates the room";
            mic.Settings.ListenText = "Recognition active";
            mic.Settings.ShowConfirmation = true;
            SpeechRecognitionUIResult res = await mic.RecognizeWithUIAsync();
            if (res.RecognitionResult.TextConfidence == SpeechRecognitionConfidence.Medium || res.RecognitionResult.TextConfidence == SpeechRecognitionConfidence.High)
            {
                if (res.RecognitionResult.Text != string.Empty)
                {
                    string result = res.RecognitionResult.Text.Replace('.','\0');
                    Device4.Background = new SolidColorBrush(Colors.Green);
                    _deviceIdList[3] = result;
                    Device4.Content = result;
                    if (isInputComplete())
                        DoneSetup.IsEnabled = true;
                }
            }
        }
    }
}