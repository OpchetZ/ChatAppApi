using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class RoomMember
    {
        public string AppUserId { get; set; }
        public int RoomId { get; set; }
        public AppUser AppUser { get; set; }
        public Chatroom Chatroom { get; set; }
    }
}