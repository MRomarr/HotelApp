
namespace HotelApp.Models
{
    public class City
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string CountryId { get; set; }
        public Country country { get; set; }

        public List<Hotel>? hotels { get; set; }
    }
}