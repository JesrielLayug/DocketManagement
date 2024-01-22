using Docket.Server.Models;
using Docket.Server.Services.Contracts;
using Docket.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Docket.Server.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DocketFeatureController : ControllerBase
    {
        private readonly IDocketFeatureService featureService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public DocketFeatureController(IDocketFeatureService featureService, IHttpContextAccessor httpContextAccessor)
        {
            this.featureService = featureService;
            this.httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("RateDocket/{docketId}")]
        public async Task<IActionResult> RateDocket([FromRoute] string docketId, [FromBody] DTOFeatureAddRate featureRate)
        {
            try
            {
                if (ModelState.IsValid && httpContextAccessor.HttpContext != null)
                {
                    await featureService.RateDocket(new DocketRate
                    {
                        Rate = featureRate.Rate,
                        DocketId = docketId,
                        UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)
                });

                    return Ok($"Docket rating is {featureRate.Rate}");
                }

                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("FavoriteDocket/{docketId}")]
        public async Task<IActionResult> AddDocketToFavorite([FromRoute] string docketId, [FromBody] DTOFeatureAddFavorite featureFavorite)
        {
            try
            {
                if (ModelState.IsValid && httpContextAccessor.HttpContext != null)
                {
                    await featureService.AddDocketToFavorite(new DocketFavorite
                    {
                        IsFavorite = featureFavorite.IsFavorite,
                        DocketId = docketId,
                        UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)
                    });

                    return Ok("Successfully added the docket to favorites");
                }

                return BadRequest(ModelState);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
