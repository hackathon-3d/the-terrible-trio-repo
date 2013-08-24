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
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
      
        private async void AddMoveButton_Click(object sender, RoutedEventArgs e)
        {
            Popup MoveNameControl = new Popup();
            MoveNameControl.Child = new MoveName();
            MoveNameControl.IsOpen = true;
             
            Move oNewMove = await MoveList.FindMove((MoveNameControl.Child as MoveName).MoveNameString);

            m_oMoveList.Items.Add(oNewMove);
            m_oMoveList.SelectedItem = oNewMove;
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
    }
}
