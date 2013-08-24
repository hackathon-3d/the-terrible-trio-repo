using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace VisualMove
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PhotoCameraPage : Page
    {
        #region Enumerations

        private enum CameraLocation
        {
            Front,
            Back
        }

        #endregion

        #region Constructors

        public PhotoCameraPage()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Page

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
            // store photo
            PhotoWrapper oPhoto = new PhotoWrapper();
            StorageFile oFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(oPhoto.FileName, CreationCollisionOption.ReplaceExisting);
            await oMediaCapture.Source.CapturePhotoToStorageFileAsync(ImageEncodingProperties.CreateJpeg(), oFile);

            // add photo to box
            Move.CurrentBox.Photos.Add(oPhoto);
        }

        private void GalleryButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        #endregion

        #region Properties

        private DeviceInformation Camera
        {
            get;
            set;
        }

        #endregion

        #region Constants

        private const string QRCodeText = "Snap a Box QR Code!";
        private const string GalleryText = "Snap a pic for the Gallery!";
        private string m_sMode = "";

        #endregion
    }
}
