using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Massages
    {
        public int Id { get; set; }
        public string Content { get; set; } =string.Empty;
        public DateTime SentAt { get; set; } = DateTime.Now;
        public string SenderId { get; set; }
        public AppUser AppUser { get; set; }
        public int RoomId { get; set; }
        public Chatroom Chatroom { get; set; }

    }
}