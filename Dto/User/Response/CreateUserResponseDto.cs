using System;

namespace webapi.Dto
{
    public class AddUserResponseDto
    {
        public Guid Id { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
