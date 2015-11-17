using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripPlanner.Model
{
    public class Item : INotifyPropertyChanged
    {
        private string name = "";

        public string Name
        {
            get { return name; }
            set { name = value; NotifyPropertyChanged("Name"); }
        }

        private bool? isChecked;

        public bool? IsChecked
        {
            get { return isChecked; }
            set { isChecked = value; NotifyPropertyChanged("Checked"); }
        }

        private int priority;
        public int Priority
        {
            get { return priority; }
            set { priority = value; NotifyPropertyChanged("Priority"); }
        }

        public Item()
        {
        }

        public Item(string name, int priority, bool isChecked = false)
        {
            Name = name;
            Priority = priority;
            IsChecked = isChecked;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
