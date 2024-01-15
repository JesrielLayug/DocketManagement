using Docket.Server.Extensions;
using Docket.Server.Models;
using Docket.Server.Services.Contracts;
using Docket.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Docket.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocketController : ControllerBase
    {
        private readonly IDocketService docketService;
        private readonly IUserService userService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public DocketController(IDocketService docketService, IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            this.docketService = docketService;
            this.userService = userService;
            this.httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("GetAll")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<DTODocket>>> GetAll()
        {
            try
            {
                var dockets = await docketService.GetAll();

                var users = await userService.GetAll();

                var dto_dockets = dockets.Convert(users);


                return Ok(dto_dockets);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAllPublic")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<DTODocket>>> GetAllPublics()
        {
            try
            {
                var public_dockets = await docketService.GetAllPublic();

                var users = await userService.GetAll();

                var dto_public_dockets = public_dockets.Convert(users);

                return Ok(dto_public_dockets);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("GetById/{docketId}")]
        public async Task<ActionResult<DTODocket>> GetById([FromRoute] string docketId)
        {
            try
            {
                var docket = await docketService.GetById(docketId);
                if (docket != null)
                {
                    return Ok(new DTODocket
                    {
                        Id = docket.Id,
                        Title = docket.Title,
                        Body = docket.Body,
                        DateCreated = docket.DateCreated,
                        DateModified = docket.DateModified,
                        IsHidden = docket.IsHidden,
                        IsPublic = docket.IsPublic,
                        UserId = docket.UserId,
                    });
                }
                return NotFound("Docket does not exist");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("GetByUser/{userId}")]
        public async Task<ActionResult<IEnumerable<DTODocket>>> GetByUserId([FromRoute] string userId)
        {
            try
            {
                var dockets = await docketService.GetByUserId(userId);
                if (dockets != null)
                {

                    var users = await userService.GetAll();

                    var dto_dockets = dockets.Convert(users);

                    return Ok(dto_dockets);
                }

                return NotFound("User does not have any dockets yet");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("GetUserDockets")]
        public async Task<ActionResult<IEnumerable<DTODocket>>> GetUserDockets()
        {
            try
            {
                var userId = string.Empty;
                if (httpContextAccessor.HttpContext != null)
                {
                    userId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                }

                var dtoDockets = new List<DTODocket>();
                var domainDockets = await docketService.GetByUserId(userId);

                foreach (var item in domainDockets)
                {
                    dtoDockets.Add(new DTODocket
                    {
                        Id = item.Id,
                        Title = item.Title,
                        Body = item.Body,
                        DateCreated = item.DateCreated,
                        DateModified = item.DateModified,
                        UserId = item.UserId,
                        IsHidden = item.IsHidden,
                        IsPublic = item.IsPublic,
                        Username = httpContextAccessor.HttpContext.User.Identity.Name
                    });
                }

                return Ok(dtoDockets);
            }
            catch(HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] DTODocket request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userId = string.Empty;

                if (httpContextAccessor.HttpContext != null)
                {
                    userId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                }

                await docketService.Add(new Models.Docket
                {
                    Title = request.Title,
                    Body = request.Body,
                    DateCreated = request.DateCreated,
                    DateModified = request.DateModified,
                    UserId = userId,
                    IsPublic = request.IsPublic,
                    IsHidden = request.IsHidden
                });
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut("Update/{docketId}")]
        public async Task<IActionResult> Update(string docketId, [FromBody] DTODocket request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var existingDocket = await docketService.GetById(docketId);
                if(existingDocket != null)
                {
                    existingDocket.Title = request.Title;
                    existingDocket.Body = request.Body;
                    existingDocket.DateCreated = request.DateCreated;
                    existingDocket.DateModified = request.DateModified;
                    existingDocket.UserId = request.UserId;
                    existingDocket.IsPublic = request.IsPublic;
                    existingDocket.IsHidden = request.IsHidden;

                    await docketService.Update(docketId, existingDocket);
                    return Ok();
                }

                return NotFound($"Docket with id of {docketId} does not exist.");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("Delete/{docketId}")]
        public async Task<IActionResult> Delete([FromRoute] string docketId)
        {
            try
            {
                var existingDocket = await docketService.GetById(docketId);
                if(existingDocket != null)
                {
                    await docketService.Delete(docketId);
                    return Ok();
                }
                return NotFound($"Dcoket with id of {docketId} does not exist.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
