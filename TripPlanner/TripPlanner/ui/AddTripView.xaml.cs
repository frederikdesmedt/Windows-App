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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace TripPlanner.ui
{
    public sealed partial class AddTripView : UserControl
    {
        private List<string> Glyphs { get; set; } = new List<string>
        {
            "\uE10F", "\uE113", "\uE11D", "\uE122", "\uE128", "\uE12B", "\uE139", "\uE13D", "\uE14D", "\uE163", "\uE170", "\uE192", "\uE19F", "\uE19E", "\uE1C3", "\uE1C4", "\uE1DE"
        };

        public delegate void TripAddedDelegate(string name, DateTime date, string glyph);

        public event TripAddedDelegate TripAdded;

        public AddTripView()
        {
            this.InitializeComponent();
            Name.Focus(FocusState.Pointer);
        }


        private void OnAddTrip(object sender, RoutedEventArgs e)
        {
            TripAdded?.Invoke(Name.Text, Date.Date?.Date ?? DateTime.Today, GlyphGrid.SelectedItem as string ?? "\uE128");
        }
    }
}
