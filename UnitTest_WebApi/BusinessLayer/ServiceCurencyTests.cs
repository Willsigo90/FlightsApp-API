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
using BusinessLayer.Interfaz;

namespace UnitTest_WebApi.BusinessLayer
{
    [TestFixture]
    public class ServiceCurencyTests
    {

        [Test]
        public async Task GetCurrency_WithSuccessfulResponse_ReturnsCurrency()
        {
            // Arrange
            string currency = "USD";
            double expectedRate = 1.0;

            var _loggerMock = new Mock<ILogger<ServiceFlights>>();
            var handlerMock = new Mock<HttpMessageHandler>();
            var configurationMock = new Mock<IConfiguration>();

            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{\"conversion_rates\": {\"USD\": 1.0}}")
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
                .Setup(x => x.GetSection("ApiConfig:ApiUrlCurrency").Value)
                .Returns("https://api.example.com/currency");

            var _serviceCurrency = new ServiceCurrency(httpClientFactory.Object, _loggerMock.Object, configurationMock.Object);

            // Act
            var result = await _serviceCurrency.GetCurrency(currency);

            // Assert
            Assert.That(result.Currency, Is.EqualTo(currency));
            Assert.That(result.Rate, Is.EqualTo(expectedRate));

        }

    }
}