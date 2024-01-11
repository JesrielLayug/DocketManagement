using Docket.Server.Extensions;
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
    public class DocketController : ControllerBase
    {
        private readonly IDocketService docketService;
        private readonly IUserService userService;

        public DocketController(IDocketService docketService, IUserService userService)
        {
            this.docketService = docketService;
            this.userService = userService;
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
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

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

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] DTODocket request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await docketService.Add(new Models.Docket
                {
                    Title = request.Title,
                    Body = request.Body,
                    DateCreated = request.DateCreated,
                    DateModified = request.DateModified,
                    UserId = request.UserId,
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
                    await docketService.Update(docketId, new Models.Docket
                    {
                        Title = request.Title,
                        Body = request.Body,
                        DateCreated = request.DateCreated,
                        DateModified = request.DateModified,
                        UserId = request.UserId,
                        IsPublic = request.IsPublic,
                        IsHidden = request.IsHidden
                    });
                    return Ok();
                }

                return NotFound($"Dcoket with id of {docketId} does not exist.");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

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
