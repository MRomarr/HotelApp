using HotelApp.DTOs.Hotel;
using HotelApp.Interface.Services;
using HotelApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HotelApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly IHotelService _hotelService;
        private readonly UserManager<ApplicationUser> _userManager;
        public HotelController(IHotelService hotelService, UserManager<ApplicationUser> userManager)
        {
            _hotelService = hotelService;
            _userManager = userManager;
        }
        [HttpGet("search")]
        public async Task<IActionResult> Search
        (
            [FromQuery] string? Location,
            [FromQuery] int? person,
            [FromQuery] DateTime? checkIn,
            [FromQuery] DateTime? checkOut
        )
        {
            // Allow: both null, or both not null, but not only one of them set
            bool checkInSet = checkIn.HasValue;
            bool checkOutSet = checkOut.HasValue;
            if (checkInSet ^ checkOutSet)
            {
                return BadRequest("You must provide both checkIn and checkOut, or neither.");
            }

            var hotels = await _hotelService.SearchHotelsAsync(Location, person, checkIn, checkOut);
            return Ok(hotels);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllHotels()
        {
            var hotels = await _hotelService.GetHotelsAsync();
            return Ok(hotels);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetHotelById(string id)
        {
            var hotel = await _hotelService.GetHotelByIdAsync(id);
            if (hotel == null)
            {
                return NotFound($"Hotel with ID {id} not found.");
            }
            return Ok(hotel);
        }
        [HttpPost]
        public async Task<IActionResult> CreateHotel([FromForm] CreateHotelDto createHotelDto)
        {

            if (createHotelDto == null)
            {
                return BadRequest("Hotel data is null.");
            }
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
            {
                return NotFound("User Not Fount");
            }
            var createdHotel = await _hotelService.CreateHotelAsync(createHotelDto,user.Id);
            return Ok(createdHotel);
        }
        
    }
}
