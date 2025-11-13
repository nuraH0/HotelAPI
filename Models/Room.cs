namespace HotelAPI.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string Number { get; set; } = string.Empty; // Broj sobe
        public string Type { get; set; } = string.Empty;   // Single, Double, Suite
        public decimal Price { get; set; }

        // Veza sa rezervacijama
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
