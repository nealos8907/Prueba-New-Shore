using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    
    public class Flight
    {
        [Key]
        public int FlightId { get; set; }

        public int TransportId { get; set; }
            
        public string Origin { get; set; }

        public string Destination { get; set; }

        public double Price { get; set; }

        public virtual Transport Transport { get; set; }

        public virtual ICollection<JourneyFlight> JourneyFlight { get; set; }

    }
}
