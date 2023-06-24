using DataAccess.DTOs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class GraphBuilder: IGraphBuilder
    {
        private Dictionary<string, List<FlightDto>>? adjacencyList;
        private ILogger<GraphBuilder> _logger;

        public GraphBuilder(ILogger<GraphBuilder> logger)
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
    }
}
