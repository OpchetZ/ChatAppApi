using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Helpers;
using api.interfaces;
using api.Models;

namespace api.Repository
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ApplicationDBContext _context;
        public MessageRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public Task<Massages> CreateAsync(Massages massagesModel)
        {
            throw new NotImplementedException();
        }

        public Task<Massages?> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Massages>> GetAllAsync(MessageQuery messageQuery)
        {
            throw new NotImplementedException();
        }

        public Task<Massages?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Massages?> UpdateAsync(int id, Massages massagesModel)
        {
            throw new NotImplementedException();
        }
    }
}