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
            var _flightGraphMock = new Mock<IFlightGraph>();

            var flights = new List<FlightDto>
            {
                new FlightDto { DepartureStation = "XXX", ArrivalStation = "YYY", FlightCarrier = "Carrier", FlightNumber = "123", Price = 100 }
            };

            _serviceFlightsMock.Setup(s => s.GetFlights()).ReturnsAsync(flights);
            //_flightGraphMock.Setup(g => g.BuildGraph(It.IsAny<List<FlightDto>>())).Returns(Task.CompletedTask);
            _flightGraphMock.Setup(g => g.FindShortestRoute("XXX", "YYY")).ReturnsAsync(flights);

            var _routes = new Routes(_serviceFlightsMock.Object, _loggerMock.Object, _flightGraphMock.Object);

            // Act
            var result = await _routes.getRoute("XXX", "YYY");

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
            var _serviceFlightsMock = new Mock<IServiceFlights>();
            var flights = new List<FlightDto>();
            var _loggerMock = new Mock<ILogger<Routes>>();
            var _flightGraphMock = new Mock<IFlightGraph>();

            _serviceFlightsMock.Setup(s => s.GetFlights()).ReturnsAsync(flights);

            var _routes = new Routes(_serviceFlightsMock.Object, _loggerMock.Object, _flightGraphMock.Object);

            // Act and Assert
            Assert.ThrowsAsync<KeyNotFoundException>(async () => await _routes.getRoute("XXX", "YYY"));
        }

        [Test]
        public async Task GetRoute_WithInvalidOrigin_ReturnsKeyNotFoundException()
        {
            // Arrange
            var _serviceFlightsMock = new Mock<IServiceFlights>();
            var _loggerMock = new Mock<ILogger<Routes>>();
            var _flightGraphMock = new Mock<IFlightGraph>();

            var flights = new List<FlightDto>
            {
                new FlightDto { DepartureStation = "XXX1", ArrivalStation = "YYY", FlightCarrier = "Carrier", FlightNumber = "123", Price = 100 }
            };

            _serviceFlightsMock.Setup(s => s.GetFlights()).ReturnsAsync(flights);

            var _routes = new Routes(_serviceFlightsMock.Object, _loggerMock.Object, _flightGraphMock.Object);

            // Act and Assert
            Assert.ThrowsAsync<KeyNotFoundException>(async () => await _routes.getRoute("XXX", "YYY"));
        }

    }
}
