using HotelApp.DTOs.City;
using HotelApp.Interface.Repositories;
using HotelApp.Interface.Services;

namespace HotelApp.Services
{
    public class CityService : ICityService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CityService(IUnitOfWork UnitOfWork)
        {
            _unitOfWork = UnitOfWork;
        }
        public async Task<CityDetailsDto> CreateCityAsync(CreateCityDto city)
        {
            var cityEntity = new Models.City
            {
                Name = city.Name,
                CountryId = city.CountryId
            };
            await _unitOfWork.Cities.AddAsync(cityEntity);
            await _unitOfWork.SaveAsync();
            return new CityDetailsDto
            {
                Id = cityEntity.Id,
                Name = cityEntity.Name,
                CountryId = cityEntity.CountryId
            };
        }

        public async Task<IEnumerable<CityDetailsDto>> GetCitiesAsync()
        {
            var cities = await _unitOfWork.Cities.GetAllAsync();
            var CitiesDto = cities.Select( city => new CityDetailsDto
            {
                Id = city.Id,
                Name = city.Name,
                CountryId = city.CountryId,
            }).ToList();
            return CitiesDto;
        }
    }
}
