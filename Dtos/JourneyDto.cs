﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos
{
    public class JourneyDto
    {
        public string Origin { get; set; }

        public string Destination { get; set; }

        public double Price { get; set; }

        public List<FlightDto>Flights { get; set; }
    }
}
