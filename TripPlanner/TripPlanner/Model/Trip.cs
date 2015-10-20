using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Printing.OptionDetails;
using Windows.UI.Notifications;
using TripPlanner.Annotations;
using Windows.UI.Xaml;

namespace TripPlanner.Model
{
    public class Trip : INotifyPropertyChanged, ISelfRepresentable<Trip>
    {
        public Trip Self => this;

        private ObservableCollection<Item> itemList = new ObservableCollection<Item>();

        public ObservableCollection<Item> ItemList
        {
            get { return itemList; }
            set { itemList = value; OnPropertyChanged(nameof(ItemList)); }
        }

        private string name = "";

        public string Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged(nameof(Name)); }
        }

        private string icon = "\uE128";

        public string Icon
        {
            get { return icon; }
            set { icon = value; OnPropertyChanged(nameof(Icon)); }
        }

        public Trip()
        {
            itemList.Add(new Item("Een item", 10));
        }

        public Trip(string name) : this()
        {
            Name = name;
        }

        public Trip(string name, string icon) : this(name)
        {
            Icon = icon;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
