using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace VisualMove
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PhotoGalleryMode3 : Page
    {
        #region Data Members
        private Dictionary<WriteableBitmap, StorageFile> m_oImageToFileMapping;
        private Dictionary<WriteableBitmap, Box> m_oImageToBoxMapping;
        #endregion

        #region Constructors

        public PhotoGalleryMode3()
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
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Open Window
            this.Frame.GoBack();
        }

        private void CameraButton_Click(object sender, RoutedEventArgs e)
        {
            // Open Window
            WriteableBitmap oPhoto = m_oFlipView.SelectedItem as WriteableBitmap;
            if(oPhoto != null)
            {            
                this.Frame.Navigate(typeof(QRCameraPage), m_oImageToBoxMapping[oPhoto]);
            }
        }
        #endregion

        #region Functions
        private async void LoadPhotos()
        {
            // clear existing photos
            m_oFlipView.Items.Clear();
            m_oImageToFileMapping = new Dictionary<WriteableBitmap, StorageFile>();
            m_oImageToBoxMapping = new Dictionary<WriteableBitmap, Box>();

            // get box folder
            foreach(Box oCurrentBox in MoveList.CurrentMove.Boxes)
            {
                if (oCurrentBox != null)
                {
                    StorageFolder oBoxFolder =
                        await oCurrentBox.AssociatedMove.MoveFolder.GetFolderAsync(oCurrentBox.ImageFolder);
                    IReadOnlyList<StorageFile> oPhotos = await oBoxFolder.GetFilesAsync();

                    // if there's an empty list of photos, we can stop the process here and show the empty indicator
                    if (oPhotos.Count <= 0)
                    {
                        EmptyBoxIndicator.Visibility = Visibility.Visible;
                    }
                    else
                    {
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
                                m_oFlipView.Items.Add(oBitmap);
                                m_oImageToFileMapping.Add(oBitmap, oPhoto);
                                m_oImageToBoxMapping.Add(oBitmap, oCurrentBox);
                            }
                        }

                        // hide the empty box indicator
                        EmptyBoxIndicator.Visibility = Visibility.Collapsed;
                    }
                }
                else
                {
                    EmptyBoxIndicator.Visibility = Visibility.Visible;
                }
            }
        }
        #endregion
    }
}