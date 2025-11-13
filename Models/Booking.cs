using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelAPI.Models
{
    public class Booking
    {
        public int Id { get; set; }

        public int RoomId { get; set; }
        public Room? Room { get; set; } // <-- nullable

        public int UserId { get; set; }
        public User? User { get; set; } // <-- nullable

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string Status { get; set; } = "Pending";
    }

}
