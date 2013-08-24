using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

using Windows.ApplicationModel.Activation;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

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
            Move.LoadFolders();

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
            oMediaCapture.Source = oCamera;
            await oMediaCapture.Source.StartPreviewAsync();
        }

        #endregion

        #region Event Handlers
        private async void CameraButton_Click(object sender, RoutedEventArgs e)
        {
            Message = "Looking for QR Code";
            Result oQR = null;

            //while (oQR == null)            
            //{
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
            //}

                if (oQR == null)
                {
                    Message = "Could not find QR code";

                }
                else
                {
                    Message = string.Format("Found QR code {0}", oQR.ToString());
                    await Move.FindBox(new QRCodeWrapper(oQR.ToString()));
                    this.Frame.Navigate(typeof(PhotoGallery), null);
                }
        }

        private void GalleryButton_Click(object sender, RoutedEventArgs e)
        {

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

        // Sets the mode
        // Mode 1 = "QRCode" = take a QR code pic, examine it to see if it's valid, and go to the gallery if so (showing pics if they exist)
        // Mode 2 = "Gallery" = you are taking pictures of stuff for the gallery
        public string Mode
        {
            get
            {
                return m_sMode;
            }
            set
            {
                //m_sMode = value;
                //if (m_sMode == "QRCode")
                //{
                //    // Show label that tells user to take a photo of a QR Code
                //    InfoText.Text = QRCodeText;
                //    // Hide button to go to gallery
                //    GalleryButton.Visibility = Visibility.Collapsed;
                //}
                //else if(m_sMode == "Gallery")
                //{
                //    // Show label that tells user to take a photo of stuff for the gallery
                //    InfoText.Text = GalleryText;
                //    // Show button to go to gallery
                //    GalleryButton.Visibility = Visibility.Visible;
                //}
            }
        }
        #endregion

        #region Constants
        private const string QRCodeText = "Snap a Box QR Code!";
        private const string GalleryText = "Snap a pic for the Gallery!";
        private string m_sMode = "";
        #endregion
    }
}
