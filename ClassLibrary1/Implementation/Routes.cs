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
using DataAccess.Validators;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.Implementation
{
    public class Routes : IRoutes
    {
        private readonly IServiceFlights _serviceFlights;
        private readonly ILogger<Routes> _logger;
        //private readonly IFlightBuilder _flightGraph;
        private readonly IGraphBuilder _graphBuilder;
        private readonly IShortestRouteFinder _shortestRouteFinder;

        public Routes(IServiceFlights serviceFlights, ILogger<Routes> logger, IGraphBuilder graphBuilder, IShortestRouteFinder shortestRouteFinder) 
        {
            _serviceFlights = serviceFlights;
            _logger = logger;
            //_flightGraph = flightGraph;
            _graphBuilder = graphBuilder;
            _shortestRouteFinder = shortestRouteFinder;
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
                var adjacencyList = await _graphBuilder.BuildGraph(flights);

                _logger.LogInformation($"Finding route by origin and destination");
                var shortestRoute = await _shortestRouteFinder.FindShortestRoute(origin, destination, adjacencyList);

                //Map FlightDto to Flight
                List<Flight> flightList = shortestRoute.Select(flightDto => new Flight(
                    new Transport(flightDto.FlightCarrier, flightDto.FlightNumber),
                    flightDto?.DepartureStation,
                    flightDto?.ArrivalStation,
                    flightDto.Price
                )).ToList();



                var journey = new Journey(origin, destination, shortestRoute.Sum(c => c.Price), flightList);

                var journeyValidator = new JourneyValidator();

                var validation = journeyValidator.Validate(journey);

                if(validation.IsValid == false)
                {
                    var errorMessages = "Error in server site: " + validation.ToString();
                    throw new ValidationException(errorMessages);
                }

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
