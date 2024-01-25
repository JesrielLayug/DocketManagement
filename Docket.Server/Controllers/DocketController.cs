using Docket.Server.Extensions;
using Docket.Server.Models;
using Docket.Server.Services.Contracts;
using Docket.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Sockets;
using System.Security.Claims;

namespace Docket.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocketController : ControllerBase
    {
        private readonly IDocketService docketService;
        private readonly IUserService userService;
        private readonly IDocketRateService rateService;
        private readonly IDocketFavoriteService favoriteService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public DocketController
            (
                IDocketService docketService, 
                IUserService userService, 
                IDocketRateService rateService,
                IDocketFavoriteService favoriteService,
                IHttpContextAccessor httpContextAccessor
            )
        {
            this.docketService = docketService;
            this.userService = userService;
            this.rateService = rateService;
            this.favoriteService = favoriteService;
            this.httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("GetAll")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<DTODocket>>> GetAll()
        {
            try
            {
                var userId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

                var dockets = await docketService.GetAll();

                var users = await userService.GetAll();

                var rates = await rateService.GetAll();

                var favorites = await favoriteService.GetAll();

                var dtodockets = dockets.Convert(users, rates, favorites, userId);

                return Ok(dtodockets);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("GetAllPublic")]
        public async Task<ActionResult<IEnumerable<DTODocket>>> GetAllPublics()
        {
            try
            {
                var userId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

                var dockets = await docketService.GetAll();

                var users = await userService.GetAll();

                var rates = await rateService.GetAll();

                var favorites = await favoriteService.GetAll();

                var dtodockets = dockets.Convert(users, rates, favorites, userId);

                return Ok(dtodockets);
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

                    var rates = await rateService.GetAll();

                    var favorites = await favoriteService.GetAll();

                    var dtodockets = dockets.Convert(users, rates, favorites, userId);

                    return Ok(dtodockets);
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

                var userId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

                var dockets = await docketService.GetByUserId(userId);

                var users = await userService.GetAll();

                var rates = await rateService.GetAll();

                var favorites = await favoriteService.GetAll();

                var dtodockets = dockets.Convert(users, rates, favorites, userId);

                return Ok(dtodockets);
            }
            catch(HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] DTODocketCreate request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    Console.WriteLine(ModelState);
                    return BadRequest(ModelState);
                }

                var userId = string.Empty;

                if (httpContextAccessor.HttpContext != null)
                {
                    userId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                }

                var docketId = await docketService.Add(new Models.Docket
                {
                    Title = request.Title,
                    Body = request.Body,
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    UserId = userId,
                    IsPublic = request.IsPublic
                });

                await rateService.Add(new Rate
                {
                    DocketId = docketId,
                    Rating = 0,
                    UserId = userId
                });


                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut("Update/{docketId}")]
        public async Task<IActionResult> Update(string docketId, [FromBody] DTODocketUpdate request)
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
                    existingDocket.DateCreated = existingDocket.DateCreated;
                    existingDocket.DateModified = DateTime.Now;
                    existingDocket.UserId = existingDocket.UserId;
                    existingDocket.IsPublic = request.IsPublic;

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
