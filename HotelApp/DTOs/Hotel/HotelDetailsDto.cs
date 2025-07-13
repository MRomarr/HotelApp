
namespace HotelApp.DTOs.Hotel
{
    public class HotelDetailsDto
    {
        public string Id { get; set; } 
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal PricePerDay { get; set; }
        public float AvrageReview { get; set; }
        public string CityId { get; set; }
        public string City { get; set; }
        public string CountryId { get; set; }
        public string Country { get; set; }
        public List<HotelPhotosDto> HotelPhotosDto { get; set; } = new List<HotelPhotosDto>();
        public List<IncludeDto> IncludesDto { get; set; } = new List<IncludeDto>();
    }
}
