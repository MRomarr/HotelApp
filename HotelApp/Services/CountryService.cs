using HotelApp.DTOs.Country;
using HotelApp.Interface.Repositories;
using HotelApp.Interface.Services;
using HotelApp.Models;

namespace HotelApp.Services
{
    public class CountryService : ICountryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CountryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CountryDetailsDto> CreateCountryAsync(string countryName)
        {
            var country = new Country
            {
                Name = countryName
            };
            await _unitOfWork.Countries.AddAsync(country);
            await _unitOfWork.SaveAsync();
            return new CountryDetailsDto
            {
                Id = country.Id,
                Name = country.Name
            };

        }

        public async Task<List<CountryDetailsDto>> GetCountriesAsync()
        {
            var countries = await _unitOfWork.Countries.GetAllAsync();
            var countryDtos = countries.Select(c => new CountryDetailsDto
            {
                Id = c.Id,
                Name = c.Name
            }).ToList();
            return countryDtos;
        }
    }
}
