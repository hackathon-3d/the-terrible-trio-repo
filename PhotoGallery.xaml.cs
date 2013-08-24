using System;
using System.Collections.Generic;
using System.IO;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace VisualMove
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PhotoGallery : Page
    {
        #region Constructors

        public PhotoGallery()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Page

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // load photos
            LoadPhotos();
        }

        #endregion

        #region Event Handlers

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            // May want a messagebox here
            CurrentBox.Photos.Clear();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Open Window
            if (CurrentBox.Photos.Count == 0)
            {
                Move.Boxes.Remove(CurrentBox);
                Move.CurrentBox = null;
            }
            this.Frame.GoBack();
        }

        private void CameraButton_Click(object sender, RoutedEventArgs e)
        {
            // Open Window
            this.Frame.Navigate(typeof(PhotoCameraPage), null);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Do something here eventually
        }

        #endregion

        private async void LoadPhotos()
        {
            // get photos from current box
            List<PhotoWrapper> oPhotos = new List<PhotoWrapper>(Move.CurrentBox.Photos);
            foreach (PhotoWrapper oPhoto in oPhotos)
            {
                StorageFile oPhotoFile = await ApplicationData.Current.LocalFolder.GetFileAsync(oPhoto.FileName);
                using (IRandomAccessStream oPhotoStream = await oPhotoFile.OpenReadAsync())
                {
                    WriteableBitmap oBitmap = new WriteableBitmap(1, 1);
                    oPhotoStream.Seek(0);
                    oBitmap.SetSource(oPhotoStream);
                    oBitmap = new WriteableBitmap(oBitmap.PixelWidth, oBitmap.PixelHeight);
                    oPhotoStream.Seek(0);
                    oBitmap.SetSource(oPhotoStream);

                    ImageDisplay.Source = oBitmap;
                }
            }
        }

        #region Properties

        public Box CurrentBox
        {
            get;
            set;
        }

        #endregion
    }
}
