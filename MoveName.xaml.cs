using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace VisualMove
{
    public sealed partial class MoveName : Page
    {
        private Regex m_oValidNameRegex = new Regex(@"^[a-zA-Z][a-zA-Z0-9_\s]*$");

        public MoveName()
        {
            this.InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            MoveNameString = m_oMoveName.Text;

            //TODO:  Check for valid names
            if (!m_oValidNameRegex.IsMatch(MoveNameString))
            {
                MessageDialog oDialog =
                    new MessageDialog("Invalid name. Please enter a character starting with a letter, and containing only alphanumeric characters plus spaces.");
                oDialog.Commands.Add(new UICommand("OK"));
                await oDialog.ShowAsync();
            }
            else
            {
                if (MoveList.MoveListCollection.Select(oMove => oMove.Name).Contains(MoveNameString))
                {
                    MessageDialog oDialog =
                    new MessageDialog("This name already exists.  Please choose a unique name.");
                    oDialog.Commands.Add(new UICommand("OK"));
                    await oDialog.ShowAsync();
                }
                else
                {
                    await MoveList.FindMove(MoveNameString);
                    Frame.Navigate(typeof(QRCameraPage));
                }
            }
        }

        public string MoveNameString
        {
            get;
            set;
        }
    }
}
