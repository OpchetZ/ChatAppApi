using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.interfaces
{
    public interface IRoommemRepository
    {
        Task<List<Chatroom>> GetUserRoomAsync(string userId);
        Task<RoomMember> CreateAsync(RoomMember roomMember);
        Task<RoomMember?> DeleteRoomMem(string userId,int RoomId);
    }
}