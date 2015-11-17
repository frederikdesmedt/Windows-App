using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TripPlannerService.Models;

namespace TripPlannerService
{
    public class TripContext : DbContext
    {
        public DbSet<Trip> Trips { get; set; }
        public DbSet<Item> Items { get; set; }
    }
}