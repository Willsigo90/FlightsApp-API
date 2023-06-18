using BusinessLayer.Implementation;
using BusinessLayer.Interfaz;
using DataAccess.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Test_WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrencyController : ControllerBase
    {

        private readonly ILogger<JourneyController> _logger;
        private readonly IServiceCurrency _serviceCurrency;

        public CurrencyController(ILogger<JourneyController> logger, IServiceCurrency serviceCurrency)
        {
            _logger = logger;
            _serviceCurrency = serviceCurrency;
        }

        [HttpGet(Name = "GetCurrency")]
        public async Task<IActionResult> Get(string currency)
        {
            try
            {
                _logger.LogInformation("Getting Currency");
                _logger.LogDebug($"Getting Currency for Code: {currency}");
                var result = await _serviceCurrency.GetCurrency(currency);

                if (result == null)
                {
                    _logger.LogWarning("Cannot find the Currency");
                    return NotFound("Cannot find the Currency");
                }

                _logger.LogInformation("Currency sent");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the Currency");
                return BadRequest(ex.Message);
            }

        }
    }
}