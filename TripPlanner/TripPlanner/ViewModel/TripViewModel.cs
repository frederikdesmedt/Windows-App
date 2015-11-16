using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TripPlanner.Annotations;
using TripPlanner.Model;

namespace TripPlanner.ViewModel
{
    public class TripViewModel : ISelfRepresentable<TripViewModel>, INotifyPropertyChanged
    {
        public TripViewModel Self => this;

        private Trip _trip;

        public Trip Trip
        {
            get { return _trip; }
            set { _trip = value; OnPropertyChanged(nameof(Trip)); }
        }

        private bool _isEditable;

        public bool IsEditable
        {
            get { return _isEditable; }
            set { _isEditable = value; OnPropertyChanged(nameof(IsEditable)); }
        }

        public TripViewModel(Trip trip, bool isEditable = false)
        {
            Trip = trip;
            IsEditable = isEditable;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
