using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Flight
    {
        public Transport? Transport { get; set; }
        public string? Origin { get; set; }
        public string? Destination { get; set; }
        public double Price { get; set; }
    }
}
