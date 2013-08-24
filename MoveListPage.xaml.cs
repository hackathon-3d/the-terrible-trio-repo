using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
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
    public sealed partial class MoveListPage : Page
    {
        public MoveListPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await MoveList.LoadFolders();
            m_oMoveList.ItemsSource = MoveList.MoveListCollection;
        }
      
        private void AddMoveButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MoveName));
        }

        private async void DeleteMoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (m_oMoveList.Items.Count > 0 && m_oMoveList.SelectedIndex != -1)
            {
                MessageDialog oDialog = new MessageDialog("Are you sure you want to delete this move?");
                oDialog.Commands.Add(new UICommand("Yes", YesDeleteCommandInvokedHandler));
                oDialog.Commands.Add(new UICommand("No"));
                await oDialog.ShowAsync();
            }
        }

        private async void YesDeleteCommandInvokedHandler(IUICommand command)
        {
            if (m_oMoveList.Items.Count > 0 && m_oMoveList.SelectedIndex != -1)
            {
                await MoveList.DeleteCurrentMove();
                m_oMoveList.ItemsSource = null;
                m_oMoveList.ItemsSource = MoveList.MoveListCollection;
                m_oMoveList.SelectedItem = null;
            }
        }

        private void m_oMoveList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MoveList.CurrentMove = m_oMoveList.SelectedItem as Move;
        }

        private void ScanButton_Click(object sender, RoutedEventArgs e)
        {
            if (MoveList.CurrentMove != null)
            {
                Frame.Navigate(typeof(QRCameraPage));
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (MoveList.CurrentMove != null)
            {
                Frame.Navigate(typeof(PhotoGalleryMode3));
            }
        }
    }
}
