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
using Windows.UI.Popups;
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
            CurrentTripList.Add(new TripViewModel(new Trip(-1)
            {
                Name = "New trip", Icon = "\uE109"
            }));
            DataContext = CurrentTripList;
            LoginAndLoadData();
        }

        private async void LoginAndLoadData()
        {
            Backend.BackendResponse re = await Backend.Azure.Login("test@outlook.com", "Bl@123");
            if (re == Backend.BackendResponse.Ok)
            {
                var dialog = new MessageDialog($"Logged in as {Backend.Azure.Username}").ShowAsync();
                await CurrentTripList.Load();
                await dialog;
            }
            else
            {
                await new MessageDialog("Couldn't log in").ShowAsync();
            }
        }

        public UserControl MainContent
        {
            set { TripContent.Content = value; }
        }

        public TripList CurrentTripList { get; set; }

        public void OpenDetails(TripViewModel trip)
        {
            TripContent.Content = new TripView(this, trip);
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
                TripContent.Content = addTrip;
                addTrip.TripAdded += OnTripAdded;
                MainSplit.IsPaneOpen = false;
            } else if (trip != null)
            {
                OpenDetails(trip);
                MainSplit.IsPaneOpen = false;
            }
        }

        private async void OnTripAdded(string name, DateTime date)
        {
            TripViewModel trip = new TripViewModel(new Trip
            {
                Name = name,
                Date = date
            });

            CurrentTripList.Add(trip);
            OpenDetails(trip);
            await Backend.Azure.SaveTrip(trip);
        }

        private void OnToggleMenu(object sender, RoutedEventArgs e)
        {
            MainSplit.IsPaneOpen = true;
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
