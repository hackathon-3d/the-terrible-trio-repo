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
            m_oMoveList.ItemsSource = MoveList.MoveListCollection.Select(oMove => oMove.Name);
        }
      
        private void AddMoveButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MoveName));
        }

        private void DeleteMoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (m_oMoveList.Items.Count > 0 && m_oMoveList.SelectedIndex != -1)
            {
                //int iIndex = m_oMoveList.SelectedIndex;
                //m_oMoveList.Items.RemoveAt(iIndex);
                //m_oRegressionProfile.Configs.RemoveAt(iIndex);
                //if (iIndex > 0)
                //{
                //    m_oRegressionListView.SelectedItem = m_oRegressionListView.Items[iIndex - 1];
                //}
                //else if (iIndex == 0 && m_oRegressionListView.Items.Count > 0)
                //{
                //    m_oRegressionListView.SelectedItem = m_oRegressionListView.Items[0];
                //}
            }
        }

        private void m_oMoveList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MoveList.CurrentMove = m_oMoveList.SelectedItem as Move;
            Frame.Navigate(typeof(QRCameraPage));
        }
    }
}
