using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcces
{
    public  class NewShoreDBContext:DbContext
    {
        public NewShoreDBContext(DbContextOptions option) : base(option)
        { 
        
        }

        public virtual DbSet<Journey> Journeys { get; set; }
        public virtual DbSet<Flight> Flights { get; set; }
        public virtual DbSet<Transport> Transports { get; set; }
        public virtual DbSet<JourneyFlight> JourneyFlights { get; set; }
    }
}
