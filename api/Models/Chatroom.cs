using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Chatroom
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; }
        public List<Massages> Massages { get; set; } = new List<Massages>();
        public List<RoomMember> RoomMembers { get; set; } = new List<RoomMember>();
    }
}