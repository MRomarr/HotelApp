

namespace HotelApp.Models
{
    public class HotelPhoto
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string ImageUrl { get; set; }
       
        public string HotelId { get; set; }
        public Hotel hotel { get; set; }
    }
}