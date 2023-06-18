using BusinessLayer.Implementation;
using BusinessLayer.Interfaz;
using DataAccess.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Test_WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JourneyController : ControllerBase
    {

        private readonly ILogger<JourneyController> _logger;
        private readonly IRoutes _route;

        public JourneyController(ILogger<JourneyController> logger, IRoutes route)
        {
            _logger = logger;
            _route = route;
        }

        [HttpGet(Name = "GetJourney")]
        public async Task<IActionResult> Get(string origin, string destination)
        {
            try
            {
                _logger.LogInformation("Getting Journey");
                _logger.LogDebug($"Getting Journey for Origin: {origin} and Destination: {destination}");
                var result = await _route.getRoute(origin, destination);

                if (result == null)
                {
                    _logger.LogWarning("Cannot find the Journey");
                    return NotFound("Cannot find the Journey");
                }

                _logger.LogInformation("Journey sent");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the Journey");
                return BadRequest(ex.Message);
            }

        }

    }
}