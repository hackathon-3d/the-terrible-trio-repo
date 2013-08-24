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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace VisualMove
{
    public sealed partial class MoveName : Page
    {
        public MoveName()
        {
            this.InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            MoveNameString = m_oMoveName.Text;
            this.Visibility = Visibility.Collapsed;

            //TODO:  Check for valid names

            if (MoveList.MoveListCollection.Select(oMove => oMove.Name).Contains(MoveNameString))
            {
                //TODO:  Tell user that name already exists
            }
            else
            {
                await MoveList.FindMove(MoveNameString);
                Frame.Navigate(typeof(QRCameraPage));
            }
        }

        public string MoveNameString
        {
            get;
            set;
        }
    }
}
