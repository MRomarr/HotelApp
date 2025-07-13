namespace HotelApp.Models
{
    public class Include
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string FlagUrl { get; set; }
    }
}
