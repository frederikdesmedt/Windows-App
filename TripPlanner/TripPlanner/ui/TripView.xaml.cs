using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Services.Maps;
using Windows.System;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
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
        private MainPage mainPage;

        private TripViewModel _trip;

        public TripViewModel Trip
        {
            get { return _trip; }
            set
            {
                _trip = value;
                DataContext = value;
            }
        }

        private Brush _background;

        public new Brush Background
        {
            get { return _background; }
            set
            {
                _background = value;
                panel.Background = _background;
            }
        }

        public TripView()
        {
            this.InitializeComponent();
        }

        public TripView(MainPage mp, TripViewModel trip) : this()
        {
            Trip = trip;
            mainPage = mp;
        }

        //protected override void OnNavigatedTo(NavigationEventArgs e)
        //{
        //    base.OnNavigatedTo(e);
        //    Trip = e.Parameter as Trip;
        //}

        private void OnEdit(object sender, RoutedEventArgs e)
        {
            if (Trip.IsRemoving)
            {
                Trip.IsRemoving = false;
            }

            Trip.IsEditable = !Trip.IsEditable;
        }

        private async void ItemUpdated(bool isEditing, Item item)
        {
            // Update if not editing
            if (!isEditing)
            {
                await Backend.Azure.SaveItem(item, Trip.Trip);
            }
        }

        private async void OnItemToggle(object sender, RoutedEventArgs e)
        {
            if (usingSpace)
            {
                usingSpace = false;
                var senderBox = (sender as CheckBox);
                senderBox.IsChecked = !senderBox.IsChecked;

                return;
            }

            Item checkedItem = ((FrameworkElement)sender).DataContext as Item;
            CheckBox usedCheckBox = (CheckBox)e.OriginalSource;

            if (checkedItem?.IsChecked != null && usedCheckBox.IsChecked.HasValue && checkedItem.IsChecked.Value != usedCheckBox.IsChecked.Value)
            {
                checkedItem.IsChecked = !checkedItem.IsChecked.Value;
                usedCheckBox.IsChecked = checkedItem.IsChecked.Value;
                await Backend.Azure.UpdateItemChecked(checkedItem);
            }
        }

        private void OnDelete(object sender, RoutedEventArgs e)
        {
            if (Trip.IsEditable)
            {
                Trip.IsEditable = false;
            }

            Trip.IsRemoving = !Trip.IsRemoving;
        }

        private async void OnRemoved(Item item)
        {
            Trip.Trip.ItemList.Remove(item);
            await Backend.Azure.RemoveItem(item);
        }

        private async void OnAdd(object sender, RoutedEventArgs e)
        {
            Item item = new Item();
            Trip.Trip.ItemList.Add(item);
            ItemList.SelectedItem = item;
            await Task.Delay(500);
            var listViewItem = ItemList.ContainerFromItem(item);
            var customControl = FindChildControl<CheckBox>(listViewItem);
            EditableTextBlock textBlock = customControl.Content as EditableTextBlock;
            if (textBlock != null)
            {
                textBlock.IsEditable = true;
                textBlock.IsEditing = true;
                textBlock.Focus();
                EditableTextBlock.IsEditingChangedDelegate del = null;
                del = (editing, item1) =>
                {
                    textBlock.OnIsEditingChanged -= del;
                    Trip.PropertyChanged += (o, args) =>
                    {
                        if (args.PropertyName == "IsEditable")
                        {
                            textBlock.IsEditable = Trip.IsEditable;
                        }
                        else if (args.PropertyName == "IsRemoving")
                        {
                            textBlock.IsRemoving = Trip.IsRemoving;
                        }
                    };

                    textBlock.IsEditable = Trip.IsEditable;
                    textBlock.IsRemoving = Trip.IsRemoving;
                };

                textBlock.OnIsEditingChanged += del;
                
            }
        }

        private T FindChildControl<T>(DependencyObject control)
                                   where T : DependencyObject
        {
            T foundChild = null;
            int childNumber = VisualTreeHelper.GetChildrenCount(control);
            for (int i = 0; i < childNumber; i++)
            {
                var child = VisualTreeHelper.GetChild(control, i);
                if (child != null && child is T)
                    foundChild = (T)child;
                else
                    foundChild = FindChildControl<T>(child);
            }
            return foundChild;
        }

        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            var nav = new Navigate();
            mainPage.MainContent = nav;

            if (Trip.Trip.Location == null)
            {
                Trip.Trip.Location = await Backend.MapService.RetrieveTrip(Trip.Trip);
            }

            nav.SetDestination(Trip.Trip.Location);
        }

        private void TripTitle_IsEditingChanged(bool isEditing, Item item)
        {
            // Update if not editing
            if (!isEditing)
            {
                Backend.Azure.SaveTripWithoutItems(Trip);
            }
        }

        private void CheckboxLoad(object sender, RoutedEventArgs e)
        {
            CheckBox box = sender as CheckBox;
            box?.AddHandler(CheckBox.KeyDownEvent, new KeyEventHandler(CheckboxKeyDown), true);
        }

        private bool usingSpace = false;

        private void CheckboxKeyDown(object sender, KeyRoutedEventArgs routedEventArgs)
        {
            if (routedEventArgs.Key == VirtualKey.Space)
            {
                var content = ((CheckBox) sender).Content as EditableTextBlock;
                routedEventArgs.Handled = false;
                usingSpace = true;
            }
        }
    }
}
