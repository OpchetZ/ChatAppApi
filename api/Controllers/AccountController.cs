using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Account;
using api.interfaces;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _siginManager;
        public AccountController(UserManager<AppUser> userManager,ITokenService tokenService,
        SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _siginManager = signInManager;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginDto loginDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());

            if(user == null) return Unauthorized("Invalid username");

            var result = await _siginManager.CheckPasswordSignInAsync(user, loginDto.Password,false);
            if(!result.Succeeded) return Unauthorized("Username not found or Password incorrect");

            return Ok(
                new NewUserDto
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Token = _tokenService.CreateToken(user)
                }
            );
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto regisDto)
        {
            try
            {
                if(!ModelState.IsValid)
                    return BadRequest(ModelState);
                var appUser = new AppUser
                {
                    UserName = regisDto.Username,
                    Email  = regisDto.Email
                };
                var createUser = await _userManager.CreateAsync(appUser, regisDto.Password);
                if (createUser.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(appUser,"User");
                    if (roleResult.Succeeded)
                    {
                        return Ok(
                            new NewUserDto
                            {
                                UserName = appUser.UserName,
                                Email = appUser.Email,
                                Token = _tokenService.CreateToken(appUser)
                            }
                        );
                    }
                    else
                    {
                        return StatusCode(500, roleResult.Errors);
                    }
                }
                else
                {
                    return StatusCode(500, createUser.Errors);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500 ,e);
            }
        }
    }
}