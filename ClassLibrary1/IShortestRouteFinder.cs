using DataAccess.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public interface IShortestRouteFinder
    {
        Task<List<FlightDto>> FindShortestRoute(string origin, string destination, Dictionary<string, List<FlightDto>>? adjacencyList);
    }
}
