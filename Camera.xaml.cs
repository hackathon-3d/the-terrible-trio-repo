using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace VisualMove
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CameraPage : Page
    {
        #region Enumerations
        private enum CameraLocation
        {
            Front,
            Back
        }
        #endregion

        #region Constructors
        public CameraPage()
        {
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
            switch(oCameras.Count)
            {
                case 0:
                    throw new Exception("No cameras found");
                case 1:
                    //Only front camera is available
                    Camera = oCameras[(int)CameraLocation.Front];
                    break;
                default:
                    Camera = oCameras[(int)CameraLocation.Back];
                    break;
            }
        }
        #endregion

        #region Event Handlers
        private async void CaptureElement_Loaded(object sender, RoutedEventArgs e)
        {
            CaptureElement oMediaCapture = sender as CaptureElement;
            if (oMediaCapture == null)
            {
                throw new Exception("Event isn't tied to a capture element (and it really should be");
            }

            MediaCaptureInitializationSettings oCameraSettings = new MediaCaptureInitializationSettings();
            oCameraSettings.VideoDeviceId = Camera.Id;

            oMediaCapture.Source = new MediaCapture();
            await oMediaCapture.Source.InitializeAsync(oCameraSettings);
            await oMediaCapture.Source.StartPreviewAsync();
        }

        private void CameraButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GalleryButton_Click(object sender, RoutedEventArgs e)
        {

        }
        #endregion

        #region Properties
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
