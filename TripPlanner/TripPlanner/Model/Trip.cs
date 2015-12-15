using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Printing.OptionDetails;
using Windows.Services.Maps;
using Windows.UI.Notifications;
using TripPlanner.Annotations;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace TripPlanner.Model
{
    public class Trip : INotifyPropertyChanged, ISelfRepresentable<Trip>
    {
        [JsonIgnore]
        public Trip Self => this;

        public int Id { get; set; }

        private ObservableCollection<Item> itemList = new ObservableCollection<Item>();

        [JsonProperty("Items")]
        public ObservableCollection<Item> ItemList
        {
            get { return itemList; }
            set { itemList = value; OnPropertyChanged(nameof(ItemList)); }
        }

        [JsonIgnore]
        public BitmapImage PopularImage { get; set; }

        public MapLocation Location { get; set; }

        private string name = "";

        [JsonProperty("Title")]
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

        private DateTime _date = DateTime.Now;

        public DateTime Date
        {
            get { return _date; }
            set { _date = value; OnPropertyChanged(nameof(Date)); }
        }

        public Trip()
        {
        }

        public Trip(int id) : this()
        {
            Id = id;
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
