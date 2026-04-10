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
    public class MessageRepository : IMessageRepository
    {
        private readonly ApplicationDBContext _context;
        public MessageRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<Massages> CreateAsync(Massages massagesModel)
        {
            await _context.Massages.AddAsync(massagesModel);
            await _context.SaveChangesAsync();
            return massagesModel;
        }

        public async Task<Massages?> DeleteAsync(int id)
        {
            var messagemodel = await _context.Massages.FirstOrDefaultAsync(x => x.Id ==id);
            if (messagemodel == null)
            {
                return null;
            }
            _context.Massages.Remove(messagemodel);
            await _context.SaveChangesAsync();
            return messagemodel;
        }

        public async Task<List<Massages>> GetAllAsync(MessageQuery messageQuery)
        {
            var message = _context.Massages.Include(a => a.AppUser).AsQueryable();
            if (!string.IsNullOrWhiteSpace(messageQuery.keyword))
            {
                message = message.Where(s => s.Content == messageQuery.keyword);
            }
            return await message.ToListAsync();
        }

        public async Task<Massages?> GetByIdAsync(int id)
        {
            return await _context.Massages.Include(a => a.AppUser).FirstOrDefaultAsync(c => c.Id ==id);
        }

        public async Task<Massages?> UpdateAsync(int id, Massages massagesModel)
        {
            var checkmessage = await _context.Massages.FindAsync(id);
            if (checkmessage == null)
            {
                return null;
            }

            checkmessage.Content = massagesModel.Content;
            await _context.SaveChangesAsync();

            return checkmessage;
        }
    }
}