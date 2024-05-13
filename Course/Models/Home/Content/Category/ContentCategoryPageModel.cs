namespace Course.Models.Home.Content.Category
{
    public class ContentCategoryPageModel
    {
        public Course.Data.Entities.Category Category { get; set; } = null!;
        public List<Course.Data.Entities.Location> Locations { get; set; } = [];
    }
}
