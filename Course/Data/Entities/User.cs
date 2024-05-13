using System.Data;
using System.Text.Json.Serialization;

namespace Course.Data.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public String Email { get; set; }
        public String? EmailComfirmCode { get; set; }
        public String? AvatarUrl { get; set; }
        [JsonIgnore] public String Salt { get; set; }
        [JsonIgnore] public String DrivedKey { get; set; }
        public DateTime? DeletedDt { get; set; }
        public DateTime? Birthdate { get; set; }
        public String? Role { get; set; }

        [JsonIgnore] public List<Reservation> Reservations { get; set; }

    }
}
