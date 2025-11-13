using HotelAPI.Data;
using HotelAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json.Serialization;

namespace HotelAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly HotelDbContext _context;

        public BookingController(HotelDbContext context)
        {
            _context = context;
        }

        // POST: api/booking
        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> CreateBooking([FromBody] Booking booking)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                return Unauthorized("Invalid user ID.");

            booking.UserId = userId;
            booking.Status = "Pending";

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return Ok(booking);
        }

        // GET: api/booking/my
        [HttpGet("my")]
        [Authorize(Roles = "User,Admin,Manager")]
        public async Task<IActionResult> GetMyBookings()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                return Unauthorized("Invalid user ID.");

            var bookings = await _context.Bookings
                .Include(b => b.Room)
                .Where(b => b.UserId == userId)
                .ToListAsync();

            return Ok(bookings);
        }

        // GET: api/booking/all
        [HttpGet("all")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> GetAllBookings()
        {
            var bookings = await _context.Bookings
                .Include(b => b.Room)
                .Include(b => b.User)
                .ToListAsync();

            return Ok(bookings);
        }

        // DELETE: api/booking/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "User,Admin,Manager")]
        public async Task<IActionResult> CancelBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound("Booking not found.");

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                return Unauthorized("Invalid user ID.");

            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (booking.UserId != userId && userRole != "Admin" && userRole != "Manager")
                return Forbid();

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return Ok("Booking cancelled successfully.");
        }

        // PUT: api/booking/update-status/{id}
        [HttpPut("update-status/{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> UpdateBookingStatus(int id, [FromBody] string status)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound("Booking not found.");

            var validStatuses = new[] { "Pending", "Confirmed", "Cancelled" };
            if (!validStatuses.Contains(status))
                return BadRequest("Invalid status value.");

            booking.Status = status;
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Status updated to {status}" });
        }
    }
}
