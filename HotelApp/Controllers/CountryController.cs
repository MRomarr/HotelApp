using System.Threading.Tasks;
using HotelApp.Interface.Repositories;
using HotelApp.Interface.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ICountryService _countryService;

        public CountryController(ICountryService countryService)
        {
            _countryService = countryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCountries()
        {
            var countries = await _countryService.GetCountriesAsync();
            return Ok(countries);
        }
        [HttpPost]
        public async Task<IActionResult> CreateCountry([FromBody] string countryName)
        {
            var country = await _countryService.CreateCountryAsync(countryName);
            if (country == null)
            {
                return BadRequest("Country already exists or invalid data.");
            }
            return Ok(country);
        }
    }
}
