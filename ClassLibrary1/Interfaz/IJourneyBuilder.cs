using DataAccess.DTOs;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaz
{
    public interface IJourneyBuilder
    {
        public Task<Journey> buildJourney(string origin, string destination,List<FlightDto> goingFlightList);
    }
}
