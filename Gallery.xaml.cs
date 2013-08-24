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

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            int index = CurrentBox.Photos.IndexOf(CurrentBox.CurrentPhoto);
            CurrentBox.Photos.Remove(CurrentBox.CurrentPhoto);
            if (index - 1 < 0)
            {
                //if(
                // CurrentBox.CurrentPhoto = CurrentBox.Photos[index + 1];
                // else
                {
                    // We are empty, no photos left
                    CurrentBox.CurrentPhoto = null;
                    DeleteButton.IsEnabled = false;
                    ClearButton.IsEnabled = false;
                }
            }
            else
            {
                CurrentBox.CurrentPhoto = CurrentBox.Photos[index];
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentBox.Photos.Clear();
            DeleteButton.IsEnabled = false;
            ClearButton.IsEnabled = false;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Camera.Mode = "QRCode";
            // Open Window
        }

        private void CameraButton_Click(object sender, RoutedEventArgs e)
        {
            Camera.Mode = "Gallery";
            // Open Window
        }

        public Camera Camera
        {
            get;
            set;
        }

        public Box CurrentBox 
        {
            get;
            set;
        }
    }
}
