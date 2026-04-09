using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class RoommemRepository : IRoommemRepository
    {
        private readonly ApplicationDBContext _context;
        public RoommemRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<RoomMember> CreateAsync(RoomMember roomMember)
        {
            await _context.RoomMembers.AddAsync(roomMember);
            await _context.SaveChangesAsync();
            return roomMember;
        }

        public async Task<RoomMember?> DeleteRoomMem(string userId, int roomId)
        {
            var roomMember = await _context.RoomMembers.FirstOrDefaultAsync(x => x.AppUserId == userId && x.RoomId == roomId);
            if(roomMember == null)
            {
                return null;
            }
            _context.RoomMembers.Remove(roomMember);
            await _context.SaveChangesAsync();
            return roomMember;
        }

        public async Task<List<Chatroom>> GetUserRoomAsync(string userId)
        {
            return await _context.Chatrooms
        .Where(room => room.RoomMembers.Any(rm => rm.AppUserId == userId))
        .ToListAsync();
        }
    }
}