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
    public sealed partial class PhotoGallery : Page
    {
        #region Data Members
        private Dictionary<WriteableBitmap, StorageFile> m_oImageToFileMapping;
        #endregion

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

        private async void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            if (m_oFlipView.Items.Count <= 0)
            {
                return;
            }

            MessageDialog oDialog = new MessageDialog("Are you sure you want to empty this entire box?");
            oDialog.Commands.Add(new UICommand("Yes", YesClearCommandInvokedHandler));
            oDialog.Commands.Add(new UICommand("No"));
            await oDialog.ShowAsync();
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

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (m_oFlipView.SelectedItem == null)
            {
                return;
            }

            MessageDialog oDialog = new MessageDialog("Are you sure you want to delete this picture?");
            oDialog.Commands.Add(new UICommand("Yes", YesDeleteCommandInvokedHandler));
            oDialog.Commands.Add(new UICommand("No"));
            await oDialog.ShowAsync();
        }

        async void YesDeleteCommandInvokedHandler(IUICommand command)
        {
            if (m_oImageToFileMapping != null)
            {
                WriteableBitmap oPhoto = m_oFlipView.SelectedItem as WriteableBitmap;
                if (oPhoto != null)
                {
                    await m_oImageToFileMapping[oPhoto].DeleteAsync();
                    m_oFlipView.Items.Remove(oPhoto);

                    m_oFlipView.UpdateLayout();

                    // if the mapping is now empty, show the empty box indicator
                    if (m_oImageToFileMapping.Count <= 0)
                    {
                        EmptyBoxIndicator.Visibility = Visibility.Visible;
                    }
                }
            }
        }

        async void YesClearCommandInvokedHandler(IUICommand command)
        {
            if (m_oImageToFileMapping != null)
            {
                foreach (StorageFile oPhoto in m_oImageToFileMapping.Values)
                {
                    await oPhoto.DeleteAsync();
                }
            }

            m_oFlipView.Items.Clear();
            m_oFlipView.UpdateLayout();

            EmptyBoxIndicator.Visibility = Visibility.Visible;
        }

        #endregion

        #region Functions

        private async void LoadPhotos()
        {
            // clear existing photos
            m_oFlipView.Items.Clear();
            m_oImageToFileMapping = new Dictionary<WriteableBitmap, StorageFile>();

            // get box folder
            Box oCurrentBox = MoveList.CurrentMove.CurrentBox;
            if (oCurrentBox != null)
            {
                StorageFolder oBoxFolder =
                    await MoveList.CurrentMove.MoveFolder.GetFolderAsync(MoveList.CurrentMove.CurrentBox.ImageFolder);
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

        #endregion
    }
}