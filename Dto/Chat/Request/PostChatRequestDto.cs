using System.ComponentModel.DataAnnotations;

namespace webapi.Dto
{
    public class PostChatRequestDto
    {
        [Required]
        public string text { get; set; }
    }
}