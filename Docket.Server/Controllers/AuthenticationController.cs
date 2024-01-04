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

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] DTOUser user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await authenticationService.Register(new Models.User
                    {
                        name = user.name,
                        gender = user.gender,
                        age = user.age,
                    });
                    return Ok("Successfully register a user.");
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace);
            }
        }

        [HttpPost("login/{id}")]
        public async Task<IActionResult> Login(string id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingUser = await authenticationService.Login(id);
                    if(existingUser != null)
                    {
                        return Ok("Successfully login");
                    }
                    return NotFound("User is not registered yet.");
                }
                return BadRequest($"Failed to login {id}");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace);
            }
        }
    }
}
