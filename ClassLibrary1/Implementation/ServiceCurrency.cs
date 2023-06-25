using BusinessLayer.Interfaz;
using DataAccess.DTOs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;
using DataAccess.Models;
using System.Reflection.Metadata;

namespace BusinessLayer.Implementation
{
    public class ServiceCurrency : IServiceCurrency
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ServiceFlights> _logger;
        private readonly IConfiguration _configuration;
        private readonly ObjectCache _cache;

        public ServiceCurrency(IHttpClientFactory httpClientFactory, ILogger<ServiceFlights> logger, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _configuration = configuration;
            _cache = MemoryCache.Default;
        }
        public async Task<CurrencyDTO> GetCurrency(string currency)
        {
            if (string.IsNullOrEmpty(currency))
            {
                throw new ArgumentException("The parameter cannot be null or empty.", nameof(currency));
            }
            string cacheKey = currency.ToUpper();
            // Verificar si el resultado está en caché
            if (_cache.Contains(cacheKey))
            {
                _logger.LogInformation("Retrieving currency from cache.");
                return (CurrencyDTO)_cache.Get(cacheKey);
            }

            currency = currency.ToUpper();
            string apiUrl = _configuration.GetSection("ApiConfig:ApiUrlCurrency").Value;

            HttpClient httpClient = _httpClientFactory.CreateClient();

            //string apiUrl = "https://v6.exchangerate-api.com/v6/7cb5ee61f8e7a15dd1499bb1/latest/USD";

            var currencyResult = new CurrencyDTO();

            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Successful call to the api that returns currency. Status code: " + response.StatusCode);

                    string content = await response.Content.ReadAsStringAsync();

                    dynamic jsonObject = JsonConvert.DeserializeObject(content);
                    double zwlRate = jsonObject?.conversion_rates[currency];


                    currencyResult.Rate = zwlRate;
                    currencyResult.Currency = currency;

                    CacheItemPolicy cachePolicy = new CacheItemPolicy();
                    _cache.Set(cacheKey, currencyResult, cachePolicy);

                    _logger.LogInformation("Successfully deserialized response from api: ");

                }
                else
                {
                    _logger.LogError("The request to the Currency service was not successful. Status code: " + response.StatusCode);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred during the request to the Currency service: " + ex.Message);
                throw new Exception("An error occurred while calling the Currency service.", ex);
            }

            return currencyResult;

        }
    }
}
