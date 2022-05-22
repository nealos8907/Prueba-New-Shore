using DataAcces;
using Dtos;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class NewShoreRepository : INewShoreRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHandleNewShoreDbRepository _handleNewShoreDbRepository;
        private readonly ILogger<NewShoreRepository> _logger;

        public NewShoreRepository(IHttpClientFactory httpClientFactory, 
            IHandleNewShoreDbRepository handleNewShoreDbRepository,
            ILogger<NewShoreRepository> logger)
        {
            _httpClientFactory = httpClientFactory;
            _handleNewShoreDbRepository = handleNewShoreDbRepository;
            _logger = logger;
        }

        public async Task<JourneyDto> GetRoutesNewShore(RequestJourneyDto requestJourney)
        {
            var journey = new JourneyDto();
            var listFlight = new List<FlightDeserializedDto>();
            try
            {
                var journeyDb = await _handleNewShoreDbRepository.GetRoute(requestJourney);
                if ( journeyDb?.Flights == null)
                {
                    var client = _httpClientFactory.CreateClient();
                    var response = await client.GetAsync("https://recruiting-api.newshore.es/api/flights/1");
                    if (response.IsSuccessStatusCode)
                    {
                        var product = await response.Content.ReadAsStringAsync();
                        listFlight = JsonConvert.DeserializeObject<List<FlightDeserializedDto>>(product);

                        var originFlights = listFlight.Where(x => x.departureStation == requestJourney.Origin).ToList();

                        if (!originFlights.Any()) 
                        {
                            return null;
                        }
                        journey.Origin = requestJourney.Origin;
                        journey.Destination = requestJourney.Destination;

                        if (originFlights.Any(x => x.arrivalStation.Equals(requestJourney.Destination)))
                        {
                            var singleFlight = originFlights.Where(x => x.arrivalStation
                            .Equals(requestJourney.Destination)).FirstOrDefault();

                            return await GetSingleRoute(singleFlight);
                        }
                        else
                        {
                            foreach (var oFlight in originFlights)
                            {
                                var existFlight = listFlight.Where(x => x.departureStation == oFlight.arrivalStation
                                        && x.arrivalStation == requestJourney.Destination).FirstOrDefault();

                                if (existFlight != null)
                                {
                                    var firstFlight = new FlightDto();
                                    firstFlight.Origin = oFlight.departureStation;
                                    firstFlight.Destination = oFlight.arrivalStation;
                                    firstFlight.Price = oFlight.Price;
                                    firstFlight.Transport = new TransportDto()
                                    {
                                        FlightCarrier = oFlight.flightCarrier,
                                        FlightNumber = oFlight.flightNumber
                                    };

                                    var secondFlight = new FlightDto();
                                    secondFlight.Origin = existFlight.departureStation;
                                    secondFlight.Destination = existFlight.arrivalStation;
                                    secondFlight.Price = existFlight.Price;
                                    secondFlight.Transport = new TransportDto()
                                    {
                                        FlightCarrier = existFlight.flightCarrier,
                                        FlightNumber = existFlight.flightNumber
                                    };

                                    journey.Flights = new List<FlightDto>();
                                    journey.Flights.Add(firstFlight);
                                    journey.Flights.Add(secondFlight);
                                    journey.Price = firstFlight.Price + secondFlight.Price;

                                }

                            }
                            if (journey?.Flights == null || !journey.Flights.Any()) 
                            {
                                return null;
                            }

                            await _handleNewShoreDbRepository.SaveRoute(journey);
                            return journey;

                        }
                    }
                    else
                    {
                        return null;
                    };
 
                }
                else
                {
                    return journeyDb;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Fecha:{DateTime.Now}- Error: {ex} -Parameters: {JsonConvert.SerializeObject(requestJourney)}");
                throw;
            }
        }

        private async Task<JourneyDto> GetSingleRoute(FlightDeserializedDto singleFlight )
        {
            var journey = new JourneyDto();
            try
            {
                var flightDto = new FlightDto();
                flightDto.Destination = singleFlight.arrivalStation;
                flightDto.Origin = singleFlight.departureStation;
                flightDto.Price = singleFlight.Price;
                flightDto.Transport = new TransportDto()
                {
                    FlightCarrier = singleFlight.flightCarrier,
                    FlightNumber = singleFlight.flightNumber
                };
                journey.Flights = new List<FlightDto> { flightDto };
                journey.Price = singleFlight.Price;
                journey.Origin = singleFlight.departureStation;
                journey.Destination = singleFlight.arrivalStation;

                await _handleNewShoreDbRepository.SaveRoute(journey);
                return journey;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Fecha:{DateTime.Now}- Error: {ex} -Parameters: {JsonConvert.SerializeObject(singleFlight)}");
                throw;
            }
            return null;

        }

    }
}
