using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TripPlannerService.App_Start;
using TripPlannerService.Models;

namespace TripPlannerService.Controllers
{
    public class TripController : ApiController
    {
        private TripContext dbContext;

        public IEnumerable<Trip> Trips = new List<Trip>
        {
            new Trip() { Title = "First trip", Items = new List<Item>
            {
                new Item {IsChecked = true, Name = "Sleeping bag", Priority = 100},
                new Item {IsChecked = false, Name = "Pants", Priority = 80}
            }},
            new Trip() { Title = "Second trip", Items = new List<Item>
            {
                new Item {IsChecked = false, Name = "Bed", Priority = 100},
                new Item {IsChecked = true, Name = "Trousers", Priority = 80}
            }}
        };

        public TripController()
        {
            dbContext = new TripContext();
            if (!dbContext.Trips.Any())
            {
                dbContext.Trips.AddRange(Trips);
                dbContext.SaveChanges();
            }
        }
        
        public IEnumerable<Trip> GetAllTrips()
        {
            return dbContext.Trips;
        }
    }
}
