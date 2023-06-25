using BusinessLayer.Implementation;
using BusinessLayer.Interfaz;
using BusinessLayer;
using DataAccess.DTOs;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models;

namespace UnitTest_WebApi.BusinessLayer
{
    [TestFixture]
    public class RoutesTests
    {

        [Test]
        public async Task GetRoute_WithValidOriginAndDestination_ReturnsJourney()
        {
            // Arrange
            var _serviceFlightsMock = new Mock<IServiceFlights>();
            var _loggerMock = new Mock<ILogger<Routes>>();
            var _graphBuilderMock = new Mock<IGraphBuilder>();
            var _shortestRouteFinderMock = new Mock<IShortestRouteFinder>();
            var _journeyBuilderMock = new Mock<IJourneyBuilder>();

            var origin = "XXX";
            var destination = "YYY";
            var flightCarrier = "Carrier";
            var flightNumber = "123";

            var flights = new List<Flight>
            {
                new Flight(new Transport(flightCarrier, flightNumber),origin, destination, 100)
            };

            var graph = new Dictionary<string, List<FlightDto>>();

            var journey = new Journey(origin, destination, 100, flights);

            var flightsList = new List<FlightDto>
            {
                new FlightDto
                {
                    DepartureStation = "XXX",
                    ArrivalStation = "YYY",
                    FlightCarrier = "Carrier",
                    FlightNumber = "123",
                    Price = 100
                }
            };

            _serviceFlightsMock.Setup(x => x.GetFlights()).ReturnsAsync(flightsList);
            _graphBuilderMock.Setup(x => x.BuildGraph(flightsList)).ReturnsAsync(graph);
            _shortestRouteFinderMock.Setup(x => x.FindShortestRoute(origin, destination, graph)).ReturnsAsync(flightsList);
            _journeyBuilderMock.Setup(x => x.buildJourney(origin, destination, flightsList)).ReturnsAsync(journey);

            var _routes = new Routes(_serviceFlightsMock.Object, _loggerMock.Object, _graphBuilderMock.Object, _shortestRouteFinderMock.Object, _journeyBuilderMock.Object);

            // Act
            var result = await _routes.getOneWayJourney("XXX", "YYY");

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Origin, Is.EqualTo("XXX"));
            Assert.That(result.Destination, Is.EqualTo("YYY"));
            Assert.That(result?.Flights?.Count, Is.EqualTo(1));
            Assert.That(result?.Flights?[0].Transport?.FlightCarrier, Is.EqualTo("Carrier"));
            Assert.That(result.Flights[0].Transport.FlightNumber, Is.EqualTo("123"));
            Assert.That(result.Flights[0].Price, Is.EqualTo(100));
            Assert.That(result.Price, Is.EqualTo(100));
        }

        [Test]
        public async Task GetRoute_WithEmptyFlights_ReturnsKeyNotFoundException()
        {
            // Arrange
            var flights = new List<FlightDto>();
            var _serviceFlightsMock = new Mock<IServiceFlights>();
            var _loggerMock = new Mock<ILogger<Routes>>();
            var _graphBuilderMock = new Mock<IGraphBuilder>();
            var _shortestRouteFinderMock = new Mock<IShortestRouteFinder>();
            var _journeyBuilderMock = new Mock<IJourneyBuilder>();

            _serviceFlightsMock.Setup(s => s.GetFlights()).ReturnsAsync(flights);

            var _routes = new Routes(_serviceFlightsMock.Object, _loggerMock.Object, _graphBuilderMock.Object, _shortestRouteFinderMock.Object, _journeyBuilderMock.Object);

            // Act and Assert
            Assert.ThrowsAsync<KeyNotFoundException>(async () => await _routes.getOneWayJourney("XXX", "YYY"));
        }

        [Test]
        public async Task GetRoute_WithInvalidOrigin_ReturnsKeyNotFoundException()
        {
            // Arrange
            var _serviceFlightsMock = new Mock<IServiceFlights>();
            var _loggerMock = new Mock<ILogger<Routes>>();
            var _graphBuilderMock = new Mock<IGraphBuilder>();
            var _shortestRouteFinderMock = new Mock<IShortestRouteFinder>();
            var _journeyBuilderMock = new Mock<IJourneyBuilder>();

            var flights = new List<FlightDto>
            {
                new FlightDto { DepartureStation = "XXX1", ArrivalStation = "YYY", FlightCarrier = "Carrier", FlightNumber = "123", Price = 100 }
            };

            _serviceFlightsMock.Setup(s => s.GetFlights()).ReturnsAsync(flights);

            var _routes = new Routes(_serviceFlightsMock.Object, _loggerMock.Object, _graphBuilderMock.Object, _shortestRouteFinderMock.Object, _journeyBuilderMock.Object);

            // Act and Assert
            Assert.ThrowsAsync<KeyNotFoundException>(async () => await _routes.getOneWayJourney("XXX", "YYY"));
        }

    }
}
