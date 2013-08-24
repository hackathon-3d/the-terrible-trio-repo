using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public sealed partial class Gallery : Page
    {
        public Gallery()
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

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            // May want a messagebox here
            DisplayMessageBox("Clear all photos for this box?", "Confirm Clear");
            Move.CurrentBox.Photos.Clear();
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

        protected async void DisplayMessageBox(string sMessage, string sTitle)
        {
            var oMessageDialog = new Windows.UI.Popups.MessageDialog(sMessage, sTitle);
            oMessageDialog.DefaultCommandIndex = 1;
            await oMessageDialog.ShowAsync();
        }
    }
}
