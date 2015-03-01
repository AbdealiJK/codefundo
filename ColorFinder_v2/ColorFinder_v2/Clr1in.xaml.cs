using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using ColorFinder_v2.Resources;
using System.IO.IsolatedStorage;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Tasks;
using System.Windows.Media;
using System.Windows.Input;
using System.IO;
using Microsoft.Xna.Framework.Media;

namespace ColorFinder_v2
{
	public partial class Clr1in : PhoneApplicationPage
	{
		String clr1;
		UIElement nowShowing;
		bool isLockedToggle = false;
		Point Point1, Point2;
		WriteableBitmap WriteableBMP;
		WriteableBitmap WB_CapturedImage;
		WriteableBitmap WB_CroppedImage;
		System.Windows.Controls.Image FinalCroppedImage = new System.Windows.Controls.Image();
		//private byte[] _imageBytes;

		CameraCaptureTask CCT = new CameraCaptureTask();

		string imageFolder = @"\Shared\ShellContent";

		string imageFileName = "DemoImage.jpg";
		public Clr1in()
		{
			InitializeComponent();
			CCT.Completed += new EventHandler<PhotoResult>(capture_completed);
		}
		//private void btnClick_Click(object sender, RoutedEventArgs e)
		//{
		//	btnClick.Visibility = Visibility.Collapsed;
		//	//CCT.Show();
		//	try
		//	{
		//		CCT.Show();
		//	}
		//	catch (System.InvalidOperationException ex)
		//	{
		//		MessageBox.Show("An error occurred");
		//	}
		//	imgOne.Visibility = Visibility.Visible;
		//	//chkclr.Visibility = Visibility.Collapsed;
		//	//coord.Visibility = Visibility.Visible;

		//	//CropBtn.Visibility = Visibility.Visible;
		//	CompositionTarget.Rendering += new EventHandler(CompositionTarget_Rendering);
		//}

		void capture_completed(object sender, PhotoResult e)
		{
			if (e.TaskResult == TaskResult.OK)
			{

				WriteableBMP = new WriteableBitmap(480,800);
				WriteableBMP.LoadJpeg(e.ChosenPhoto);
				//System.Windows.Media.Imaging.BitmapImage bmp = new System.Windows.Media.Imaging.BitmapImage();
				//bmp.SetSource(e.ChosenPhoto);
				//System.Windows.Controls.Image i1 = new System.Windows.Controls.Image();
				//i1.Source = bmp;
				//Canvas.SetLeft(i1,10);
				//Canvas.SetTop(i1,10);
				imgOne.Source = WriteableBMP;
				nowShowing = imgOne;
				//Using Isolated storage for storage
				using (var isoFile = IsolatedStorageFile.GetUserStoreForApplication())
				{
					if (!isoFile.DirectoryExists(imageFolder))
					{
						isoFile.CreateDirectory(imageFolder);
					}
					string filePath = System.IO.Path.Combine(imageFolder, imageFileName);
					using (var stream = isoFile.CreateFile(filePath))
					{
						WriteableBMP.SaveJpeg(stream, WriteableBMP.PixelWidth, WriteableBMP.PixelHeight, 0, 100);
					}
				}
			}
			//WB_CapturedImage = new WriteableBitmap(WriteableBMP);
		}

		private Point Center;
		private double InitialScale;

		private void GestureListener_PinchStarted(object sender, PinchStartedGestureEventArgs e)
		{
			//Testing pivotLock attribute
			if (!isLockedToggle)
			{
				return;
			}
			// Store the initial rotation angle and scaling
			InitialScale = (nowShowing.RenderTransform as CompositeTransform).ScaleX;
			// Calculate the center for the zooming
			Point firstTouch = e.GetPosition(nowShowing, 0);
			Point secondTouch = e.GetPosition(nowShowing, 1);
			Center = new Point(firstTouch.X + (secondTouch.X - firstTouch.X) / 2.0, firstTouch.Y + (secondTouch.Y - firstTouch.Y) / 2.0);
		}
		private void OnPinchDelta(object sender, PinchGestureEventArgs e)
		{
			//Testing pivotLock attribute
			if (!isLockedToggle)
			{
				return;
			}
			// If its less that the original size or more than 4x then don’t apply
			if (InitialScale * e.DistanceRatio > 4 || (InitialScale != 1 && e.DistanceRatio == 1) || InitialScale * e.DistanceRatio < 1)
				return;
			// If its original size then center it back
			if (e.DistanceRatio <= 1.08)
			{
				(nowShowing.RenderTransform as CompositeTransform).CenterX = 0;
				(nowShowing.RenderTransform as CompositeTransform).CenterY = 0;
				(nowShowing.RenderTransform as CompositeTransform).TranslateX = 0;
				(nowShowing.RenderTransform as CompositeTransform).TranslateY = 0;
			}
		(nowShowing.RenderTransform as CompositeTransform).CenterX = Center.X;
			(nowShowing.RenderTransform as CompositeTransform).CenterY = Center.Y;
			// Update the rotation and scaling
			if (this.Orientation == PageOrientation.Landscape)
			{
				// When in landscape we need to zoom faster, if not it looks choppy
				(nowShowing.RenderTransform as CompositeTransform).ScaleX = InitialScale * (1 + (e.DistanceRatio - 1) * 2);
			}
			else
			{
				(nowShowing.RenderTransform as CompositeTransform).ScaleX = InitialScale * e.DistanceRatio;
			}
		(nowShowing.RenderTransform as CompositeTransform).ScaleY = (nowShowing.RenderTransform as CompositeTransform).ScaleX;
		}
		private void Image_DragDelta(object sender, DragDeltaGestureEventArgs e)
		{
			//Testing pivotLock attribute
			if (!isLockedToggle)
			{
				return;
			}
			// if is not touch enabled or the scale is different than 1 then don’t allow moving
			if ((nowShowing.RenderTransform as CompositeTransform).ScaleX <= 1.1)
				return;
			double centerX = (nowShowing.RenderTransform as CompositeTransform).CenterX;
			double centerY = (nowShowing.RenderTransform as CompositeTransform).CenterY;
			double translateX = (nowShowing.RenderTransform as CompositeTransform).TranslateX;
			double translateY = (nowShowing.RenderTransform as CompositeTransform).TranslateY;
			double scale = (nowShowing.RenderTransform as CompositeTransform).ScaleX;
			double width = (nowShowing as Image).ActualWidth;
			double height = (nowShowing as Image).ActualHeight;
			// Verify limits to not allow the image to get out of area
			if (centerX - scale * centerX + translateX + e.HorizontalChange < 0 && centerX + scale * (width - centerX) + translateX + e.HorizontalChange > width)
			{
				(nowShowing.RenderTransform as CompositeTransform).TranslateX += e.HorizontalChange;
			}
			if (centerY - scale * centerY + translateY + e.VerticalChange < 0 && centerY + scale * (height - centerY) + translateY + e.VerticalChange > height)
			{
				(nowShowing.RenderTransform as CompositeTransform).TranslateY += e.VerticalChange;
			}
			return;
		}

		//private void chkclr_Click(object sender, RoutedEventArgs e)
		//{
		//	imgOne.Visibility = Visibility.Visible;
		//	chkclr.Visibility = Visibility.Collapsed;
		//	//coord.Visibility = Visibility.Visible;

		//	//CropBtn.Visibility = Visibility.Visible;
		//	CompositionTarget.Rendering += new EventHandler(CompositionTarget_Rendering);
		//}

		private void imgOne_DoubleTap(object sender, System.Windows.Input.GestureEventArgs e)
		{
			isLockedToggle = !isLockedToggle;
		}

		private void OriginalImage_MouseMove(object sender, MouseEventArgs e)
		{
			Point2 = e.GetPosition(imgOne);
		}
		//Mouse Up  
		private void OriginalImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			Point2 = e.GetPosition(imgOne);
		}
		//Mouse Down  
		private void OriginalImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			Point1 = e.GetPosition(imgOne);//Set first touchable coordinates as point1  
			Point2 = Point1;
			rect.Visibility = Visibility.Visible;
		}

		private void CompositionTarget_Rendering(object sender, EventArgs e)
		{
			//Used for rendering the cropping rectangle on the image.  
			rect.SetValue(Canvas.LeftProperty, (Point1.X < Point2.X) ? Point1.X : Point2.X);
			rect.SetValue(Canvas.TopProperty, (Point1.Y < Point2.Y) ? Point1.Y : Point2.Y);
			rect.Width = (int)Math.Abs(Point2.X - Point1.X);
			rect.Height = (int)Math.Abs(Point2.Y - Point1.Y);
		}

		private void CropBtn_Click(object sender, RoutedEventArgs e)
		{
			//CropBtn.Visibility = Visibility.Collapsed;
			// Get the size of the source image  
			double originalImageWidth = WriteableBMP.PixelWidth;
			double originalImageHeight = WriteableBMP.PixelHeight;

			// Get the size of the image when it is displayed on the phone  
			double displayedWidth = imgOne.ActualWidth;
			double displayedHeight = imgOne.ActualHeight;

			// Calculate the ratio of the original image to the displayed image  
			double widthRatio = originalImageWidth / displayedWidth;
			double heightRatio = originalImageHeight / displayedHeight;

			// Create a new WriteableBitmap. The size of the bitmap is the size of the cropping rectangle  
			// drawn by the user, multiplied by the image size ratio.  
			WB_CroppedImage = new WriteableBitmap((int)(widthRatio * Math.Abs(Point2.X - Point1.X)), (int)(heightRatio * Math.Abs(Point2.Y - Point1.Y)));

			// Calculate the offset of the cropped image. This is the distance, in pixels, to the top left corner  
			// of the cropping rectangle, multiplied by the image size ratio.  
			int xoffset = (int)(((Point1.X < Point2.X) ? Point1.X : Point2.X) * widthRatio);
			int yoffset = (int)(((Point1.Y < Point2.Y) ? Point1.Y : Point2.X) * heightRatio);

			// Copy the pixels from the targeted region of the source image into the target image,   
			// using the calculated offset  
			if (WB_CroppedImage.Pixels.Length > 0)
			{
				for (int i = 0; i < WB_CroppedImage.Pixels.Length; i++)
				{
					int x = (int)((i % WB_CroppedImage.PixelWidth) + xoffset);
					int y = (int)((i / WB_CroppedImage.PixelWidth) + yoffset);
					WB_CroppedImage.Pixels[i] = WriteableBMP.Pixels[y * WriteableBMP.PixelWidth + x];
				}

				// Set the source of the image control to the new cropped image  
				FinalCroppedImage.Source = WB_CroppedImage;
				//SaveBtn.Visibility = Visibility.Visible;

			}
			else
			{
				FinalCroppedImage.Source = null;
				SaveBtn.Visibility = Visibility.Collapsed;
			}
			//rect.Visibility = Visibility.Collapsed;  
		}

		private void SaveBtn_Click(object sender, RoutedEventArgs e)
		{
			//SaveBtn.Visibility = Visibility.Collapsed;
			CropBtn.Visibility = Visibility.Collapsed;
			show.Visibility = Visibility.Visible;
			try
			{
				String tempJPEG = "CroppedImage.jpg";
				//Create virtual store and file stream. Check for duplicate tempJPEG files.  
				var myStore = IsolatedStorageFile.GetUserStoreForApplication();
				if (myStore.FileExists(tempJPEG))
				{
					myStore.DeleteFile(tempJPEG);
				}
				IsolatedStorageFileStream myFileStream = myStore.CreateFile(tempJPEG);
				//Encode the WriteableBitmap into JPEG stream and place into isolated storage.  
				Extensions.SaveJpeg(WB_CroppedImage, myFileStream, WB_CroppedImage.PixelWidth, WB_CroppedImage.PixelHeight, 0, 85);
				myFileStream.Close();
				//Create a new file stream.  
				myFileStream = myStore.OpenFile(tempJPEG, FileMode.Open, FileAccess.Read);

				//Add the JPEG file to the photos library on the device.  
				MediaLibrary library = new MediaLibrary();
				Picture pic = library.SavePicture("SavedPicture.jpg", myFileStream);
				MessageBox.Show("Cropped image saved successfully to media library!");
				myFileStream.Close();
			}
			catch
			{
				MessageBox.Show("Error on image saving!");
			}

		}

		private void capture1(object sender, RoutedEventArgs e)
		{
			try
			{
				CCT.Show();
			}
			catch (System.InvalidOperationException ex)
			{
				MessageBox.Show("An error occurred");
			}
			imgOne.Visibility = Visibility.Visible;
			//chkclr.Visibility = Visibility.Collapsed;
			//coord.Visibility = Visibility.Visible;

			//CropBtn.Visibility = Visibility.Visible;
			CompositionTarget.Rendering += new EventHandler(CompositionTarget_Rendering);
		}

		public void show_Click(object sender, RoutedEventArgs e)
		{
			//coord.Visibility = Visibility.Collapsed;
			WriteableBitmap wbmp = new WriteableBitmap(WB_CroppedImage);
			int[] PixelData = wbmp.Pixels;
			int blue, green, red;
			blue = green = red = 0;
			for (int i = 0; i < PixelData.Length; i++)
			{
				byte[] colorArray = BitConverter.GetBytes(wbmp.Pixels[i]);
				blue += colorArray[0];
				green += colorArray[1];
				red += colorArray[2];
			}
			int b, g, r;
			b = blue / PixelData.Length;
			g = green / PixelData.Length;
			r = red / PixelData.Length;
			string bhex = b.ToString("X2");
			string ghex = g.ToString("X2");
			string rhex = r.ToString("X2");
			clr1 = "#" + rhex + "" + ghex + "" + bhex;
			PhoneApplicationService.Current.State["c1b"] = b;
			PhoneApplicationService.Current.State["c1g"] = g;
			PhoneApplicationService.Current.State["c1r"] = r;
			this.NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
		}
	}
}