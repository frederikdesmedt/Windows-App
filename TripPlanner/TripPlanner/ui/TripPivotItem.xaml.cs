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
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace TripPlanner.ui
{
    public sealed partial class TripPivotItem : UserControl
    {
        private readonly DependencyProperty title = DependencyProperty.Register("Title", typeof(string), typeof(TripPivotItem), null);
        private readonly DependencyProperty glyph = DependencyProperty.Register("Glyph", typeof(string), typeof(TripPivotItem), null);

        public string Glyph
        {
            get { return GetValue(glyph) as string; }
            set { SetValue(glyph, value); }
        }

        public string Title
        {
            get { return GetValue(title) as string; }
            set { SetValue(title, value); }
        }

        public TripPivotItem()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }
    }
}
