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
        private readonly IGraphBuilder _graphBuilder;
        private readonly IShortestRouteFinder _shortestRouteFinder;
        private readonly IJourneyBuilder _journeyBuilder;

        public Routes(IServiceFlights serviceFlights, ILogger<Routes> logger, IGraphBuilder graphBuilder, IShortestRouteFinder shortestRouteFinder, IJourneyBuilder journeyBuilder) 
        {
            _serviceFlights = serviceFlights;
            _logger = logger;
            _graphBuilder = graphBuilder;
            _shortestRouteFinder = shortestRouteFinder;
            _journeyBuilder = journeyBuilder;
        }

        public async Task<List<Journey>> getRoundTripJourney(string origin, string destination)
        {
            try
            {
                var flights = await getFlights();

                validateFlight(origin, destination, flights);

                var graph = await buildGraphs(flights);
                var goingflight = await getShortestRoute(origin, destination, graph);
                var returnflight = await getShortestRoute(destination, origin, graph);
                var goingJourney = await _journeyBuilder.buildJourney(origin, destination, goingflight);
                var returnJourney = await _journeyBuilder.buildJourney(destination, origin, returnflight);

                var result = new List<Journey>();
                result.Add(goingJourney);
                result.Add(returnJourney);

                _logger.LogInformation($"Journey found");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while getting the route: {ex.Message}");
                throw; // Re-throw the exception to propagate it to the caller
            }

        }
        public async Task<Journey> getOneWayJourney(string origin, string destination)
        {
            try
            {

                var flights = await getFlights();

                validateFlight(origin, destination, flights);

                var graph = await buildGraphs(flights);
                var flight = await getShortestRoute(origin, destination, graph);
                var journey = await _journeyBuilder.buildJourney(origin, destination, flight);

                _logger.LogInformation($"Journey found");
                return journey;
            }
            catch (Exception ex)
            {

                _logger.LogError($"An error occurred while getting the Journey: {ex.Message}");
                throw; // Re-throw the exception to propagate it to the caller
            }
        }

        private async Task<List<FlightDto>> getFlights()
        {
            _logger.LogInformation($"Geting all Flights");
            var result = await _serviceFlights.GetFlights();

            if (result == null)
            {
                _logger.LogError("No flights found");
                throw new KeyNotFoundException("No flights found");
            }

            return result;
        }

        private async Task<Dictionary<string, List<FlightDto>>> buildGraphs(List<FlightDto> flights)
        {
            _logger.LogInformation($"Start Building Graph");
            var flightGraph = await _graphBuilder.BuildGraph(flights);
            return flightGraph;
        }

        private async Task<List<FlightDto>> getShortestRoute(string origin, string destination, Dictionary<string, List<FlightDto>> flightGraph)
        {

            _logger.LogInformation($"Finding route by origin and destination");

            var route = await _shortestRouteFinder.FindShortestRoute(origin, destination, flightGraph);
            return route;
        }

        private void validateFlight(string origin, string destination, List<FlightDto> flights)
        {

            try
            {
                bool hasOriginStation = flights.Any(flight =>
                flight?.DepartureStation == origin || flight?.ArrivalStation == origin);

                bool hasDestination = flights.Any(flight =>
                flight?.DepartureStation == destination || flight?.ArrivalStation == destination);

                if (!hasOriginStation || !hasDestination)
                {
                    _logger.LogError("Origin or Destination didn't found");
                    throw new KeyNotFoundException("Origin or Destination didn't found");
                }
            }
            catch (Exception ex)
            {

                _logger.LogError($"An error occurred while validate flight: {ex.Message}");
                throw; // Re-throw the exception to propagate it to the caller
            }
        }
       
    }
}
