using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using TripPlanner.Model;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TripPlanner.ui
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TripEditor : Page
    {

        private Trip Trip { get; set; }
        public TripEditor()
        {
            this.InitializeComponent();
        }

        public TripEditor(Trip trip) : this()
        {
            Trip = trip;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Trip = e.Parameter as Trip;
            Bindings.Update();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
