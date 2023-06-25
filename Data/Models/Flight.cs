using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Flight
    {
        private readonly Transport? transport;
        private readonly string? origin;
        private readonly string? destination;
        private readonly double price;
        
        public Flight(Transport transport, string origin, string destination, double price)
        {
            this.transport = transport;
            this.origin = origin;
            this.destination = destination;
            this.price = price;
         }
        public Transport Transport
        {
            get { return transport; }
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
    }
}
