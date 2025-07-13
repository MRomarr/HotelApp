using HotelApp.DTOs.Hotel;

namespace HotelApp.Interface.Services
{
    public interface IHotelService
    {
        
        Task<List<HotelDetailsDto>> SearchHotelsAsync(string? Location, int? person, DateTime? checkIn, DateTime? checkOut);
        Task<List<HotelDetailsDto>> GetHotelsAsync();
        Task<HotelDetailsDto> GetHotelByIdAsync(string hotelId);
        Task<HotelDetailsDto> CreateHotelAsync(CreateHotelDto createHotelDto);
        
        
    }
}
