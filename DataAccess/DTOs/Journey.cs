using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class Journey
    {
        public List<Flight> Flights { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public int Price { get; set; }

    }
}
