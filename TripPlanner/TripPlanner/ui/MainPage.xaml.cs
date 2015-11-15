using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TripPlanner.Model;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Input;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using TripPlanner.ui;
using TripPlanner.ViewModel;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TripPlanner
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        public MainPage()
        {
            this.InitializeComponent();
            CurrentTripList = new TripList();
            DataContext = CurrentTripList;
        }

        public TripList CurrentTripList { get; set; }

        public void OpenDetails(TripViewModel trip)
        {
            TripContent.Content = new TripView(trip);
        }

        public void OpenEditor(Trip trip)
        {
            PivotItem item = new PivotItem
            {
                Content = new EditTrip(trip)
            };
        }

        public void OpenMainPage()
        {
            CurrentTripList = new TripList();
            DataContext = CurrentTripList;
        }

        private void TripList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TripViewModel trip = e.AddedItems.First() as TripViewModel;

            if (trip != null)
            {
                OpenDetails(trip);
            }
        }
    }
}
