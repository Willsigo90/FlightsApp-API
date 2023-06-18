using BusinessLayer.Implementation;
using Microsoft.Extensions.Configuration;
using DataAccess.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;

namespace UnitTest_WebApi.BusinessLayer
{
    [TestFixture]
    public class ServiceFlightsTests
    {

        [Test]
        public async Task GetFlights_WithSuccessfulResponse_ReturnsFlightList()
        {
            //Arrange
            var _loggerMock = new Mock<ILogger<ServiceFlights>>();
            var handlerMock = new Mock<HttpMessageHandler>();
            var configurationMock = new Mock<IConfiguration>();

            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(new List<FlightDto>()
                {
                    new FlightDto{
                    ArrivalStation = "XXX",
                    DepartureStation = "YYY",
                    FlightCarrier = "ZZZ",
                    Price = 1000,
                }
                })),
            };

            var mockrequest = new Mock<HttpRequest>();
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            var httpClient = new HttpClient(handlerMock.Object);
            var httpClientFactory = new Mock<IHttpClientFactory>();
            httpClientFactory.Setup(c => c.CreateClient(It.IsAny<string>())).Returns(httpClient);

            configurationMock
                .Setup(x => x.GetSection("ApiConfig:ApiUrlFlights").Value)
                .Returns("https://api.example.com/flighs");

            var _serviceFlights = new ServiceFlights(httpClientFactory.Object, _loggerMock.Object, configurationMock.Object);

            // Act
            var result = await _serviceFlights.GetFlights();

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].DepartureStation, Is.EqualTo("YYY"));
            Assert.That(result[0].ArrivalStation, Is.EqualTo("XXX"));
            Assert.That(result[0].FlightCarrier, Is.EqualTo("ZZZ"));
            Assert.That(result[0].Price, Is.EqualTo(1000));
        }

    }
}