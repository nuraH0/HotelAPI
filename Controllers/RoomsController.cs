using HotelAPI.Data;
using HotelAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomsController : ControllerBase
    {
        private readonly HotelDbContext _context;

        public RoomsController(HotelDbContext context)
        {
            _context = context;
        }

        // GET: api/rooms
        [HttpGet]
        public async Task<IActionResult> GetRooms()
        {
            var rooms = await _context.Rooms.ToListAsync();
            return Ok(rooms);
        }

        // POST: api/rooms
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddRoom([FromBody] Room room)
        {
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();
            return Ok(room);
        }

        // PUT: api/rooms/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateRoom(int id, [FromBody] Room updatedRoom)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null) return NotFound();

            room.Number = updatedRoom.Number;
            room.Type = updatedRoom.Type;
            room.Price = updatedRoom.Price;

            await _context.SaveChangesAsync();
            return Ok(room);
        }

        // DELETE: api/rooms/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null) return NotFound();

            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Room deleted" });
        }
    }
}
