using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Chatroom;
using api.Models;

namespace api.Mappers
{
    public static class ChatroomMapper
    {
        public static ChatroomDto ToChatroomDto(this Chatroom roommodel)
        {
            return new ChatroomDto
            {
                Id = roommodel.Id,
                Name = roommodel.Name,
                Type = roommodel.Type
            };
        }
        public static Chatroom ToChatroomFromCreate(this CreateChatroomDto createChatroom)
        {
            return new Chatroom
            {
                Name = createChatroom.Name,
                Type = createChatroom.Type
            };
        }

        public static Chatroom ToChatroomFromUpdate(this UpdateChatroomDto updateChatroom)
        {
            return new Chatroom
            {
                Name = updateChatroom.Name,
                Type = updateChatroom.Type
            };
        }
    }
}