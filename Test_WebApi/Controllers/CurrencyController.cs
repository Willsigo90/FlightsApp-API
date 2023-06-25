using BusinessLayer.Implementation;
using BusinessLayer.Interfaz;
using DataAccess.DTOs;
using DataAccess.Validators;
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
            if (string.IsNullOrEmpty(currency))
            {
                throw new ArgumentException("The parameter cannot be null or empty.", nameof(currency));
            }
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

                var routeValidator = new CurrencyValidator();

                var validation = routeValidator.Validate(result);

                if (!validation.IsValid)
                {
                    var errorMessages = validation.Errors.Select(x => x.ErrorMessage).ToList();
                    return BadRequest(errorMessages);
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