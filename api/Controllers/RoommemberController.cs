using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Chatroom;
using api.Extensions;
using api.interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;



namespace api.Controllers
{
    [Route("api/Roommember")]
    [ApiController]
    public class RoommemberController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IChatroomRepository _chatroom;
        private readonly IRoommemRepository _roommem;
        public RoommemberController(UserManager<AppUser> userManager,
        IChatroomRepository chatroomRepo, IRoommemRepository roommemRepo)
        {
            _userManager = userManager;
            _chatroom = chatroomRepo;
            _roommem = roommemRepo;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserRoomAsync()
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);

            if (appUser == null) return Unauthorized("User not found.");

            // ส่ง ID ไปดึงข้อมูลจาก Repo อย่างปลอดภัย
            var userRooms = await _roommem.GetUserRoomAsync(appUser.Id);



            var roomDtos = userRooms.Select(room => new ChatroomDto
            {
                Id = room.Id,
                Name = room.Name,
                Type = room.Type
            }).ToList();

            return Ok(roomDtos);
        }
        [HttpPost("{roodId}")]
        [Authorize]
        public async Task<IActionResult> JoinRoomAsync([FromRoute] int roomId)
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            if (appUser == null) return Unauthorized();

            var roomMemberModel = new RoomMember
            {
                AppUserId = appUser.Id,
                RoomId = roomId
            };
            await _roommem.CreateAsync(roomMemberModel);


            return StatusCode(201, new { message = "เข้าร่วมห้องแชทสำเร็จ!" });
        }
        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> LeaveRoomAsync([FromRoute] int roomId)
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            if (appUser == null) return Unauthorized();

           
            var deletedMember = await _roommem.DeleteRoomMem(appUser.Id, roomId);

            
            if (deletedMember == null)
            {
                return NotFound("คุณไม่ได้เป็นสมาชิกของห้องนี้ หรือไม่พบห้องแชท");
            }

            
            return Ok(new { message = "คุณออกจากห้องแชทเรียบร้อยแล้ว" });
        }
    }
}