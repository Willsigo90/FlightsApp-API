using BusinessLayer.Interfaz;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test_WebApi.Controllers;

namespace UnitTest_WebApi.WebApi
{
    [TestFixture]
    public class JourneyControllerTests
    {

        [Test]
        public async Task Get_WithValidParameters_ReturnsOkResult()
        {
            // Arrange
            var _loggerMock = new Mock<ILogger<JourneyController>>();
            var _routeMock = new Mock<IRoutes>();

            string origin = "XXX";
            string destination = "YYY";

            var goingJourny = new Journey(origin, destination, 100, new List<Flight>());
            var returnJourny = new Journey(destination, origin, 100, new List<Flight>());

            var completeJourney = new List<Journey>();

            completeJourney.Add(goingJourny);
            completeJourney.Add(returnJourny);


            _routeMock.Setup(x => x.getRoundTripJourney(origin, destination)).ReturnsAsync(completeJourney);

            var _journeyController = new JourneyController(_loggerMock.Object, _routeMock.Object);

            // Act
            var result = await _journeyController.Get(origin, destination);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.That(((List<Journey>)((Microsoft.AspNetCore.Mvc.ObjectResult)result).Value)[0].Destination.Equals("YYY"));
            Assert.That(((List<Journey>)((Microsoft.AspNetCore.Mvc.ObjectResult)result).Value)[0].Origin.Equals("XXX"));
            Assert.That(((List<Journey>)((Microsoft.AspNetCore.Mvc.ObjectResult)result).Value)[1].Destination.Equals("XXX"));
            Assert.That(((List<Journey>)((Microsoft.AspNetCore.Mvc.ObjectResult)result).Value)[1].Origin.Equals("YYY"));
        }

        [Test]
        public async Task Get_WithNullResult_ReturnsNotFoundResult()
        {
            // Arrange
            var _loggerMock = new Mock<ILogger<JourneyController>>();
            var _routeMock = new Mock<IRoutes>();

            string origin = "XXX";
            string destination = "YYY";

            var _journeyController = new JourneyController(_loggerMock.Object, _routeMock.Object);


            // Act
            var result = await _journeyController.Get(origin, destination);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        [Test]
        public async Task Get_WithException_ReturnsBadRequestResult()
        {
            // Arrange
            var _loggerMock = new Mock<ILogger<JourneyController>>();
            var _routeMock = new Mock<IRoutes>();

            string origin = "XXX";
            string destination = "YYY";
            var exceptionMessage = "Test exception";


            _routeMock.Setup(r => r.getRoundTripJourney(origin, destination)).ThrowsAsync(new Exception(exceptionMessage));

            var _journeyController = new JourneyController(_loggerMock.Object, _routeMock.Object);
            // Act
            var result = await _journeyController.Get(origin, destination);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = (BadRequestObjectResult)result;
            Assert.AreEqual(exceptionMessage, badRequestResult.Value);
        }
    }
}
