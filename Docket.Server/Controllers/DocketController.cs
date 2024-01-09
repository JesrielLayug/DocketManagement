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

        public DocketController(IDocketService docketService)
        {
            this.docketService = docketService;
        }

        [HttpGet("GetAll")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Models.Docket>>> GetAll()
        {
            try
            {
                var dockets = await docketService.GetAll();
                return Ok(dockets);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetById/{docketId}")]
        public async Task<ActionResult<Models.Docket>> GetById([FromRoute] string docketId)
        {
            try
            {
                var docket = await docketService.GetById(docketId);
                if (docket != null)
                {
                    return Ok(docket);
                }
                return NotFound("Docket does not exist");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetByUser/{userId}")]
        public async Task<ActionResult<IEnumerable<Models.Docket>>> GetByUserId([FromRoute] string userId)
        {
            try
            {
                var dockets = await docketService.GetByUserId(userId);
                if (dockets != null)
                    return Ok(dockets);

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
                    Color = request.Color,
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
                        Color = request.Color,
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
