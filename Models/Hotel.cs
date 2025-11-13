using System.Collections.Generic;

namespace HotelAPI.Models
{
    public class Hotel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;

        // Veza sa sobama
        public ICollection<Room> Rooms { get; set; } = new List<Room>();
    }
}
