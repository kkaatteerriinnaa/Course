namespace Course.Models.Home.Content.Location
{
    public class ContentLocationPageModel
    {
        public Course.Data.Entities.Location Location { get; set; } = null!;
        public List<Course.Data.Entities.Room> Rooms { get; set; } = [];
    }
}
