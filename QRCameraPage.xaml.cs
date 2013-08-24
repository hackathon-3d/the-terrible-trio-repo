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

            
            Message = string.Format("Found QR code {0}", oQR.ToString());
        }
        #endregion

        protected async void DisplayMessageBox(string sMessage, string sTitle)
        {
            var oMessageDialog = new Windows.UI.Popups.MessageDialog(sMessage, sTitle);
            oMessageDialog.DefaultCommandIndex = 1;
            await oMessageDialog.ShowAsync();
        }

        #region Properties
        public string Message
        {
            get;
            private set;
        }

        private DeviceInformation Camera
        {
            get;
            set;
        }

        #endregion
    }
}
