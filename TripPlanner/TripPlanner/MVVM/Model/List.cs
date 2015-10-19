using Les3_Data.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripPlanner.MVVM.Model
{
    public class TripList {
        public List<Item> list = new List<Item>();

        public void AddItem(Item item)
        {
            lijst.Add(item);
        }

        public void DeleteItem(Item item)
        {
            lijst.Remove(item);
        }
    }
}
