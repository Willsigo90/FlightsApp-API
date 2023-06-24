using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Journey
    {
        private readonly string? origin;
        private readonly string? destination;
        private readonly double price;
        private readonly List<Flight>? flights;

        public Journey(string origin, string destination, double price, List<Flight> flights)
        {
            this.origin = origin;
            this.destination = destination;
            this.price = price;
            this.flights = flights;
        }

        public string Origin
        {
            get { return origin; }
        }
        public string Destination
        {
            get { return destination; }
        }
        public double Price
        {
            get { return price; }
        }
        public List<Flight> Flights
        {
            get { return flights; }
        }
    }
}
