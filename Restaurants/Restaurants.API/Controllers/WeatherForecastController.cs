using Microsoft.AspNetCore.Mvc;

namespace Restaurants.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherForecastController : ControllerBase
{
   
    private readonly IWeatherForecastService _weatherForecastService;
    public class TemperatureRequest()
    {
        public int Min { get; set;}
        public int Max { get; set;}
    }

    public WeatherForecastController(IWeatherForecastService weatherForecastService)
    {
        _weatherForecastService = weatherForecastService;
    }
    

    [HttpPost]
    public string Hello([FromBody] string name)
    {
        return $"Hello {name}";
    }

    [HttpPost]
    [Route("generate")]
    public IActionResult GetDetail([FromQuery] int count, [FromBody] TemperatureRequest request)
    {
        if (count == 0) {
            return BadRequest($"The count is not right: {count}");
        }

        var result = _weatherForecastService.Get(count, request.Min, request.Max);
        if (result != null)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }
}
