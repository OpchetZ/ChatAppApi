using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Chatroom
{
    public class UpdateChatroomDto
    {
        [Required]
        [MaxLength(30, ErrorMessage ="Room name cannot over 30 character")]
        public string Name { get; set; }
        [Required]
        public string Type { get; set; }  = "private";
    }
}