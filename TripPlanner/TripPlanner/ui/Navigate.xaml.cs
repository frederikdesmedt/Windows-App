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
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace TripPlanner.ui
{
    public sealed partial class Navigate : UserControl
    {
        
       
        public Navigate()
        {
            this.InitializeComponent();
        }

        public void SetDestination(MapLocation loc)
        {
            try
            {
                destinationMap.Center = loc.Point;
                destinationMap.ZoomLevel = 12;
            }
            catch (Exception e)
            {
                //exeption
            }

        }


      

        //private void AddPoint(Map controlMap, Geocoordinate geo)
        //{
        //    // With the new Map control:
        //    //  Map -> MapLayer -> MapOverlay -> UIElements
        //    //  - Add a MapLayer to the Map
        //    //  - Add an MapOverlay to that layer
        //    //  - We can add a single UIElement to that MapOverlay.Content
        //    MapLayer ml = new MapLayer();
        //    MapOverlay mo = new MapOverlay();

        //    // Add an Ellipse UI
        //    Ellipse r = new Ellipse();
        //    r.Fill = new SolidColorBrush(Color.FromArgb(255, 240, 5, 5));
        //    // the item is placed on the map at the top left corner so
        //    // in order to center it, we change the margin to a negative
        //    // margin equal to half the width and height
        //    r.Width = r.Height = 12;
        //    r.Margin = new Thickness(-6, -6, 0, 0);

        //    // Add the Ellipse to the Content 
        //    mo.Content = r;
        //    // Set the GeoCoordinate of that content
        //    mo.GeoCoordinate = geo;
        //    // Add the MapOverlay to the MapLayer
        //    ml.Add(mo);
        //    // Add the MapLayer to the Map
        //    controlMap.Layers.Add(ml);
        //}
        private void DestinationMap_OnLoaded(object sender, RoutedEventArgs e)
        {
            destinationMap.MapServiceToken = "AvC9TPmIp6G7RwtF0EDPAUcUTHz7qGrUMLAo_yDYmFnEdZ8xXt-Yn0Xk-9Q7KK-A";
        }
    }
}
