using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TripPlannerService.Models;

namespace TripPlannerService.App_Start
{
    public class UserContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; }
    }
}