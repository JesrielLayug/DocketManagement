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
                    return Ok(new Response
                    {
                        isSuccess = true,
                        message = "Successfully register user.",
                        statusCode = System.Net.HttpStatusCode.OK
                    });
                }
                return BadRequest(new Response
                {
                    isSuccess = false,
                    message = ModelState.ToString(),
                    statusCode = System.Net.HttpStatusCode.BadRequest
                });
            }
            catch(Exception ex)
            {
                return BadRequest(new Response
                {
                    isSuccess = false,
                    message = ex.StackTrace,
                    statusCode = System.Net.HttpStatusCode.BadRequest
                });
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
                    return BadRequest(new Response
                    {
                        isSuccess = false,
                        message = "User does not exist",
                        statusCode = System.Net.HttpStatusCode.NotFound
                    });
                }

                var isUserPasswordCorrect = authenticationService.VerifyPasswordHash(request.password, existingUser.PasswordHash, existingUser.PasswordSalt);
                if(isUserPasswordCorrect)
                {
                    string token = authenticationService.CreateToken(existingUser);
                    return Ok(new Response
                    {
                        isSuccess = true,
                        message = "Successfully logged in.",
                        statusCode = System.Net.HttpStatusCode.OK,
                        token = token
                    });
                }
                return BadRequest(new Response
                {
                    isSuccess = false,
                    message = "Incorrect password",
                    statusCode = System.Net.HttpStatusCode.BadRequest
                });
                
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Exception details: {ex}");
                return BadRequest(new Response
                {
                    isSuccess = false,
                    message = ex.StackTrace,
                    statusCode = System.Net.HttpStatusCode.BadRequest
                });
            }
        }
    }
}
