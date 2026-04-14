using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Chatroom;
using api.Extensions;
using api.Helpers;
using api.interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/chatroom")]
    [ApiController]
    public class ChatroomController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly IChatroomRepository _chatRepo;
        private readonly IRoommemRepository _roommem;
        private readonly UserManager<AppUser> _user;
        public ChatroomController(ApplicationDBContext context, IChatroomRepository chatRepo, IRoommemRepository roommem,
        UserManager<AppUser> userManager)
        {
            _chatRepo = chatRepo;
            _user = userManager;
            _roommem = roommem;
            _context = context;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllAsync([FromQuery] QueryObject queryObject)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var chatrooms = await _chatRepo.GetAllAsync(queryObject);
            var chatroomDto = chatrooms.Select(s => s.ToChatroomDto()).ToList();

            return Ok(chatroomDto);
        }
        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var chatroom = await _chatRepo.GetByIdAsync(id);
            if (chatroom == null)
            {
                return NotFound();
            }
            return Ok(chatroom.ToChatroomDto());
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateChatroomDto createChatroom)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // 🌟 2. ดึงข้อมูลคนที่กำลัง Login (คนที่กดสร้างห้อง)
            var username = User.GetUsername(); // ใช้ Extension Method ที่เราเคยทำไว้
            var appUser = await _user.FindByNameAsync(username);

            if (appUser == null) return Unauthorized("ไม่พบผู้ใช้งาน");

            // 🌟 3. สั่งเซฟ "ห้องแชท" ลงฐานข้อมูลก่อน
            var chatroomModel = createChatroom.ToChatroomFromCreate();
            await _chatRepo.CreateAsync(chatroomModel);
            // พอรันบรรทัดนี้จบ EF Core จะเสก ID ห้องที่เพิ่งสร้างเสร็จกลับมาใส่ใน chatroomModel.Id ทันที

            // 🌟 4. สร้างความสัมพันธ์: ดึงคนสร้าง เข้าห้องแชท!
            var roomMember = new RoomMember
            {
                AppUserId = appUser.Id,     // ID คนกดสร้าง
                RoomId = chatroomModel.Id   // ID ห้องที่เพิ่งได้มาใหม่ๆ
            };
            await _roommem.CreateAsync(roomMember); // เซฟลงตารางสมาชิก

            return CreatedAtAction(nameof(GetById), new { id = chatroomModel.Id }, chatroomModel.ToChatroomDto());
        }
        [HttpPut]
        [Route("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateChatroomDto updateChatroomDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            //แปลง DTO ให้เป็น Model
            var chatroomModel = updateChatroomDto.ToChatroomFromUpdate();
            var result = await _chatRepo.UpdateAsync(id, chatroomModel);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result.ToChatroomDto());

        }
        [HttpDelete]
        [Route("{id:int}")]
        [Authorize]

        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var chatroomModel = await _chatRepo.DeleteAsync(id);

            if (chatroomModel == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}