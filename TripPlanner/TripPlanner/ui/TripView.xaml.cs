using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using TripPlanner.Model;
using TripPlanner.ViewModel;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TripPlanner.ui
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TripView : UserControl
    {

        private TripViewModel _trip;

        public TripViewModel Trip
        {
            get { return _trip; }
            set { _trip = value;
                DataContext = value;
            }
        }

        public TripView()
        {
            this.InitializeComponent();
        }

        public TripView(TripViewModel trip) : this()
        {
            Trip = trip;
        }

        //protected override void OnNavigatedTo(NavigationEventArgs e)
        //{
        //    base.OnNavigatedTo(e);
        //    Trip = e.Parameter as Trip;
        //}

        private void OnEdit(object sender, RoutedEventArgs e)
        {
            Trip.IsEditable = !Trip.IsEditable;
        }

        private async void ItemUpdated(bool isEditing, Item item)
        {
            // Update if not editing
            if (!isEditing)
            {
                await Backend.Local.SaveItem(item, Trip.Trip);
            }
        }

        private async void OnItemToggle(object sender, RoutedEventArgs e)
        {
            Item checkedItem = ((FrameworkElement) sender).DataContext as Item;
            CheckBox usedCheckBox = (CheckBox) e.OriginalSource;

            if (checkedItem?.IsChecked != null && usedCheckBox.IsChecked.HasValue && checkedItem.IsChecked.Value != usedCheckBox.IsChecked.Value)
            {
                checkedItem.IsChecked = !checkedItem.IsChecked.Value;
                usedCheckBox.IsChecked = checkedItem.IsChecked.Value;
                await Backend.Local.UpdateItemChecked(checkedItem);
            }
        }
    }
}
