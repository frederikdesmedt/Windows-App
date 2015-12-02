using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Services.Maps;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace TripPlanner.ui
{
    public sealed partial class Navigate : UserControl
    {
        
       
        public Navigate()
        {
            this.InitializeComponent();

           
        }

        public async void SetDestination(string des)
        {
            try
            {

                MapLocation x = await GetPositionFromAddressAsync(des);
                destinationMap.Center = x.Point;
            }
            catch (Exception e)
            {
                //exeption
            }

        }


      public static async Task<MapLocation> GetPositionFromAddressAsync(string address)
        {
            // get current location to use as a search start point
            var locator = new Geolocator();
            var position = await locator.GetGeopositionAsync();

            // convert current location to a GeoPoint
            var basicGeoposition = new BasicGeoposition();
            basicGeoposition.Latitude = position.Coordinate.Latitude;
            basicGeoposition.Longitude = position.Coordinate.Longitude;
            var point = new Geopoint(basicGeoposition);

            // using the address passed in as a parameter, search for MapLocations that match it
            var mapLocationFinderResult = await MapLocationFinder.FindLocationsAsync(address, point, 2);

            if (mapLocationFinderResult.Status == MapLocationFinderStatus.Success)
            {
                return mapLocationFinderResult.Locations[0];
            }

            return default(MapLocation);
        


        }
    }
}
