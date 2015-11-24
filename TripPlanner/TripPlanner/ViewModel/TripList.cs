using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripPlanner.Model;

namespace TripPlanner.ViewModel
{
    public class TripList : ObservableCollection<TripViewModel>
    {

        public async Task Load()
        {
            foreach (TripViewModel trip in (await Backend.Local.GetTrips()).Select(t => new TripViewModel(t)))
            {
                Add(trip);
            }
        }
    }
}
