namespace HotelApp.Models
{
    public class Review
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public string HotelId { get; set; }
        public Hotel Hotel { get; set; }
    }
}
