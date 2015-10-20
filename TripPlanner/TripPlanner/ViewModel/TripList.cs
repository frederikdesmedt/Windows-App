using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripPlanner.Model;

namespace TripPlanner.ViewModel
{
    public class TripList : ObservableCollection<Trip>
    {

        public TripList()
        {
            Add(new Trip("First trip", "\uE1C4"));
            Add(new Trip("Second trip", "\uE1C3"));
            Add(new Trip("Third trip", "\uE129"));
        }
    }
}
