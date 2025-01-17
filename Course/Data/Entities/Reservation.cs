﻿using System.Text.Json.Serialization;

namespace Course.Data.Entities
{
    public class Reservation
    {
        public Guid Id { get; set; }
        public Guid RoomId { get; set; }
        public Guid UserId { get; set; }
        public DateTime Date {  get; set; }
        public Double Price { get; set; }
        public DateTime OrderDt { get; set; }
        public DateTime? DeletedDt { get; set; }

        [JsonIgnore] public User User { get; set; }
        [JsonIgnore] public Room Room { get; set; }  
    }
}
