using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Web;

namespace TripPlannerService.Models
{
    public class Trip
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }

        public virtual ICollection<Item> Items { get; set; }
        public string UserEmail { get; set; }
        public DateTime Date;
        public DbGeography Location;
    }
}