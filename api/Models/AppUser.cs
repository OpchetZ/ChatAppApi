using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace api.Models
{
    public class AppUser : IdentityUser
    {
        public List<RoomMember> RoomMembers { get; set; } = new List<RoomMember>();
        public List<Massages> Massages { get; set; } = new List<Massages>();
    }
}