namespace HotelApp.DTOs.Hotel
{
    public class CreateHotelDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal PricePerDay { get; set; }
        public string CityId { get; set; } = string.Empty;

        public List<CreateIncludeDto>? IncludesIds { get; set; } = new List<CreateIncludeDto>();
        public IFormFileCollection HotelPhotos { get; set; }
    }
}
