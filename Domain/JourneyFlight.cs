using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class JourneyFlight
    { 
        public int JourneyFlightId { get; set; }

        public int JourneyId { get; set; }

        public int FlightId { get; set; }

        public virtual Journey Journey { get; set; }

        public virtual Flight Flight { get; set; }
    }
}
