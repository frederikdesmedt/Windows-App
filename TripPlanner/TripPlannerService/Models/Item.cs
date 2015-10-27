using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TripPlannerService.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsChecked { get; set; }
        public int Priority { get; set; }
    }
}