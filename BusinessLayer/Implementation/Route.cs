using BusinessLayer.Interfaz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DataAccess.DTOs;

namespace BusinessLayer.Implementation
{
    public class Route : IRoute
    {
        public Route() { }

        public Journey getRoute(string origin, string destination)
        {
            var json = @"
            [
                {
                    ""departureStation"": ""MZL"",
                    ""arrivalStation"": ""MDE"",
                    ""flightCarrier"": ""CO"",
                    ""flightNumber"": ""8001"",
                    ""price"": 200
                },
                {
                    ""departureStation"": ""MZL"",
                    ""arrivalStation"": ""CTG"",
                    ""flightCarrier"": ""CO"",
                    ""flightNumber"": ""8002"",
                    ""price"": 200
                },
                {
                    ""departureStation"": ""PEI"",
                    ""arrivalStation"": ""BOG"",
                    ""flightCarrier"": ""CO"",
                    ""flightNumber"": ""8003"",
                    ""price"": 200
                },
                {
                    ""departureStation"": ""MDE"",
                    ""arrivalStation"": ""BCN"",
                    ""flightCarrier"": ""CO"",
                    ""flightNumber"": ""8004"",
                    ""price"": 500
                },
                {
                    ""departureStation"": ""CTG"",
                    ""arrivalStation"": ""CAN"",
                    ""flightCarrier"": ""CO"",
                    ""flightNumber"": ""8005"",
                    ""price"": 300
                },
                {
                    ""departureStation"": ""BOG"",
                    ""arrivalStation"": ""MAD"",
                    ""flightCarrier"": ""CO"",
                    ""flightNumber"": ""8006"",
                    ""price"": 500
                },
                {
                    ""departureStation"": ""BOG"",
                    ""arrivalStation"": ""MEX"",
                    ""flightCarrier"": ""CO"",
                    ""flightNumber"": ""8007"",
                    ""price"": 300
                },
                {
                    ""departureStation"": ""MZL"",
                    ""arrivalStation"": ""PEI"",
                    ""flightCarrier"": ""CO"",
                    ""flightNumber"": ""8008"",
                    ""price"": 200
                },
                {
                    ""departureStation"": ""MDE"",
                    ""arrivalStation"": ""CTG"",
                    ""flightCarrier"": ""CO"",
                    ""flightNumber"": ""8009"",
                    ""price"": 200
                },
                {
                    ""departureStation"": ""BOG"",
                    ""arrivalStation"": ""CTG"",
                    ""flightCarrier"": ""CO"",
                    ""flightNumber"": ""8010"",
                    ""price"": 200
                },
                {
                    ""departureStation"": ""MDE"",
                    ""arrivalStation"": ""MZL"",
                    ""flightCarrier"": ""CO"",
                    ""flightNumber"": ""9001"",
                    ""price"": 200
                },
                {
                    ""departureStation"": ""CTG"",
                    ""arrivalStation"": ""MZL"",
                    ""flightCarrier"": ""CO"",
                    ""flightNumber"": ""9002"",
                    ""price"": 200
                },
                {
                    ""departureStation"": ""BOG"",
                    ""arrivalStation"": ""PEI"",
                    ""flightCarrier"": ""CO"",
                    ""flightNumber"": ""9003"",
                    ""price"": 200
                },
                {
                    ""departureStation"": ""BCN"",
                    ""arrivalStation"": ""MDE"",
                    ""flightCarrier"": ""ES"",
                    ""flightNumber"": ""9004"",
                    ""price"": 500
                },
                {
                    ""departureStation"": ""CAN"",
                    ""arrivalStation"": ""CTG"",
                    ""flightCarrier"": ""MX"",
                    ""flightNumber"": ""9005"",
                    ""price"": 300
                },
                {
                    ""departureStation"": ""MAD"",
                    ""arrivalStation"": ""BOG"",
                    ""flightCarrier"": ""ES"",
                    ""flightNumber"": ""9006"",
                    ""price"": 500
                },
                {
                    ""departureStation"": ""MEX"",
                    ""arrivalStation"": ""BOG"",
                    ""flightCarrier"": ""MX"",
                    ""flightNumber"": ""9007"",
                    ""price"": 300
                },
                {
                    ""departureStation"": ""PEI"",
                    ""arrivalStation"": ""MZL"",
                    ""flightCarrier"": ""CO"",
                    ""flightNumber"": ""9008"",
                    ""price"": 200
                },
                {
                    ""departureStation"": ""CTG"",
                    ""arrivalStation"": ""MDE"",
                    ""flightCarrier"": ""CO"",
                    ""flightNumber"": ""9009"",
                    ""price"": 200
                },
                {
                    ""departureStation"": ""CTG"",
                    ""arrivalStation"": ""BOG"",
                    ""flightCarrier"": ""CO"",
                    ""flightNumber"": ""9010"",
                    ""price"": 200
                }
            ]";

            var flights = JsonConvert.DeserializeObject<List<Flight>>(json);
            var graph = new FlightGraph(flights);

            //var origin = "BCN";
            //var destination = "CAN";

            var shortestRoute = graph.FindShortestRoute(origin, destination);

            Console.WriteLine("Shortest Route:");

            //foreach (var flight in shortestRoute)
            //{
            //    Console.WriteLine($"Flight {flight.flightNumber} - {flight.departureStation} to {flight.arrivalStation}, Price: {flight.price}");
            //}

            var journey = new Journey();

            journey.Origin = origin;
            journey.Destination = destination;
            journey.Flights = shortestRoute;
            journey.Price = shortestRoute.Sum(c => c.Price);

            return journey;

            


        }
    }
}

public class FlightGraph
{
    private Dictionary<string, List<Flight>> adjacencyList;

    public FlightGraph(List<Flight> flights)
    {
        BuildGraph(flights);
    }

    private void BuildGraph(List<Flight> flights)
    {
        adjacencyList = new Dictionary<string, List<Flight>>();

        foreach (var flight in flights)
        {
            if (!adjacencyList.ContainsKey(flight.DepartureStation))
                adjacencyList[flight.DepartureStation] = new List<Flight>();

            adjacencyList[flight.DepartureStation].Add(flight);
        }
    }

    public List<Flight> FindShortestRoute(string source, string destination)
    {
        var distances = new Dictionary<string, int>();
        var previous = new Dictionary<string, Flight>();
        var priorityQueue = new SortedSet<(int distance, string station)>();

        foreach (var station in adjacencyList.Keys)
        {
            distances[station] = int.MaxValue;
            previous[station] = null;
        }

        distances[source] = 0;
        priorityQueue.Add((0, source));

        while (priorityQueue.Count > 0)
        {
            var (distance, currentStation) = priorityQueue.Min;
            priorityQueue.Remove((distance, currentStation));

            if (currentStation == destination)
                break;

            if (distance > distances[currentStation])
                continue;

            foreach (var flight in adjacencyList[currentStation])
            {
                var newDistance = distance + flight.Price;

                if (newDistance < distances[flight.ArrivalStation])
                {
                    distances[flight.ArrivalStation] = newDistance;
                    previous[flight.ArrivalStation] = flight;
                    priorityQueue.Add((newDistance, flight.ArrivalStation));
                }
            }
        }

        return ReconstructRoute(previous, destination);
    }

    private List<Flight> ReconstructRoute(Dictionary<string, Flight> previous, string destination)
    {
        var route = new List<Flight>();
        var currentStation = destination;

        while (previous[currentStation] != null)
        {
            var flight = previous[currentStation];
            route.Insert(0, flight);
            currentStation = flight.DepartureStation;
        }

        return route;
    }
}