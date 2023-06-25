using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaz
{
    public interface IRoutes
    {
        public Task<List<Journey>> getRoundTripJourney(string origin, string destination);
        public Task<Journey> getOneWayJourney(string origin, string destination);
        
    }
}
