using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TripPlannerService.Models
{
    public class User
    {
        [Key]
        public string Email { get; set; }
        public string Salt { get; set; }
        public string Hash { get; set; }
        public IEnumerable<Trip> PlannedTrips;
    }
}