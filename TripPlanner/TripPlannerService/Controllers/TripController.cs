using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            IEnumerable<Trip> trips = (from trip in dbContext.Trips
                               where trip.UserEmail == email
                               select trip).Include(t => t.Items);
            return trips;
        }

        [HttpPost]
        [Route("api/Trip/Save/Trip")]
        public int SaveTrip(Trip trip)
        {
            trip.UserEmail = User.Identity.Name;
            dbContext.Trips.AddOrUpdate(trip);
            dbContext.SaveChanges();
            return trip.Id;
        }

        [HttpPost]
        [Route("api/Trip/Save/Trip/NoItems")]
        public int SaveTripWithoutItems(Trip trip)
        {
            trip.UserEmail = User.Identity.Name;
            if (trip.Id > 0)
            {
                Trip dbTrip = dbContext.Trips.Find(trip.Id);
                if (dbTrip != null)
                {
                    if (!string.IsNullOrEmpty(trip.Title))
                    {
                        dbTrip.Title = trip.Title;
                    }

                    if (!string.IsNullOrEmpty(trip.Icon))
                    {
                        dbTrip.Icon = trip.Icon;
                    }

                    if (trip.Date >= DateTime.Today)
                    {
                        dbTrip.Date = trip.Date;
                    }

                    if (trip.Location != null)
                    {
                        dbTrip.Location = trip.Location;
                    }
                }

                dbContext.SaveChanges();
            }
            else
            {
                return SaveTrip(trip);
            }
            
            return trip.Id;
        }

        [HttpPost]
        [Route("api/Trip/Save/{tripId}/Item")]
        public int SaveItem(int tripId, [FromBody] Item item)
        {
            Trip trip = dbContext.Trips.Find(tripId);
            if (trip != null && trip.UserEmail == User.Identity.Name)
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
                return item.Id;
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
            Trip trip = dbContext.Trips.First(tr => tr.UserEmail.ToLower() == User.Identity.Name.ToLower() && tr.Items.Any(it => it.Id == item.Id));

            if (item != null && trip != null)
            {
                item.IsChecked = isChecked == 1;
                dbContext.SaveChanges();
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        [HttpDelete]
        [Route("api/Trip/Remove/Item/{itemId}")]
        public void RemoveItem(int itemId)
        {
            if (itemId == 0)
            {
                return;
            }

            Item item = dbContext.Items.Find(itemId);
            Trip trip = dbContext.Trips.First(tr => tr.UserEmail == User.Identity.Name && tr.Items.Any(it => it.Id == item.Id));
            if (item != null && trip != null)
            {
                dbContext.Items.Remove(item);
                dbContext.SaveChanges();
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        [HttpDelete]
        [Route("api/Trip/Remove/Trip/{tripId}")]
        public void RemoveTrip(int tripId)
        {
            Trip trip = dbContext.Trips.Find(tripId);
            if (trip.UserEmail == User.Identity.Name)
            {
                dbContext.Trips.Remove(trip);
                dbContext.SaveChanges();
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }
    }
}
