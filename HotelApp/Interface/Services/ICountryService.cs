using HotelApp.DTOs.Country;

namespace HotelApp.Interface.Services
{
    public interface ICountryService
    {
        Task <List<CountryDetailsDto>> GetCountriesAsync();
        Task <CountryDetailsDto> CreateCountryAsync(string countryName);

    }
}
