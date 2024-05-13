using Microsoft.AspNetCore.Mvc;

namespace Course.Models.Home.Model
{
    public class FormModel
    {
        [FromForm(Name = "user-name")]
        public String UserName { get; set; } = null!;

        [FromForm(Name = "user-email")]
        public String UserEmail { get; set; } = null!;
    }
}
