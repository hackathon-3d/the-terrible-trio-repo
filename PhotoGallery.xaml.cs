using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Open Window
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
            // clear existing photos
            m_oFlipView.Items.Clear();
            //Photos = new ObservableCollection<WriteableBitmap>();

            // get box folder
            Box oCurrentBox = Move.CurrentBox;
            if (oCurrentBox != null)
            {
                StorageFolder oBoxFolder = await ApplicationData.Current.LocalFolder.GetFolderAsync(Move.CurrentBox.ImageFolder);
                IReadOnlyList<StorageFile> oPhotos = await oBoxFolder.GetFilesAsync();

                // iterate through folder and load each photo
                foreach (StorageFile oPhoto in oPhotos)
                {
                    using (IRandomAccessStream oPhotoStream = await oPhoto.OpenReadAsync())
                    {
                        WriteableBitmap oBitmap = new WriteableBitmap(1, 1);
                        oPhotoStream.Seek(0);
                        oBitmap.SetSource(oPhotoStream);
                        oBitmap = new WriteableBitmap(oBitmap.PixelWidth, oBitmap.PixelHeight);
                        oPhotoStream.Seek(0);
                        oBitmap.SetSource(oPhotoStream);

                        // add photo
                        //Photos.Add(oBitmap);
                        m_oFlipView.Items.Add(oBitmap);
                    }
                }
            }

        }
    }
}
