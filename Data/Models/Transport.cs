using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Transport
    {
        //public string? FlightCarrier { get; set; }
        //public string? FlightNumber { get; set; }

        private readonly string? flightCarrier;
        private readonly string? flightNumber;

        public Transport(string? flightCarrier, string? flightNumber)
        {
            this.flightNumber = flightNumber;
            this.flightCarrier = flightCarrier;
        }

        public string? FlightNumber { get {  return flightNumber; } }
        public string? FlightCarrier { get { return flightCarrier; } }
    }
}
