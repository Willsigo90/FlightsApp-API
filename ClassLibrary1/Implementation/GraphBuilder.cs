using BusinessLayer.Interfaz;
using DataAccess.DTOs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Implementation
{
    public class GraphBuilder : IGraphBuilder
    {
        private Dictionary<string, List<FlightDto>>? graph;
        private ILogger<GraphBuilder> _logger;

        public GraphBuilder(ILogger<GraphBuilder> logger)
        {
            _logger = logger;
        }
        public async Task<Dictionary<string, List<FlightDto>>> BuildGraph(List<FlightDto> flights)
        {
            try
            {
                graph = new Dictionary<string, List<FlightDto>>();

                foreach (var flight in flights)
                {
                    if (!graph.ContainsKey(flight.DepartureStation))
                        graph[flight.DepartureStation] = new List<FlightDto>();

                    graph[flight.DepartureStation].Add(flight);
                }
                return graph;
            }
            catch (Exception ex)
            {
                // Handle the exception or re-throw it if necessary
                _logger.LogError($"an error occurred while building the flight graph: {ex.Message}");
                throw new Exception("An error occurred while building the flight graph.", ex);
            }
        }
    }
}
