using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelApp.Models
{
    public class Hotelincludes
    {
        
        public int number { get; set; }
        public string IncludeId { get; set; }
        public Include Include { get; set; }
        public string HotelId { get; set; }
        public Hotel Hotel { get; set; }
    }
}
