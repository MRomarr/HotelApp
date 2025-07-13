using HotelApp.DTOs.City;

namespace HotelApp.Interface.Services
{
    public interface ICityService
    {
        Task<IEnumerable<CityDetailsDto>> GetCitiesAsync();
        Task<CityDetailsDto> CreateCityAsync(CreateCityDto city);
    }
}
