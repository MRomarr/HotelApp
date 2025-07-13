
namespace HotelApp.Models
{
    public class Hotel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal PricePerDay { get; set; }
        public float AvrageReview { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public string CityId { get; set; }
        public City city { get; set; }

        public List<HotelPhoto> HotelPhotos { get; set; } = new List<HotelPhoto>();

        public List<Booking> Bookings { get; set; } = new List<Booking>();

        public List<Hotelincludes>? includes { get; set; }= new List<Hotelincludes>();
    }
}
