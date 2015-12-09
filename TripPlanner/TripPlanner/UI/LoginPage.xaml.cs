using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using TripPlanner.Model;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TripPlanner.ui
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            
        }

        private async void Login(object sender, RoutedEventArgs e)
        {
            if (await Backend.Azure.Login(Username.Text, Password.Password) == Backend.BackendResponse.Unauthorized)
            {
                Password.Focus(FocusState.Programmatic);
                var dialog = new MessageDialog("Couldn't log in").ShowAsync();
            }
            else
            {
                var dialog = new MessageDialog($"Welcome {Backend.Azure.Username}!").ShowAsync();
                Frame.Navigate(typeof(MainPage));
            }
        }

        private void Username_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
                Password.Focus(FocusState.Programmatic);
        }

        private void Password_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
                Login(null, null);
        }
    }
}
