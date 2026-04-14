using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Message; // ปรับให้ตรงกับ Namespace ของคุณ
using api.Extensions;
using api.interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/Message")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageRepository _messageRepo;
        private readonly IRoommemRepository _roommemRepo; 
        private readonly UserManager<AppUser> _userManager;

        public MessageController(
            IMessageRepository messageRepo, 
            IRoommemRepository roommemRepo, 
            UserManager<AppUser> userManager)
        {
            _messageRepo = messageRepo;
            _roommemRepo = roommemRepo;
            _userManager = userManager;
        }

        
        [HttpGet("{roomId:int}")]
        [Authorize]
        public async Task<IActionResult> GetMessagesInRoom([FromRoute] int roomId)
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            if (appUser == null) return Unauthorized();

            
            var userRooms = await _roommemRepo.GetUserRoomAsync(appUser.Id);
            if (!userRooms.Any(r => r.Id == roomId))
            {
                return StatusCode(403, "Forbidden: คุณไม่ได้เป็นสมาชิกของห้องแชทนี้");
            }

            
            var messages = await _messageRepo.GetMessagesByRoomIdAsync(roomId); 
            var messageDtos = messages.Select(m => m.ToMessageDto()).ToList();

            return Ok(messageDtos);
        }

       
        [HttpPost("{roomId:int}")]
        [Authorize]
        public async Task<IActionResult> SendMessage([FromRoute] int roomId, [FromBody] CreateMessageDto createDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            if (appUser == null) return Unauthorized();

           
            var userRooms = await _roommemRepo.GetUserRoomAsync(appUser.Id);
            if (!userRooms.Any(r => r.Id == roomId))
            {
                return StatusCode(403, "Forbidden: คุณไม่สามารถส่งข้อความเข้าห้องที่คุณไม่ได้เป็นสมาชิกได้");
            }

            
            var messageModel = createDto.ToMessageFromCreate(roomId, appUser.Id);

           
            await _messageRepo.CreateAsync(messageModel);

            
            return Ok(messageModel.ToMessageDto()); 
            
        }

        
        [HttpPut("{messageId:int}")]
        [Authorize]
        public async Task<IActionResult> EditMessage([FromRoute] int messageId, [FromBody] UpdateMessageDto updateDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            if (appUser == null) return Unauthorized();

            
            var existingMessage = await _messageRepo.GetByIdAsync(messageId);
            if (existingMessage == null) return NotFound("ไม่พบข้อความนี้");

            
            if (existingMessage.SenderId != appUser.Id)
            {
                return StatusCode(403, "Forbidden: คุณไม่สามารถแก้ไขข้อความของคนอื่นได้!");
            }

           
            var updatedModel = updateDto.ToMessageFromUpdate();

           
            var result = await _messageRepo.UpdateAsync(messageId, updatedModel);
            
            return Ok(result?.ToMessageDto());
        }

       
        [HttpDelete("{messageId:int}")]
        [Authorize]
        public async Task<IActionResult> DeleteMessage([FromRoute] int messageId)
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            if (appUser == null) return Unauthorized();

           
            var existingMessage = await _messageRepo.GetByIdAsync(messageId);
            if (existingMessage == null) return NotFound("ไม่พบข้อความนี้");

           
            if (existingMessage.SenderId != appUser.Id)
            {
                return StatusCode(403, "Forbidden: คุณไม่สามารถลบข้อความของคนอื่นได้!");
            }

            
            await _messageRepo.DeleteAsync(messageId);

            return NoContent(); 
        }
    }
}