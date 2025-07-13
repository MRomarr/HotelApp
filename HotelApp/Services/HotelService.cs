using HotelApp.DTOs.Hotel;
using HotelApp.Interface.Repositories;
using HotelApp.Interface.Services;
using HotelApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HotelApp.Services
{
    public class HotelService : IHotelService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageServece _imageServece;
        
        public HotelService(IUnitOfWork UnitOfWork, IImageServece imageServece)
        {
            _unitOfWork = UnitOfWork;
            _imageServece = imageServece;
        }

        public async Task<List<HotelDetailsDto>> GetHotelsAsync()
        {
            var hotels = await _unitOfWork.Hotels.GetAllAsync();
            var hotelDtos = hotels.Select(h => new HotelDetailsDto
            {
                Id = h.Id,
                Name = h.Name,
                Description = h.Description,
                PricePerDay = h.PricePerDay,
                CityId = h.CityId,
                City = h.city.Name,
                CountryId = h.city.CountryId,
                Country = h.city.country.Name,
                AvrageReview = h.AvrageReview,
                IncludesDto =
                    h.includes.Select(i => new IncludeDto
                    {
                        Id = i.IncludeId, 
                        Name = i.Include.Name, 
                        FlagUrl = i.Include.FlagUrl 
                    }).ToList(),
                HotelPhotosDto = h.HotelPhotos.Select(p => new HotelPhotosDto
                {
                    Id = p.Id,
                    ImageUrl = p.ImageUrl,
                }).ToList(),
            }).ToList();
            return hotelDtos;
        }
        public async Task<HotelDetailsDto>? GetHotelByIdAsync(string hotelId)
        {
            var hotel = await _unitOfWork.Hotels.GetByIdAsync(hotelId);
            if (hotel is not null)
            {
                return null;
            }
            var hotelDto = new HotelDetailsDto
            {
                Id = hotel.Id,
                Name = hotel.Name,
                Description = hotel.Description,
                PricePerDay = hotel.PricePerDay,
                CityId = hotel.CityId,
                City = hotel.city?.Name,
                CountryId = hotel.city?.CountryId,
                Country = hotel.city?.country?.Name,
                AvrageReview = hotel.AvrageReview,
                IncludesDto = hotel.includes?.Select(i => new IncludeDto
                {
                    Id = i.IncludeId,
                    Name = i.Include?.Name,
                    FlagUrl = i.Include?.FlagUrl
                }).ToList() ?? new List<IncludeDto>(),
                HotelPhotosDto = hotel.HotelPhotos?.Select(p => new HotelPhotosDto
                {
                    Id = p.Id,
                    ImageUrl = p.ImageUrl,
                }).ToList() ?? new List<HotelPhotosDto>(),
            };
            return hotelDto;
        }
        public async Task<HotelDetailsDto> CreateHotelAsync(CreateHotelDto createHotelDto)
        {
            var hotelPhotoUrls = await _imageServece.AddImagesAsync(createHotelDto.HotelPhotos, createHotelDto.Name);
            var hotel = new Hotel
            {
                UserId = "a6e4f09b-bc33-47fe-9855-a49881f3100f",
                Name = createHotelDto.Name,
                Description = createHotelDto.Description,
                PricePerDay = createHotelDto.PricePerDay,
                CityId = createHotelDto.CityId,
                HotelPhotos = hotelPhotoUrls.Select(url => new HotelPhoto
                {
                    ImageUrl = url
                }).ToList(),
            };
            await _unitOfWork.Hotels.AddAsync(hotel);
            await _unitOfWork.SaveAsync();

            // Add includes for the saved hotel
            hotel.includes = createHotelDto.IncludesIds.Select(i => new Hotelincludes
            {
                IncludeId = i.IncludeId,
                number = i.Number,
                HotelId = hotel.Id
            }).ToList();

            _unitOfWork.Hotels.Update(hotel);
            await _unitOfWork.SaveAsync();

            var savedHotel = await _unitOfWork.Hotels.GetByIdAsync(hotel.Id);

            return new HotelDetailsDto
            {
                Id = savedHotel.Id,
                Name = savedHotel.Name,
                Description = savedHotel.Description,
                PricePerDay = savedHotel.PricePerDay,
                CityId = savedHotel.CityId,
                City = savedHotel.city?.Name,
                CountryId = savedHotel.city?.CountryId,
                Country = savedHotel.city?.country?.Name,
                AvrageReview = savedHotel.AvrageReview,
                IncludesDto = savedHotel.includes?.Select(i => new IncludeDto
                {
                    Id = i.IncludeId,
                    Name = i.Include?.Name,
                    FlagUrl = i.Include?.FlagUrl
                }).ToList() ?? new List<IncludeDto>(),
                HotelPhotosDto = savedHotel.HotelPhotos?.Select(p => new HotelPhotosDto
                {
                    Id = p.Id,
                    ImageUrl = p.ImageUrl,
                }).ToList() ?? new List<HotelPhotosDto>(),
            };
        }

        public async Task<List<HotelDetailsDto>> SearchHotelsAsync(string? Location, int? person, DateTime? checkIn, DateTime? checkOut)
        {
            var hotels = await _unitOfWork.Hotels.GetAllAsync();
            if (Location is not null)
            {
                hotels = hotels.Where(h => h.city.country.Name.Contains(Location, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            if (checkIn.HasValue && checkOut.HasValue)
            {
                hotels = hotels.Where(h => h.Bookings == null || !h.Bookings.Any(b =>
                    (b.checkIn < checkOut.Value && b.checkOut > checkIn.Value))).ToList();
            }
            if (person.HasValue)
            {
                hotels = hotels.Where(h => h.Bookings == null || h.Bookings.Count >= person.Value).ToList();
            }
            

            var hotelDtos = hotels.Select(h => new HotelDetailsDto
            {
                Id = h.Id,
                Name = h.Name,
                Description = h.Description,
                PricePerDay = h.PricePerDay,
                CityId = h.CityId,
                City = h.city?.Name,
                CountryId = h.city?.CountryId,
                Country = h.city?.country?.Name,
                AvrageReview = h.AvrageReview,
                IncludesDto = h.includes?.Select(i => new IncludeDto
                {
                    Id = i.IncludeId,
                    Name = i.Include?.Name,
                    FlagUrl = i.Include?.FlagUrl
                }).ToList() ?? new List<IncludeDto>(),
                HotelPhotosDto = h.HotelPhotos?.Select(p => new HotelPhotosDto
                {
                    Id = p.Id,
                    ImageUrl = p.ImageUrl,
                }).ToList() ?? new List<HotelPhotosDto>(),
            }).ToList();

            return hotelDtos;
        }
    }
}
