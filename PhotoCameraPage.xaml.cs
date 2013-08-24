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

            // resolution variables
            int iMaxResolution = 0;
            int iHeight = 0;
            int iWidth = 0;
            int iSelectedIndex = 0;
            IReadOnlyList<IMediaEncodingProperties> oAvailableResolutions = oCamera.VideoDeviceController.GetAvailableMediaStreamProperties(MediaStreamType.Photo);

            // if no settings available, bail
            if (oAvailableResolutions.Count < 1) return;

            // list the different format settings
            for (int i = 0; i < oAvailableResolutions.Count; i++)
            {
                VideoEncodingProperties oProperties = (VideoEncodingProperties)oAvailableResolutions[i];
                if (oProperties.Width * oProperties.Height > iMaxResolution)
                {
                    iHeight = (int)oProperties.Height;
                    iWidth = (int)oProperties.Width;
                    iMaxResolution = (int)oProperties.Width;
                    iSelectedIndex = i;
                }
            }

            // set resolution
            await oCamera.VideoDeviceController.SetMediaStreamPropertiesAsync(MediaStreamType.VideoPreview, oAvailableResolutions[iSelectedIndex]);

            oMediaCapture.Source = oCamera;
            await oMediaCapture.Source.StartPreviewAsync();
        }

        #endregion

        #region Event Handlers

        private async void CameraButton_Click(object sender, RoutedEventArgs e)
        {
            // store photo
            String sFileName = String.Format("{0}.jpg", Guid.NewGuid().ToString());
            StorageFolder oBoxFolder =
                await MoveList.CurrentMove.MoveFolder.GetFolderAsync(MoveList.CurrentMove.CurrentBox.ImageFolder);
            StorageFile oPhotoFile =
                await oBoxFolder.CreateFileAsync(sFileName, CreationCollisionOption.ReplaceExisting);
            await oMediaCapture.Source.CapturePhotoToStorageFileAsync(ImageEncodingProperties.CreateJpeg(),
                                                                      oPhotoFile);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
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
