namespace Course.Models.Home.Model
{
    public class PageModel
    {
        public String TabHeader { get; set; } = null!;
        public String PageTitle { get; set; } = null!;
        public FormModel? FormModel { get; set; }
    }
}
