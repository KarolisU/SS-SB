using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SSSB.Auth;
using SSSB.Auth.Model;
using SSSB.Data.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSSB.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<SSSBUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ITokenManager _tokenManager;
        public AuthController(UserManager<SSSBUser> userManager, IMapper mapper, ITokenManager tokenManager)
        {
            _userManager = userManager;
            _mapper = mapper;
            _tokenManager = tokenManager;
        }
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterUserDto registerUserDto)
        {
            var user = await _userManager.FindByNameAsync(registerUserDto.UserName);
            if (user != null)
                return BadRequest("Request invalid");
            var newUser = new SSSBUser
            {
                Email = registerUserDto.Email,
                UserName = registerUserDto.UserName
            };
            var createUserResult = await _userManager.CreateAsync(newUser, registerUserDto.Password);
            if (!createUserResult.Succeeded)
                return BadRequest("Could not create a user.");
            await _userManager.AddToRoleAsync(newUser, SSSBUserRoles.RegisteredUser);
            return CreatedAtAction(nameof(Register), _mapper.Map<UserDto>(newUser));
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.UserName);
            if (user == null)
                return BadRequest("User cianame or password is invalid.");

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!isPasswordValid)
                return BadRequest("User  password is invalid.");
            var accessToken = await _tokenManager.CreateAccessTokenAsync(user);
            return Ok(new SuccessfulLoginResponseDto(accessToken));
        }
    }
}
