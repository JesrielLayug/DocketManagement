using Docket.Server.Models;
using Docket.Server.Services.Contracts;
using Docket.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        public async Task<ActionResult<Response>> Register([FromBody] DTOUserRegister request)
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
                        PasswordSalt = salt,
                        Role = "user"
                    };

                    await authenticationService.Register(newUser);
                    return Ok("Successfully register user.");
                }
                return BadRequest(ModelState);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.StackTrace);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromBody] DTOUserLogin request)
        {
            try
            {
                var existingUser = await userService.GetByName(request.username);
                if (existingUser == null)
                {
                    return BadRequest("User does not exist");
                }

                var isUserPasswordCorrect = authenticationService.VerifyPasswordHash(request.password, existingUser.PasswordHash, existingUser.PasswordSalt);
                if(isUserPasswordCorrect)
                {
                    string token = authenticationService.CreateToken(existingUser);
                    return Ok(token);
                }
                return BadRequest("Incorrect password");
                
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Exception details: {ex}");
                return BadRequest(ex.StackTrace);
            }
        }
    }
}
