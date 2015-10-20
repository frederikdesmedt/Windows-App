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
            }
        }

        public void OnTripHolding(object sender, RightTappedRoutedEventArgs args)
        {
            Trip trip = Trip;
        }
    }
}
