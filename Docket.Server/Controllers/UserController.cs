using Docket.Server.Models;
using Docket.Server.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Docket.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] User user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await userService.Create(user);
                return Ok("Successfully created a user.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace);
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
