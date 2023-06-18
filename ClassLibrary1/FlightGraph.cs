﻿using BusinessLayer.Implementation;
using DataAccess.DTOs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class FlightGraph : IFlightGraph
    {
        private Dictionary<string, List<FlightDto>>? adjacencyList;
        private ILogger<FlightGraph> _logger;

        public FlightGraph(ILogger<FlightGraph> logger)
        {
            _logger = logger;
        }


        public async Task<Dictionary<string, List<FlightDto>>> BuildGraph(List<FlightDto> flights)
        {
            try
            {
                adjacencyList = new Dictionary<string, List<FlightDto>>();

                foreach (var flight in flights)
                {
                    if (!adjacencyList.ContainsKey(flight.DepartureStation))
                        adjacencyList[flight.DepartureStation] = new List<FlightDto>();

                    adjacencyList[flight.DepartureStation].Add(flight);
                }
                return adjacencyList;
            }
            catch (Exception ex)
            {
                // Handle the exception or re-throw it if necessary
                _logger.LogError($"an error occurred while getting the route: {ex.Message}");
                throw new Exception("An error occurred while building the flight graph.", ex);
            }
        }

        public async Task<List<FlightDto>> FindShortestRoute(string source, string destination)
        {
            try
            {
                var distances = new Dictionary<string, int>();
                var previous = new Dictionary<string, FlightDto>();
                var priorityQueue = new SortedSet<(int distance, string station)>();

                foreach (var station in adjacencyList.Keys)
                {
                    distances[station] = int.MaxValue;
                    previous[station] = null;
                }

                distances[source] = 0;
                priorityQueue.Add((0, source));

                while (priorityQueue.Count > 0)
                {
                    var (distance, currentStation) = priorityQueue.Min;
                    priorityQueue.Remove((distance, currentStation));

                    if (currentStation == destination)
                        break;

                    if (distance > distances[currentStation])
                        continue;

                    foreach (var flight in adjacencyList[currentStation])
                    {
                        var newDistance = distance + flight.Price;

                        if (newDistance < distances[flight.ArrivalStation])
                        {
                            distances[flight.ArrivalStation] = newDistance;
                            previous[flight.ArrivalStation] = flight;
                            priorityQueue.Add((newDistance, flight.ArrivalStation));
                        }
                    }
                }

                return await ReconstructRoute(previous, destination);

            }
            catch (Exception ex)
            {
                // Handle the exception or re-throw it if necessary
                _logger.LogError($"An error occurred while finding the shortest route: {ex.Message}");
                throw new Exception("An error occurred while finding the shortest route.", ex);
            }
        }

        private async Task<List<FlightDto>> ReconstructRoute(Dictionary<string, FlightDto> previous, string destination)
        {
            try
            {
                var route = new List<FlightDto>();
                var currentStation = destination;

                while (previous[currentStation] != null)
                {
                    var flight = previous[currentStation];
                    route.Insert(0, flight);
                    currentStation = flight.DepartureStation;
                }

                return route;
            }
            catch (Exception ex)
            {
                // Handle the exception or re-throw it if necessary
                _logger.LogError($"An error occurred while reconstructing the route: {ex.Message}");
                throw new Exception("An error occurred while reconstructing the route.", ex);
            }
        }
    }
}
