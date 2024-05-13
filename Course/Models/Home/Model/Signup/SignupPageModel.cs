namespace Course.Models.Home.Model.Singnup
{
    public class SignupPageModel
    {
        public SignupFormModel? FormModel { get; set; }

        public Dictionary<String, String>? ValidationErrors { get; set; }
    }
}
