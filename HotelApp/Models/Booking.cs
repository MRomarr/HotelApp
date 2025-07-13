namespace HotelApp.Models
{
    public class Booking
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime checkIn { get; set; }
        public DateTime checkOut { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public int MaxPerson { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public string HotelId { get; set; }
        public Hotel Hotel { get; set; }
    }
}
