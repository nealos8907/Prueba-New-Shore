using DataAcces;
using Domain;
using Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class HandleNewShoreDbRepository : IHandleNewShoreDbRepository
    {
        private readonly NewShoreDBContext _newShoreDBContext;
        private readonly ILogger<HandleNewShoreDbRepository> _logger;
        public HandleNewShoreDbRepository(NewShoreDBContext newShoreDBContext, 
            ILogger<HandleNewShoreDbRepository> logger )
        {
            _newShoreDBContext = newShoreDBContext;
            _logger = logger;
        }

        public async Task<bool> SaveRoute(JourneyDto journey)
        {
            var result = false;
            var journeyToSave = new Journey();
            var listJourneyFlightToSave = new List<JourneyFlight>();
            try
            {
                journeyToSave.Origin = journey.Origin;
                journeyToSave.Destination = journey.Destination;
                journeyToSave.Price = journey.Price;

                _newShoreDBContext.Journeys.Add(journeyToSave);
                await _newShoreDBContext.SaveChangesAsync();

                foreach (var flight in journey.Flights)
                {
                    var transportToSave = new Transport();
                    transportToSave.FlightCarrier = flight.Transport.FlightCarrier;
                    transportToSave.FlightNumber = flight.Transport.FlightNumber;

                    _newShoreDBContext.Transports.Add(transportToSave);
                    await _newShoreDBContext.SaveChangesAsync();

                    var flightToSave = new Flight();
                    var existFlightDb = await CheckExistFlightDb(flight);
                    if(existFlightDb == null)
                    {
                        
                        flightToSave.Origin = flight.Origin;
                        flightToSave.Destination = flight.Destination;
                        flightToSave.Price = flight.Price;
                        flightToSave.TransportId = transportToSave.TransportId;

                        _newShoreDBContext.Flights.Add(flightToSave);
                        await _newShoreDBContext.SaveChangesAsync();
                    }

                    var journeyFlightToSave = new JourneyFlight();
                    journeyFlightToSave.JourneyId = journeyToSave.JourneyId;
                    journeyFlightToSave.FlightId = existFlightDb == null ? flightToSave.FlightId : existFlightDb.FlightId;

                    _newShoreDBContext.JourneyFlights.Add(journeyFlightToSave);
                    await _newShoreDBContext.SaveChangesAsync();
                }
                result = true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fecha:{DateTime.Now}- Error: {ex} -Parameters: {JsonConvert.SerializeObject(journey)}");
                throw;
            }
            return result;
        }

        public async Task<JourneyDto> GetRoute(RequestJourneyDto requestJourney) 
        {
            var journeyToShow = new JourneyDto();
            try
            {
                var journey = await _newShoreDBContext.Journeys
                    .Where(y => y.Origin == requestJourney.Origin && y.Destination == requestJourney.Destination)
                    .FirstOrDefaultAsync();

                var existFlight = await _newShoreDBContext.Flights.Include(y => y.Transport)
                    .FirstOrDefaultAsync(x => x.Origin == requestJourney.Origin && 
                    x.Destination == requestJourney.Destination);

                if (journey != null)
                {
                    var listFlightId = await _newShoreDBContext.JourneyFlights
                    .Where(x => x.JourneyId == journey.JourneyId).Select(y => y.FlightId).ToListAsync();

                    journeyToShow.Flights = new List<FlightDto>();
                    foreach (var flightId in listFlightId)
                    {
                        var flight = await _newShoreDBContext.Flights
                            .Where(x => x.FlightId == flightId).Include(y => y.Transport).FirstOrDefaultAsync();

                        var flightToShow = new FlightDto();
                        flightToShow.Origin = flight.Origin;
                        flightToShow.Destination = flight.Destination;
                        flightToShow.Price = flight.Price;
                        flightToShow.Transport = new TransportDto()
                        {
                            FlightCarrier = flight.Transport.FlightCarrier,
                            FlightNumber = flight.Transport.FlightNumber
                        };
                        
                        journeyToShow.Price += flightToShow.Price;
                        journeyToShow.Flights.Add(flightToShow);
                    }
                    journeyToShow.Origin = journey.Origin;
                    journeyToShow.Destination = journey.Destination;
                    return journeyToShow;
                }
                if (existFlight != null)
                {
                    journeyToShow.Origin = existFlight.Origin;
                    journeyToShow.Destination = existFlight.Destination;
                    journeyToShow.Price = existFlight.Price;
                    var flight = new FlightDto()
                    {
                        Origin = existFlight.Origin,
                        Destination = existFlight.Destination,
                        Price = existFlight.Price,
                        Transport = new TransportDto() { FlightCarrier = existFlight.Transport.FlightCarrier,
                            FlightNumber = existFlight.Transport.FlightNumber },
                    };
                    journeyToShow.Flights = new List<FlightDto>();
                    journeyToShow.Flights.Add(flight);

                    return journeyToShow;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fecha:{DateTime.Now}- Error: {ex} -Parameters: {JsonConvert.SerializeObject(requestJourney)}");
                throw;
            }
            return null; 
        }

        private async Task<Flight> CheckExistFlightDb(FlightDto flight)
        {
            var flightToSave = new Flight();
            try
            {   
                flightToSave = await _newShoreDBContext.Flights
                    .FirstOrDefaultAsync(x => x.Origin == flight.Origin && x.Destination == flight.Destination);
                if (flightToSave != null)
                {
                    return flightToSave;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Fecha:{DateTime.Now}- Error: {ex} -Parameters: {JsonConvert.SerializeObject(flight)}");
                throw;
            }
            return null;

        }
       
    }
}
