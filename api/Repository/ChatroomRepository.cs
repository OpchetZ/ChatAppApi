using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Helpers;
using api.interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class ChatroomRepository : IChatroomRepository
    {
        private readonly ApplicationDBContext _context;
        public ChatroomRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<Chatroom> CreateAsync(Chatroom chatroomModel)
        {
            await _context.Chatrooms.AddAsync(chatroomModel);
            await _context.SaveChangesAsync();
            return chatroomModel;
        }

        public async Task<Chatroom?> DeleteAsync(int id)
        {
            var chatroomModel = await _context.Chatrooms.FirstOrDefaultAsync(x => x.Id == id);
            if(chatroomModel == null)
            {
                return null;
            }
            _context.Chatrooms.Remove(chatroomModel);
            await _context.SaveChangesAsync();
            return chatroomModel;
        }

        public async Task<List<Chatroom>> GetAllAsync(QueryObject query)
        {
            var Chatroom = _context.Chatrooms.Include(c => c.Massages).ThenInclude(a => a.AppUser).AsQueryable();
            if (!string.IsNullOrWhiteSpace(query.keyword))
            {
                Chatroom = Chatroom.Where(s => s.Name.Contains(query.keyword));
            }
            if (query.IsDecsending == true)
            {
                Chatroom = Chatroom.OrderByDescending(s => s.Name);
            }
            var skipnum = (query.PageNumber - 1) * query.PageSize;
            return await Chatroom.Skip(skipnum).Take(query.PageSize).ToListAsync();
        }

        public async Task<Chatroom?> GetByIdAsync(int id)
        {
            return await _context.Chatrooms.Include(c => c.Massages).FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Chatroom?> UpdateAsync(int id, Chatroom chatroomModel)
        {
            var exist = await _context.Chatrooms.FirstOrDefaultAsync(x => x.Id == id);
            if(exist == null)
            {
                return null;
            }
            exist.Name = chatroomModel.Name;
            exist.Type = chatroomModel.Type;

            await _context.SaveChangesAsync();
            return exist;
        }
    }
}