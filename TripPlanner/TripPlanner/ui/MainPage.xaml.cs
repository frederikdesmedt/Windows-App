using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using TripPlanner.Model;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Services.Maps;
using Windows.UI;
using Windows.UI.Input;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
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
            CurrentTripList.Add(new TripViewModel(new Trip(-1)
            {
                Name = "New trip",
                Icon = "\uE109"
            }));

            DataContext = CurrentTripList;
        }

        private async void LoadData()
        {
            await CurrentTripList.Load();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            LoadData();
        }

        public UserControl MainContent
        {
            set { TripContent.Content = value; }
        }

        public TripList CurrentTripList { get; set; }

        public void OpenDetails(TripViewModel trip)
        {
            TripContent.Content = new TripView(this, trip);
            SetBackgroundToLocation(trip.Trip);
        }

        private async void SetBackgroundToLocation(Trip trip)
        {
            
            Background = new SolidColorBrush(Color.FromArgb(255, 65, 131, 215));
            MainSplit.PaneBackground = Background;
            bool shouldSave = false;
            if (trip.Location == null)
            {
                trip.Location = await Backend.MapService.RetrieveTrip(trip);
                shouldSave = true;
            }
            
            MapLocation location = trip.Location;

            if (location != null)
            {
                if (trip.PopularImage == null)
                {
                    var result = await Backend.MapService.GetStreetviewImage(location);
                    trip.PopularImage = result;
                }

                if (trip.PopularImage == null)
                {
                    Background = new SolidColorBrush(Color.FromArgb(255, 65, 131, 215));
                    var tripView = TripContent.Content as TripView;
                    if (tripView != null)
                    {
                        tripView.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                    }
                }
                else
                {
                    Background = new ImageBrush() { ImageSource = trip.PopularImage, Stretch = Stretch.UniformToFill };
                    var tripView = TripContent.Content as TripView;
                    if (tripView != null)
                    {
                        tripView.Background = new SolidColorBrush(Color.FromArgb(130, 50, 50, 50));
                    }
                }

                if (shouldSave)
                {
                    await Backend.Azure.SaveTrip(trip);
                }
            }
            
        }

        public void OpenMainPage()
        {
            CurrentTripList = new TripList();
            DataContext = CurrentTripList;
        }

        private void TripList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
            {
                TripContent.Content = null;
                return;
            }

            TripViewModel trip = e.AddedItems.First() as TripViewModel;

            if (trip?.Trip.Id == -1)
            {
                AddTripView addTrip = new AddTripView();
                addTrip.Background = new SolidColorBrush(Color.FromArgb(255, 65, 131, 215));
                TripContent.Content = addTrip;
                addTrip.TripAdded += OnTripAdded;
                MainSplit.IsPaneOpen = false;
            }
            else if (trip != null)
            {
                OpenDetails(trip);
                MainSplit.IsPaneOpen = false;
            }
        }

        private async void OnTripAdded(string name, DateTime date, string glyph)
        {
            TripViewModel trip = new TripViewModel(new Trip
            {
                Name = name,
                Date = date,
                Icon = glyph
            });
            
            CurrentTripList.Add(trip);
            OpenDetails(trip);
            await Backend.Azure.SaveTrip(trip);
            SetBackgroundToLocation(trip.Trip);
        }

        private void OnToggleMenu(object sender, RoutedEventArgs e)
        {
            MainSplit.IsPaneOpen = true;
        }

        private void Home_OnClick(object sender, RoutedEventArgs e)
        {
            var trip = TripList.SelectedItem as TripViewModel;
            if(trip != null)
            OpenDetails(trip);
        }

        private void Logout_OnClick(object sender, RoutedEventArgs e)
        {
            Backend.Azure.Logout();
            Frame.Navigate(typeof (LoginPage));
        }
    }
}
