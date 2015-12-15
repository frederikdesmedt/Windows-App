using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using TripPlanner.Model;
using TripPlanner.ViewModel;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TripPlanner.ui
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RegistrationPage : Page
    {

        public RegistrationPage()
        {
            this.InitializeComponent();
        }

        private void RepeatPassword_OnKeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                Register(sender, e);
            }
        }

        private void Password_OnKeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                RepeatPassword.Focus(FocusState.Programmatic);
            }
        }

        private void Username_OnKeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                Password.Focus(FocusState.Programmatic);
            }
        }

        private async void Register(object sender, RoutedEventArgs e)
        {
            UserViewModel user = new UserViewModel
            {
                Password = Password.Password,
                RepeatPassword = RepeatPassword.Password,
                Username = Username.Text
            };

            Backend.RegistrationResponse response = await Backend.Azure.Register(user);

            if (response.ValidationErrors != null && response.ValidationErrors.Any())
            {
                Error.Text = response.ValidationErrors.First().ErrorMessage;
            }
            else
            {
                if (response.BackendResponse == Backend.BackendResponse.Ok)
                {
                    Error.Text = "";
                    Frame.Navigate(typeof (LoginPage), user.Username);
                } else if (response.BackendResponse == Backend.BackendResponse.BadRequest)
                {
                    Error.Text = "Email is already used, please pick another one or log in";
                }
                else
                {
                    Error.Text = "Looks like something went wrong, try again later";
                }
            }
        }

        private void Login(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof (LoginPage));
        }
    }
}
