using System;

namespace webapi.Entities
{
    public record User
    {
        public Guid Id { get; init; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}