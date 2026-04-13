using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Chatroom;
using api.Helpers;
using api.interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/chatroom")]
    [ApiController]
    public class ChatroomController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly IChatroomRepository _chatRepo;
        public ChatroomController(ApplicationDBContext context, IChatroomRepository chatRepo)
        {
            _chatRepo = chatRepo;
            _context = context;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllAsync([FromQuery] QueryObject queryObject)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var chatrooms = await _chatRepo.GetAllAsync(queryObject);
            var chatroomDto = chatrooms.Select(s => s.ToChatroomDto()).ToList();

            return Ok(chatroomDto);
        }
        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var chatroom = await _chatRepo.GetByIdAsync(id);
            if(chatroom == null)
            {
                return NotFound();
            }
            return Ok(chatroom.ToChatroomDto());
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateChatroomDto createChatroom)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var chatroomModel = createChatroom.ToChatroomFromCreate();
            await _chatRepo.CreateAsync(chatroomModel);
            return CreatedAtAction(nameof(GetById), new { id = chatroomModel.Id}, chatroomModel.ToChatroomDto());
        }
        [HttpPut]
        [Route("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Update([FromRoute] int id,[FromBody] UpdateChatroomDto updateChatroomDto)
        {
            if(!ModelState.IsValid)
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
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var chatroomModel = await _chatRepo.DeleteAsync(id);

            if(chatroomModel == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}