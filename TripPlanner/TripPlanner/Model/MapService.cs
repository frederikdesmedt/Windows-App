using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Services.Maps;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Newtonsoft.Json;

namespace TripPlanner.Model
{
    public class MapService
    {

        public Dictionary<Trip, MapLocation> LocationMapping = new Dictionary<Trip, MapLocation>();
        public Dictionary<MapLocation, BitmapImage> StreetviewMapping = new Dictionary<MapLocation, BitmapImage>(); 

        public MapService() { }

        public async Task<MapLocation> RegisterTrip(Trip t)
        {
            //if (LocationMapping.ContainsKey(t))
            //{
            //    return LocationMapping[t];
            //}
            MapLocation ml = await GetPositionFromAddressAsync(t.Name);
            LocationMapping.Add(t, ml);
            return ml;
        }

        public async Task<BitmapImage> GetStreetviewImage(MapLocation location)
        {
            if (StreetviewMapping.ContainsKey(location))
            {
                return StreetviewMapping[location];
            }
            // string url = $"https://maps.googleapis.com/maps/api/streetview?size=800x600&&fov=90&heading=200&pitch=0";

            //string url = $"https://maps.googleapis.com/maps/api/place/nearbysearch/json?location={location.Point.Position.Latitude},{location.Point.Position.Longitude}&radius=500&keyword=monument";
            
            string placesUrl = $"https://maps.googleapis.com/maps/api/place/nearbysearch/json?location={location.Point.Position.Latitude},{location.Point.Position.Longitude}&radius=500&type=park&key=AIzaSyBHZBLrtywOoyYjlYn2FSi_Ueia76VwL5U";

            HttpWebRequest request = WebRequest.CreateHttp(new Uri(placesUrl, UriKind.Absolute));
            request.ContentType = "application/json";
            request.Accept = "application/json";
            request.Method = "GET";
            var response = await request.GetResponseAsync();
            var reader = new StreamReader(response.GetResponseStream());
            Debug.WriteLine(placesUrl);
            var deserializedObject = JsonConvert.DeserializeObject<dynamic>(reader.ReadToEnd());


            foreach (var v in deserializedObject.results)
            {
                if (v.photos != null)
                {
                    foreach (var photo in v.photos)
                    {
                        var refer = photo.photo_reference;
                        string imageUrl = $"https://maps.googleapis.com/maps/api/place/photo?maxwidth=1200&photoreference={refer}&key=AIzaSyBHZBLrtywOoyYjlYn2FSi_Ueia76VwL5U";
                        Debug.WriteLine("Image URL: " + imageUrl);
                        BitmapImage bi = new BitmapImage(new Uri(imageUrl, UriKind.Absolute));
                        StreetviewMapping.Add(location, bi);
                        return bi;
                    }
                }
            }






            return null;
        }

        public async Task<MapLocation> GetPositionFromAddressAsync(string address)
        {
            var locator = new Geolocator();
            locator.DesiredAccuracyInMeters = 50;
            TimeSpan maxAge = new TimeSpan(0, 0, 1);
            TimeSpan timeout = new TimeSpan(0, 0, 15);
            var position = await locator.GetGeopositionAsync();

            var basicGeoposition = new BasicGeoposition();
            var point = new Geopoint(basicGeoposition);

            var mapLocationFinderResult = await MapLocationFinder.FindLocationsAsync(address, point, 2);

            if (mapLocationFinderResult.Status == MapLocationFinderStatus.Success)
            {
                try
                {
                    return mapLocationFinderResult.Locations[0];
                }
                catch (Exception e)
                {
                    return null;
                }
                
                
            }

            return default(MapLocation);
        }
    }
}
