using DataAccess.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public interface IGraphBuilder
    {
        Task<Dictionary<string, List<FlightDto>>> BuildGraph(List<FlightDto> flights);
    }
}
