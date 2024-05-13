using System.Data;

namespace Course.Data.Entities
{
    public class Location
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public Guid? CountryId { get; set; }
        public Guid? CityId { get; set; }
        public string? Adress { get; set; } = null!;
        public string? Stars { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime? DeletedDT { get; set; }
        public String? PhotoUrl { get; set; }
        public String? Slug { get; set; }
    }
}
