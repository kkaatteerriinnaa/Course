using System.Data;

namespace Course.Data.Entities
{
    public class Room
    {
        public Guid Id { get; set; }
        public Guid LocationId { get; set; }
        public string? Stars { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime? DeletedDT { get; set; }
        public Double DailyPrice { get; set; }
        public String? PhotoUrl { get; set; }
        public String? Slug { get; set; }
        public List<Reservation> Reservations { get; set; } 
    }
}
