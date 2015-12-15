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
    public sealed partial class SelectIcon : UserControl
    {

        private readonly DependencyProperty glyph = DependencyProperty.Register("Glyph", typeof(string), typeof(TripPivotItem), null);
        public SelectIcon()
        {
            this.InitializeComponent();
        }

        public string Glyph
        {
            get { return GetValue(glyph) as string; }
            set { SetValue(glyph, value); }
        }
        private void wrapGrid_PointerPressed(object sender, PointerRoutedEventArgs e)
        {                     //is niet de juiste methode, maar dit moet aangeroepen worden als er een icoontje gekozen word.
            TripPivotItem a = new TripPivotItem();
            //a.Glyph = 

            //Azure.fix(); //databank wegschrijven
        }
    }
}
