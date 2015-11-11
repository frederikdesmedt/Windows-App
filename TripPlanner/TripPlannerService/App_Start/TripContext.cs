﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TripPlannerService.Models;

namespace TripPlannerService
{
    public class TripContext : DbContext
    {
        public virtual DbSet<Trip> Trips { get; set; }
    }
}