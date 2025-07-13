using Microsoft.AspNetCore.Identity;

namespace HotelApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string Address { get; set; }

        public List<Hotel>? hotels { get; set; }=new List<Hotel>();
    }
}
