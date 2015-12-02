﻿using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.Devices.Geolocation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TripPlanner.ui
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Location : Page
    {
        Geolocator geolocator;
        public Location()
        {

            geolocator = new Geolocator();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            GetCurrentLocation();
            base.OnNavigatedTo(e);
        }

        private async void GetCurrentLocation()
        {
            try
            {
                geolocator.DesiredAccuracyInMeters = 50;
                TimeSpan maxAge = new TimeSpan(0, 0, 1);
                TimeSpan timeout = new TimeSpan(0, 0, 15);

                Geoposition myLocation = await geolocator.GetGeopositionAsync(maxAge, timeout);

                //Geocoordinate geocord = new Geocoordinate(mylocation.coordinate.latitude, mylocation.coordinate.longitude);
                //mapje.center = geocord;
                //mapje.zoomlevel = 12;



            }
            catch (Exception e)
            {
                //exeption
            }
        }

        private void NavigateToPoint()
        {
            // BingMapsDirectionsTask bingMapsDirectionsTask = new BingMapsDirectionsTask();
            //LabeledMapLocation navigation = new LabeledMapLocation("TODO_BIND_LOCATION_NAME", null);
            //bingMapsDirectionsTask.End = navigation;

            //bingMapsDirectionsTask.Show();
        }
    }
}
