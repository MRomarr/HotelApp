
namespace HotelApp.Models
{
    public class Country
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }

        public List<City>? Cities { get; set; }
    }
}