using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Message;
using api.Models;

namespace api.Mappers
{
    public static class MessageMapper
    {
        public static MessageDto ToMessageDto(this Massages massagesModel)
        {
            return new MessageDto
            {
              Id = massagesModel.Id,
              Content = massagesModel.Content,
              SentAt = massagesModel.SentAt,
              SenderId = massagesModel.SenderId,
              RoomId = massagesModel.RoomId
            };
        }
        public static Massages ToMessageFromCreate(this CreateMessageDto createMessageDto,int RoomId,string SenderId)
        {
            return new Massages
            {
              
              Content = createMessageDto.Content,
              RoomId = RoomId,
              SenderId = SenderId
            };
        }
        public static void ToMessageFromUpdate(this UpdateMessageDto updateMessageDto, Massages existmass)
        {
            
              
              existmass.Content = updateMessageDto.Content;
             
           
        }
        
    }
}