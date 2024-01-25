using Docket.Server.Models;
using Docket.Server.Services.Contracts;
using Docket.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Docket.Server.Extensions;

namespace Docket.Server.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RateController : ControllerBase
    {
        private readonly IDocketRateService rateService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public RateController
            (
                IDocketRateService rateService,
                IHttpContextAccessor httpContextAccessor
            )
        {
            this.rateService = rateService;
            this.httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("GetExisting/{docketId}")]
        public async Task<ActionResult<DTOFeatureRate>> GetExisting(string docketId)
        {
            try
            {
                if(ModelState.IsValid && httpContextAccessor.HttpContext != null)
                {
                    var user = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

                    var rate = await rateService.GetExisting(user, docketId);

                    if(rate == null)
                    {
                        return Ok(new DTOFeatureRate
                        {
                            Rate = 0,
                            DocketId = docketId
                        });
                    }

                    return Ok(new DTOFeatureRate
                    {
                        Rate = rate.Rating,
                        DocketId = rate.DocketId
                    });
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<DTOFeatureRate>>> GetAll()
        {
            try
            {
                if (ModelState.IsValid && httpContextAccessor.HttpContext != null)
                {
                    var user = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

                    var rates = await rateService.GetByUserId(user);

                    return Ok(rates);
                }
                return NotFound("No rated dockets found");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AddRate")]
        public async Task<IActionResult> AddRate([FromBody] DTOFeatureRate request)
        {
            try
            {
                if (ModelState.IsValid && httpContextAccessor.HttpContext != null)
                {
                    var user = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

                    var userRated = await rateService.GetExisting(user, request.DocketId);

                    if (userRated != null)
                    {

                        userRated.Rating = request.Rate;

                        await rateService.Update(userRated);
                    }
                    else
                    {
                        await rateService.Add(new Rate
                        {
                            DocketId = request.DocketId,
                            Rating = request.Rate,
                            UserId = user
                        });
                    }

                    return Ok("Successfully rated the docket");
                }
                return BadRequest("Failed to rate the docket");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }

    }
}
