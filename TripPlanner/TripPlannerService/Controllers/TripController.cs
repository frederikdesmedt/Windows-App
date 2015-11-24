using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using TripPlannerService.App_Start;
using TripPlannerService.Models;

namespace TripPlannerService.Controllers
{

    [Authorize]
    public class TripController : ApiController
    {
        private TripContext dbContext;

        public IEnumerable<Trip> Trips = new List<Trip>
        {
            new Trip() { Title = "First trip", UserEmail = "test@outlook.com", Icon = "\uE1C4", Items = new List<Item>
            {
                new Item {IsChecked = true, Name = "Sleeping bag", Priority = 100},
                new Item {IsChecked = false, Name = "Pants", Priority = 80}
            }},
            new Trip() { Title = "Second trip", UserEmail = "test@outlook.com", Icon = "\uE1C3", Items = new List<Item>
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
            string email = User.Identity.Name;
            return from trip in dbContext.Trips
                   where trip.UserEmail == email
                   select trip;
        }

        [HttpPost]
        [Route("api/Trip/Save/Trip")]
        public void SaveTrip(Trip trip)
        {
            trip.UserEmail = User.Identity.Name;
            dbContext.Trips.AddOrUpdate(trip);
            //dbContext.SaveChanges();
        }

        [HttpPost]
        [Route("api/Trip/Save/{tripId}/Item")]
        public void SaveItem(int tripId,  [FromBody] Item item)
        {
            Trip trip = dbContext.Trips.Find(tripId);
            if (trip != null)
            {
                bool foundExisting = false;
                foreach (var existingItem in new List<Item>(trip.Items.Where(i => i.Id == item.Id)))
                {
                    foundExisting = true;
                    existingItem.IsChecked = item.IsChecked;
                    existingItem.Name = item.Name;
                    existingItem.Priority = item.Priority;
                }

                if (!foundExisting)
                {
                    trip.Items.Add(item);
                }
                dbContext.SaveChanges();
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        [HttpPost]
        [Route("api/Trip/Check/{itemId}/{isChecked}")]
        public void UpdateItemChecked(int itemId, int isChecked)
        {
            Item item = dbContext.Items.Find(itemId);
            if (item != null)
            {
                item.IsChecked = isChecked == 1;
                dbContext.SaveChanges();
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }
    }
}
