using BusinessLayer.Interfaz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DataAccess.DTOs;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using DataAccess.Models;

namespace BusinessLayer.Implementation
{
    public class Routes : IRoutes
    {
        private readonly IServiceFlights _serviceFlights;
        private readonly ILogger<Routes> _logger;
        private readonly IFlightGraph _flightGraph;

        public Routes(IServiceFlights serviceFlights, ILogger<Routes> logger, IFlightGraph flightGraph) 
        {
            _serviceFlights = serviceFlights;
            _logger = logger;
            _flightGraph = flightGraph;
        }

        public async Task<Journey> getRoute(string origin, string destination)
        {
            try
            {
                origin = origin.ToUpper();
                destination = destination.ToUpper();

                _logger.LogInformation($"Geting all Flights");
                var flights = await _serviceFlights.GetFlights();

                if(flights?.Count == 0)
                {
                    _logger.LogError("No flights found");
                    throw new KeyNotFoundException("No flights found");
                }

                bool hasOriginStation = flights.Any(flight =>
                flight?.DepartureStation == origin || flight?.ArrivalStation == origin);

                bool hasDestination = flights.Any(flight =>
                flight?.DepartureStation == destination || flight?.ArrivalStation == destination);

                if (!hasOriginStation || !hasDestination)
                {
                    _logger.LogError("Origin or Destination didn't found");
                    throw new KeyNotFoundException("Origin or Destination didn't found");
                }

                _logger.LogInformation($"Start Building Graph");
                await _flightGraph.BuildGraph(flights);

                _logger.LogInformation($"Finding route by origin and destination");
                var shortestRoute = await _flightGraph.FindShortestRoute(origin, destination);

                //Map FlightDto to Flight
                List<Flight> flightList = shortestRoute.Select(flightDto => new Flight
                {
                    Transport = new Transport
                    {
                        FlightCarrier = flightDto.FlightCarrier,
                        FlightNumber = flightDto.FlightNumber
                    },
                    Origin = flightDto.DepartureStation,
                    Destination = flightDto.ArrivalStation,
                    Price = flightDto.Price
                }).ToList();

                var journey = new Journey();

                journey.Origin = origin;
                journey.Destination = destination;
                journey.Flights = flightList;
                journey.Price = shortestRoute.Sum(c => c.Price);

                _logger.LogInformation($"Journey found");
                return journey;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while getting the route: {ex.Message}");
                throw; // Re-throw the exception to propagate it to the caller
            }

        }
    }
}
