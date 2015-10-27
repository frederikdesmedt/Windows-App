using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using TripPlanner.Model;
using System.Diagnostics;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace TripPlanner.ui
{
    public sealed partial class TripControl : UserControl
    {
        public TripControl()
        {
            this.InitializeComponent();
            
        }

        public static DependencyProperty TripProp = DependencyProperty.Register("Trip", typeof(Trip), typeof(TripControl), null);

        public Trip Trip
        {
            get { return GetValue(TripProp) as Trip; }
            set
            {
                SetValue(TripProp, value);
                DataContext = value;
                Bindings.Update(); //can't use metadata in dependency property because the callback would be a static method
            }
        }

        public void OnTripHolding(object sender, RightTappedRoutedEventArgs args)
        {
            FrameworkElement senderElement = sender as FrameworkElement;
            FlyoutBase flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);
            flyoutBase.ShowAt(senderElement);
        }

        private void MenuEdit_OnClick(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Editing trip " + Trip.Name);
            //Trip_OnTapped(sender, null);
        }

        private void MenuDelete_OnClick(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Deleting trip " + Trip.Name);
        }

        //private void Trip_OnTapped(object sender, TappedRoutedEventArgs e)
        //{
        //    Debug.WriteLine("Default action for trip in listview");
        //    Frame f = Window.Current.Content as Frame;
        //    f.Navigate(typeof (TripEditor), Trip);
        //}
    }
}
