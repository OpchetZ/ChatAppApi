using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Helpers;
using api.Models;

namespace api.interfaces
{
    public interface IMessageRepository
    {
        Task<List<Massages>> GetAllAsync(MessageQuery messageQuery);
        Task<Massages?> GetByIdAsync(int id);
        Task<Massages> CreateAsync(Massages massagesModel);
        Task<Massages?> UpdateAsync(int id, Massages massagesModel);
        Task<Massages?> DeleteAsync(int id);
    }
}