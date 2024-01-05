﻿using Docket.Server.Models;
using Docket.Server.Services.Contracts;
using Docket.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Docket.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService authenticationService;
        private readonly IUserService userService;

        public AuthenticationController(IAuthenticationService authenticationService, IUserService userService)
        {
            this.authenticationService = authenticationService;
            this.userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] DTOUser request)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    authenticationService.CreatePasswordHash(request.password, out byte[] hash, out byte[] salt);

                    var newUser = new User
                    {
                        name = request.name,
                        gender = request.gender,
                        age = request.age,
                        PasswordHash = hash,
                        PasswordSalt = salt
                    };

                    await authenticationService.Register(newUser);
                    return Ok(newUser);
                }
                return BadRequest(ModelState);
            }
            catch(Exception ex)
            {
                return BadRequest($"Failed to register {ex.StackTrace}");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(string username, string password)
        {
            try
            {
                var existingUser = await userService.GetByName(username);
                if (existingUser.name != username)
                {
                    return NotFound("Username does not exits.");
                }

                var isUserPasswordCorrect = authenticationService.VerifyPasswordHash(password, existingUser.PasswordHash, existingUser.PasswordSalt);
                if(isUserPasswordCorrect)
                {
                    string token = authenticationService.CreateToken(existingUser);
                    return Ok(token);
                }
                return BadRequest("Incorrect password");
                
            }
            catch(Exception ex)
            {
                return BadRequest($"Failed to register {ex.StackTrace}");
            }
        }
    }
}