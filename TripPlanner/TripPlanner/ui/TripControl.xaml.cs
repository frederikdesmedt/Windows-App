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
using TripPlanner.ViewModel;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace TripPlanner.ui
{
    public sealed partial class TripControl : UserControl
    {
        public TripControl()
        {
            this.InitializeComponent();
        }

        public static DependencyProperty TripListDependencyProperty = DependencyProperty.Register("TripList",
            typeof (TripList), typeof (TripControl), null);

        public static DependencyProperty TripProp = DependencyProperty.Register("Trip", typeof(TripViewModel), typeof(TripControl), null);

        public TripList TripList
        {
            get { return GetValue(TripListDependencyProperty) as TripList; }
            set
            {
                SetValue(TripListDependencyProperty, value);
                DataContext = value;
            }
        }

        public TripViewModel Trip
        {
            get { return GetValue(TripProp) as TripViewModel; }
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

        private async void MenuDelete_OnClick(object sender, RoutedEventArgs e)
        {
            if (!TripList[0].Equals(Trip))
            {
                TripList.Remove(Trip);
                await Backend.Azure.RemoveTrip(Trip.Trip);
            }
        }
    }
}
