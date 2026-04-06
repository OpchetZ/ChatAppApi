using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Helpers;
using api.Models;

namespace api.interfaces
{
    public interface IChatroomRepository
    {
        Task<List<Chatroom>> GetAllChatroom(QueryObject query);
        Task<Chatroom?> GetByIdAsync(int id);
        Task<Chatroom> CreateAsync(Chatroom chatroomModel);
        Task<Chatroom?> UpdateAsync(int id, Chatroom chatroomModel);
        Task<Chatroom?> DeleteAsync(int id);

    }
}