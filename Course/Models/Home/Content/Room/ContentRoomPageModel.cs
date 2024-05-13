
namespace Course.Models.Home.Content.Room
{
    public class ContentRoomPageModel
    {
        public Course.Data.Entities.Room Room { get; set; } = null!;
        public int Year { get; set; }
        public int Month { get; set; }
        public int? Day { get; set; }
    }
}
