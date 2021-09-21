using System;

namespace webapi.Dto
{
    public class UpdateUserResponseDto
    {
        public Guid Id { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }
    }
}
