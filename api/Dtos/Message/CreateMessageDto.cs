using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Message
{
    public class CreateMessageDto
    {
        [Required]
        [MaxLength(250, ErrorMessage ="Cannot be over 250 char")]
        public string Content { get; set; } =string.Empty;
    }
}