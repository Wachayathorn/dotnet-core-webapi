using System;

namespace webapi.Dto
{
    public class DeleteUserResponseDto
    {
        public Guid Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
