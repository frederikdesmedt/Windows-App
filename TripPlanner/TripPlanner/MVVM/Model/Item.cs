using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripPlanner.MVVM.Model
{
    public class Item : INotifyPropertyChanged
    {
        public string Name { get; set; }
        private Boolean Is_Checked;

        public int Priority
        {
            get { return Priority; }
            set { Priority = value; NotifyPropertyChanged("Priority"); }
        }

        public Item(string naam, int priority)
        {
            Name = naam;
            Priority = priority;
            Is_Checked = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
