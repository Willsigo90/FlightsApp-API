using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Journey
    {
        public string? Origin { get; set; }
        public string? Destination { get; set; }
        public int Price { get; set; }
        public List<Flight>? Flights { get; set; }

    }
}
