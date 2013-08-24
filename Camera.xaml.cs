using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class Camera : Page
    {
        public Camera()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
        
        private void CameraButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GalleryButton_Click(object sender, RoutedEventArgs e)
        {

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
                m_sMode = value;
                if (m_sMode == "QRCode")
                {
                    // Show label that tells user to take a photo of a QR Code
                    InfoText.Text = QRCodeText;
                    // Hide button to go to gallery
                    GalleryButton.Visibility = Visibility.Collapsed;
                }
                else if(m_sMode == "Gallery")
                {
                    // Show label that tells user to take a photo of stuff for the gallery
                    InfoText.Text = GalleryText;
                    // Show button to go to gallery
                    GalleryButton.Visibility = Visibility.Visible;
                }
            }
        }

        private const string QRCodeText = "Snap a Box QR Code!";
        private const string GalleryText = "Snap a pic for the Gallery!";
        private string m_sMode = "";
    }
}
