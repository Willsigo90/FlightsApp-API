using DataAccess.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaz
{
    public interface IServiceFlights
    {
        public Task<List<FlightDto>> GetFlights();
    }
}
