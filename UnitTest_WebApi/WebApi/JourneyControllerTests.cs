﻿using BusinessLayer.Interfaz;
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

            var resultJourney = new Journey()
            {
                Origin = origin,
                Destination = destination
            };

            _routeMock.Setup(x => x.getRoute(origin, destination)).ReturnsAsync(resultJourney);

            var _journeyController = new JourneyController(_loggerMock.Object, _routeMock.Object);

            // Act
            var result = await _journeyController.Get(origin, destination);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.That(((Journey)((Microsoft.AspNetCore.Mvc.ObjectResult)result).Value).Destination.Equals("YYY"));
            Assert.That(((Journey)((Microsoft.AspNetCore.Mvc.ObjectResult)result).Value).Origin.Equals("XXX"));
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


            _routeMock.Setup(r => r.getRoute(origin, destination)).ThrowsAsync(new Exception(exceptionMessage));

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
