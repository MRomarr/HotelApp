namespace HotelApp.DTOs.Auth
{
    public class AuthDto
    {
        public string Message { get; set; }
        public string Token { get; set; }
        public DateTime TokenExpiration { get; set; }
        public List<string> Role { get; set; }
        public bool IsAuthenticated { get; set; }
    }
}
