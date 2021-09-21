using System.ComponentModel.DataAnnotations;

namespace webapi.Dto
{
    public class SignInRequestDto
    {
        [Required]
        [StringLength(20, MinimumLength = 1)]
        public string Username { get; set; }
        
        [Required]
        [StringLength(20, MinimumLength = 8)]
        public string Password { get; set; }
    }
}