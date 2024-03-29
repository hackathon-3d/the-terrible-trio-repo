﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage.Streams;
using Windows.System.Threading;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using ZXing;
using ZXing.QrCode;

namespace VisualMove
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public partial class QRCameraPage : Page
    {
        #region Enumerations
        private enum CameraLocation
        {
            Front,
            Back
        }
        #endregion

        #region Data Members
        private bool m_bTakePictures;
        private bool m_bBoxMatchingMode;
        #endregion

        #region Constructors
        public QRCameraPage()
        {
            DataContext = this;

            this.InitializeComponent();
        }
        #endregion

        #region Page
        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            DeviceInformationCollection oCameras = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);
            switch (oCameras.Count)
            {
                case 0:
                    throw new Exception("No cameras found");
                case 1:
                    //Only front camera is available
                    Camera = oCameras[(int)CameraLocation.Front];
                    break;
                default:
                    //By default, we want the back camera
                    Camera = oCameras[(int)CameraLocation.Back];
                    break;
            }

            MediaCaptureInitializationSettings oCameraSettings = new MediaCaptureInitializationSettings();
            oCameraSettings.VideoDeviceId = Camera.Id;

            MediaCapture oCamera = new MediaCapture();
            await oCamera.InitializeAsync(oCameraSettings);

            // resolution variables
            //int iMaxResolution = 0;
            //int iHeight = 0;
            //int iWidth = 0;
            //int iSelectedIndex = 0;
            //IReadOnlyList<IMediaEncodingProperties> oAvailableResolutions = oCamera.VideoDeviceController.GetAvailableMediaStreamProperties(MediaStreamType.Photo);

            //// if no settings available, bail
            //if (oAvailableResolutions.Count < 1) return;

            //// list the different format settings
            //for (int i = 0; i < oAvailableResolutions.Count; i++)
            //{
            //    VideoEncodingProperties oProperties = (VideoEncodingProperties)oAvailableResolutions[i];
            //    if (oProperties.Width * oProperties.Height > iMaxResolution)
            //    {
            //        iHeight = (int)oProperties.Height;
            //        iWidth = (int)oProperties.Width;
            //        iMaxResolution = (int)oProperties.Width;
            //        iSelectedIndex = i;
            //    }
            //}

            //// set resolution
            //await oCamera.VideoDeviceController.SetMediaStreamPropertiesAsync(MediaStreamType.VideoPreview, oAvailableResolutions[iSelectedIndex]);

            // begin video preview
            oMediaCapture.Source = oCamera;
            await oMediaCapture.Source.StartPreviewAsync();

            //DrawingCanvas.RenderTransform = oMediaCapture.RenderTransform;
            //DrawingCanvas.RenderTransformOrigin = oMediaCapture.RenderTransformOrigin;

            Result oQR = null;
            m_bTakePictures = true;

            Box oReferenceBox = e.Parameter as Box;
            bool bSearchMode = oReferenceBox != null;

            while (m_bTakePictures && (oQR == null || bSearchMode))
            {
                InMemoryRandomAccessStream oPhotoStream = new InMemoryRandomAccessStream();
                await oMediaCapture.Source.CapturePhotoToStreamAsync(ImageEncodingProperties.CreateJpeg(),
                                                                        oPhotoStream);

                WriteableBitmap oBitmap = new WriteableBitmap(1, 1);
                oPhotoStream.Seek(0);
                oBitmap.SetSource(oPhotoStream);
                oBitmap = new WriteableBitmap(oBitmap.PixelWidth, oBitmap.PixelHeight);
                oPhotoStream.Seek(0);
                oBitmap.SetSource(oPhotoStream);

                BarcodeReader oReader = new BarcodeReader();
                oReader.Options.TryHarder = true;
                oReader.AutoRotate = true;

                oQR = oReader.Decode(oBitmap);

                if (bSearchMode && (oQR != null))
                {
                    bSearchMode = oQR.ToString().GetHashCode().ToString() != oReferenceBox.QRCode.QRCode;
                    m_bBoxMatchingMode = true;
                }
                else
                {
                    m_bBoxMatchingMode = false;
                }
            }

            if (oQR != null)
            {
                // show message
                Message = String.Empty;
                QRCodeImage.Visibility = Visibility.Visible;
                //Message = string.Format("Found QR code!", oQR.ToString());

                // highlight qr area
                //RectangleGeometry oGeometry = new RectangleGeometry();
                //List<ResultPoint> oPoints = new List<ResultPoint>(oQR.ResultPoints);

                //// generate polygon
                //Polygon oSegment = new Polygon();
                //oSegment.Stroke = new SolidColorBrush(Color.FromArgb(125, 255, 0, 0));
                //oSegment.Fill = new SolidColorBrush(Color.FromArgb(125, 255, 0, 0));
                //PointCollection oNewPoints = new PointCollection();
                //foreach (ResultPoint oPoint in oPoints)
                //{
                //    oNewPoints.Add(DrawingCanvas.RenderTransform.TransformPoint(new Point(oPoint.X, oPoint.Y)));
                //}
                //oSegment.Points = oNewPoints;

                //// add polygon to canvas
                //DrawingCanvas.Children.Add(oSegment);

                // find box
                await MoveList.CurrentMove.FindBox(new QRCodeWrapper(oQR.ToString()));

                // start transition timer
                DispatcherTimer oTransitionTimer = new DispatcherTimer();
                oTransitionTimer.Interval = new TimeSpan(0, 0, TRANSITION_TIMER_INTERVAL);
                oTransitionTimer.Tick += oTransitionTimer_Tick;
                oTransitionTimer.Start();
            }
        }

        protected override async void OnNavigatedFrom(NavigationEventArgs e)
        {
            await oMediaCapture.Source.StopPreviewAsync();
            m_bTakePictures = false;
            base.OnNavigatedFrom(e);
        }
        #endregion

        #region Event Handlers

        private void CameraButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        void oTransitionTimer_Tick(object sender, object e)
        {
            if (!m_bBoxMatchingMode)
            {
                Message = WaitingForQR;
                DispatcherTimer oTransitionTimer = (DispatcherTimer)sender;
                oTransitionTimer.Stop();
                QRCodeImage.Visibility = Visibility.Collapsed;
                this.Frame.Navigate(typeof(PhotoGallery), null);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MoveListPage));
        }

        #endregion

        #region Properties

        private string Message
        {
            get
            {
                return m_oTextMessage.Text;
            }
            set
            {
                m_oTextMessage.Text = value;
            }
        }

        private DeviceInformation Camera
        {
            get;
            set;
        }

        #endregion

        #region Constants

        private const int TRANSITION_TIMER_INTERVAL = 2;
        private const string WaitingForQR = "Waiting for QR Code...";
        private const string QRCodeText = "Snap a Box QR Code!";
        private const string GalleryText = "Snap a pic for the Gallery!";

        #endregion
    }
}
