using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media;
using Coding4Fun.Toolkit.Controls;
using Microsoft.Phone.Controls;
using RestSharp;

namespace winfinityClient.Helpers
{
    public class PanAndZoomBehavior : Behavior<FrameworkElement>
    {
        private const double MinZoom = 1.0;
        private readonly CompositeTransform _old = new CompositeTransform();
        private double _initialScale;
        private GestureListener _listener;
        public UserCreate CurrentID { get; set; }
        //image dimension
        public double ImgWidth { get; set; }
        public double ImgHeight { get; set; }
        public Point ImageCenter;
        bool isWidthFit;
        public BoundBox bbox;
        public BoundBox firstbox;
        public bool hasfirstbox;

        public PanAndZoomBehavior()
        {
            bbox = new BoundBox();
            firstbox = new BoundBox();
            hasfirstbox = false;
            ImgWidth = 1456;//_targetImage.PixelWidth;
            ImgHeight = 2592; //_targetImage.PixelHeight;
            MaxZoom = 10.0;
            ImageCenter = new Point(ScreenSizeMod.XPixels / 2.0, ScreenSizeMod.YPixels / 2.0);
            isWidthFit = !((double)ImgHeight / ImgWidth > (double)ScreenSizeMod.YPixels / ScreenSizeMod.XPixels);
            if (isWidthFit)
            {
                bbox.x1 = 0;
                bbox.x2 = ImgWidth;
                bbox.y1 = 0;
                bbox.y2 = +(ScreenSizeMod.YPixels / ScreenSizeMod.XPixels * ImgWidth);
            }
            else
            {
                bbox.y1 = 0;
                bbox.y2 = ImgHeight;
                bbox.x1 = 0;
                bbox.x2 = +(ScreenSizeMod.XPixels / ScreenSizeMod.YPixels * ImgHeight);
            }
        }

        /// <summary>
        /// This does not enforce zoom bounds on setting.
        /// </summary>
        public double MaxZoom { get; set; }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.RenderTransform = new CompositeTransform();
            _listener = GestureService.GetGestureListener(AssociatedObject);
            _listener.PinchDelta += OnPinchDelta;
            _listener.PinchStarted += OnPinchStarted;
            _listener.PinchCompleted += _listener_PinchCompleted;
            _listener.DragCompleted += _listener_DragCompleted;
            _listener.DragDelta += OnDragDelta;
            // wait for the RootVisual to be initialized
            Dispatcher.BeginInvoke(() =>
                ((PhoneApplicationFrame)Application.Current.RootVisual).OrientationChanged += OrientationChanged);
        }

        void _listener_DragCompleted(object sender, DragCompletedGestureEventArgs e)
        {
            ImageCenter.X += e.HorizontalChange;
            ImageCenter.Y += e.VerticalChange;
            bbox.Pan(-e.HorizontalChange / ScreenSizeMod.XPixels * ImgWidth, e.VerticalChange / ScreenSizeMod.YPixels * ImgHeight);
            
            //Send bbox to server
            RestClient client = new RestClient(UriMod.EventUri);
            RestRequest request = new RestRequest();
            request.Method = Method.POST;
            request.AddParameter("room_id", CurrentID.data.rooms[0].room_id, ParameterType.GetOrPost);
            request.AddParameter("user_key", CurrentID.data.key, ParameterType.GetOrPost);
            request.AddParameter("bbox_x1", bbox.x1, ParameterType.GetOrPost);
            request.AddParameter("bbox_x2", bbox.x2, ParameterType.GetOrPost);
            request.AddParameter("bbox_y1", bbox.y1, ParameterType.GetOrPost);
            request.AddParameter("bbox_y2", bbox.y2, ParameterType.GetOrPost);
            try
            {
                client.ExecuteAsync(request, response =>
                    {
                        if (response.ResponseStatus == ResponseStatus.Completed)
                        {
                            Dispatcher.BeginInvoke(() =>
                            {
                                ToastPrompt toast = new ToastPrompt();
                                toast.Message = "Drag complete posted";
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

        void _listener_PinchCompleted(object sender, PinchGestureEventArgs e)
        {

        }

        protected override void OnDetaching()
        {
            ((PhoneApplicationPage)Application.Current.RootVisual).OrientationChanged -= OrientationChanged;
            _listener.PinchDelta -= OnPinchDelta;
            _listener.PinchStarted -= OnPinchStarted;
            _listener.DragDelta -= OnDragDelta;
            _listener = null;
            base.OnDetaching();
        }

        private void OnPinchDelta(object sender, PinchGestureEventArgs e)
        {
            var img = sender as Image;
            var transform = img.RenderTransform as CompositeTransform;
            var a = transform.Transform(e.GetPosition(img, 0)); // we need the points to be relative to the current transform
            var b = transform.Transform(e.GetPosition(img, 1));

            var scale = new CompositeTransform
            {
                CenterX = (a.X + b.X) / 2,
                CenterY = (a.Y + b.Y) / 2,
                ScaleX = Clamp(e.DistanceRatio * _initialScale / _old.ScaleX,
                    MinZoom / _old.ScaleX,
                    MaxZoom / _old.ScaleX),
                ScaleY = Clamp(e.DistanceRatio * _initialScale / _old.ScaleY,
                    MinZoom / _old.ScaleY,
                    MaxZoom / _old.ScaleY)
            };

            ConstrainToParentBounds(img, scale);

            transform = ComposeScaleTranslate(transform, scale);
            img.RenderTransform = transform;

            _old.CenterX = transform.CenterX;
            _old.CenterY = transform.CenterY;
            _old.TranslateX = transform.TranslateX;
            _old.TranslateY = transform.TranslateY;
            _old.ScaleX = transform.ScaleX;
            _old.ScaleY = transform.ScaleY;
        }

        private void OnPinchStarted(object sender, PinchStartedGestureEventArgs e)
        {
            var img = sender as Image;
            var transform = img.RenderTransform as CompositeTransform;

            _old.CenterX = transform.CenterX;
            _old.CenterY = transform.CenterY;
            _old.TranslateX = transform.TranslateX;
            _old.TranslateY = transform.TranslateY;
            _old.ScaleX = transform.ScaleX;
            _old.ScaleY = transform.ScaleY;
            _initialScale = transform.ScaleX;
        }

        private static void OnDragDelta(object sender, DragDeltaGestureEventArgs e)
        {
            // Translation is done as the last operation, so no need to move the operation up in composition order
            var img = sender as Image;
            var transform = img.RenderTransform as CompositeTransform;

            var translate = new CompositeTransform
            {
                TranslateX = e.HorizontalChange,
                TranslateY = e.VerticalChange
            };

            //ConstrainToParentBounds(img, translate);

            img.RenderTransform = ComposeScaleTranslate(transform, translate);
        }

        private static void ConstrainToParentBounds(FrameworkElement elm, CompositeTransform transform)
        {
            var p = (FrameworkElement)elm.Parent;
            var canvas = p.TransformToVisual(elm).TransformBounds(new Rect(0, 0, p.ActualWidth, p.ActualHeight));
            // Now compute the new viewport relative to the previous
            var newViewport = transform.TransformBounds(new Rect(0, 0, elm.ActualWidth, elm.ActualHeight));

            var top = newViewport.Top - canvas.Top;
            var bottom = canvas.Bottom - newViewport.Bottom;
            var left = newViewport.Left - canvas.Left;
            var right = canvas.Right - newViewport.Right;

            if (top > 0)
                if (top + bottom > 0)
                    transform.TranslateY += (bottom - top) / 2;
                else
                    transform.TranslateY -= top;
            else if (bottom > 0)
                if (top + bottom > 0)
                    transform.TranslateY += (bottom - top) / 2;
                else
                    transform.TranslateY += bottom;

            if (left > 0)
                if (left + right > 0)
                    transform.TranslateX += (right - left) / 2;
                else
                    transform.TranslateX -= left;
            else if (right > 0)
                if (left + right > 0)
                    transform.TranslateX += (right - left) / 2;
                else
                    transform.TranslateX += right;
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

        private static double Clamp(double val, double min, double max)
        {
            return val > min ? val < max ? val : max : min;
        }

        private void OrientationChanged(object sender, OrientationChangedEventArgs e)
        {
            // Handling orientation change is a heck more involved than I initially thought
            AssociatedObject.RenderTransform = new CompositeTransform();
        }
    }

}
