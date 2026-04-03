using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Message
{
    public class MessageDto
    {
        public int Id { get; set; }
        public string Content { get; set; } =string.Empty;
        public DateTime SentAt { get; set; } = DateTime.Now;
        public string SenderId { get; set; } = string.Empty;
        public int RoomId { get; set; }
    }
}