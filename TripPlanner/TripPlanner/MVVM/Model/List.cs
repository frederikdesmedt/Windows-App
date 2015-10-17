using Les3_Data.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripPlanner.MVVM.Model
{
    class List {
        public List<Item> lijst;
        public List()
        {
         lijst = new List<Item>();
        }

        public void Add_Item(Item item)
        {
            lijst.Add(item);
        }

        public void Delete_Item(Item item)
        {
            lijst.Remove(item);
        }
    }
}
