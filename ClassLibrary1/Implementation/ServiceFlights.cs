using BusinessLayer.Interfaz;
using DataAccess.DTOs;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Runtime.Caching;

namespace BusinessLayer.Implementation
{
    public class ServiceFlights : IServiceFlights
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ServiceFlights> _logger;
        private readonly IConfiguration _configuration;
        private readonly ObjectCache _cache;

        public ServiceFlights(IHttpClientFactory httpClientFactory, ILogger<ServiceFlights> logger, IConfiguration configuration/*, ObjectCache cache*/)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _configuration = configuration;
            //_cache = cache;
            _cache = MemoryCache.Default;

        }
        //public FlightDto GetFlights()
        public async Task<List<FlightDto>> GetFlights()
        {
            // Verificar si el resultado está en caché
            string cacheKey = "FlightsCacheKey";
            if (_cache.Contains(cacheKey))
            {
                _logger.LogInformation("Retrieving flights from cache.");
                return (List<FlightDto>)_cache.Get(cacheKey);
            }

            HttpClient httpClient = _httpClientFactory.CreateClient();

            //string apiUrl = "https://recruiting-api.newshore.es/api/flights/2";
            string apiUrl = _configuration.GetSection("ApiConfig:ApiUrlFlights").Value;

            var flights = new List<FlightDto>();

            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Successful call to the api that returns flights. Status code: " + response.StatusCode);

                    string content = await response.Content.ReadAsStringAsync();

                    flights = JsonConvert.DeserializeObject<List<FlightDto>>(content);

                    _logger.LogInformation("Successfully deserialized response from api: ");

                    CacheItemPolicy cachePolicy = new CacheItemPolicy();
                    _cache.Set(cacheKey, flights, cachePolicy);

                }
                else
                {
                    _logger.LogError("The request to the Flights service was not successful. Status code: " + response.StatusCode);
                    
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while the request to the Flights service: " + ex.Message);
                throw new Exception("An error occurred while calling the service.", ex);
            }

            return flights;
        }
    }
}
