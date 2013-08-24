using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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
            this.Frame.Navigate(typeof(QRCameraPage), null);
        }

        private void CameraButton_Click(object sender, RoutedEventArgs e)
        {
            // Open Window
            this.Frame.Navigate(typeof(PhotoCameraPage), null);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region Properties

        public Box CurrentBox 
        {
            get;
            set;
        }

        #endregion
    }
}