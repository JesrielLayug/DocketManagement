using Docket.Server.Models;
using Docket.Server.Services.Contracts;
using Docket.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Docket.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var users = await userService.GetAll();
                return Ok(users);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.StackTrace);
            }
        }

        [HttpGet("GetById/{userId}")]
        public async Task<IActionResult> GetById([FromRoute] string userId) 
        {
            try
            {
                var user = await userService.GetById(userId);
                if(user != null)
                {
                    return Ok(user);
                }
                return NotFound("User does not exist");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.StackTrace);
            }

        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] DTOUser user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await userService.Create(new User
                {
                    name = user.name,
                    gender = user.gender,
                    age = user.age,
                });
                return Ok("Successfully created a user.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace);
            }
        }

        [HttpPut("Update/{userId}")]
        public async Task<IActionResult> Update(string userId, [FromBody] DTOUser user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var existingUser = await userService.GetById(userId);
                if (existingUser != null)
                {
                    await userService.Update(userId, new User
                    {
                        id = user.id,
                        name = user.name,
                        gender = user.gender,
                        age = user.age,
                    });
                    return Ok("Successfully updated user");
                }

                return NotFound($"User with id of {userId} does not exist.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace);
            }
        }

        [HttpDelete("Delete/{userId}")]
        public async Task<IActionResult> Delete(string userId)
        {
            try
            {
                var existingUser = await userService.GetById(userId);
                if(existingUser != null)
                {
                    await userService.Remove(userId);
                    return Ok("Successfully deleted user.");
                }
                return NotFound("User does not exist.");
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.StackTrace);
            }
        }
    }
}
