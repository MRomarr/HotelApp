using HotelApp.DTOs.City;
using HotelApp.Interface.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController :ControllerBase
    {
        private readonly ICityService _cityService;
        public CityController(ICityService cityService)
        {
            _cityService = cityService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCities()
        {
            var cities = await _cityService.GetCitiesAsync();
            return Ok(cities);
        }
        [HttpPost]
        public async Task<IActionResult> CreateCity([FromBody] CreateCityDto createCityDto)
        {
            if (createCityDto == null || string.IsNullOrEmpty(createCityDto.Name) || string.IsNullOrEmpty(createCityDto.CountryId))
            {
                return BadRequest("Invalid city data.");
            }
            var createdCity = await _cityService.CreateCityAsync(createCityDto);
            return Ok(createdCity);
        }
        
    }
}
