using System;
using System.ComponentModel.DataAnnotations;

namespace webapi.Dto
{
    public class UpdateUserRequestDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Firstname { get; set; }

        [Required]
        [StringLength(50)]
        public string Lastname { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 8)]
        public string Username { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 8)]
        public string Password { get; set; }
    }
}
