﻿using DataAccess.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public interface IFlightGraph
    {
        public Task<List<FlightDto>> FindShortestRoute(string origin, string destination);
        public Task<Dictionary<string, List<FlightDto>>> BuildGraph(List<FlightDto> flights);
    }
}