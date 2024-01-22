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
    public class DocketFeatureController : ControllerBase
    {
        private readonly IDocketFeatureService featureService;
        private readonly IDocketService docketService;
        private readonly IUserService userService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public DocketFeatureController
            (
                IDocketFeatureService featureService,
                IDocketService docketService,
                IUserService userService,
                IHttpContextAccessor httpContextAccessor
            )
        {
            this.featureService = featureService;
            this.docketService = docketService;
            this.userService = userService;
            this.httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("GetDocketFavorites")]
        public async Task<ActionResult<IEnumerable<DTODocketFeature>>> GetDocketFavorites()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dockets = await docketService.GetAll();

                    var favorites = await featureService.GetAllFavorites();

                    var users = await userService.GetAll();

                    var favoriteDockets = dockets.ConvertWithFeatures(users, favorites);

                    return Ok(favoriteDockets);
                }

                return NotFound("No favorite dockets");
            }
            catch(Exception ex)
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

                    var existingFavorite = await featureService.GetByDocketId(docketId);
                    if(existingFavorite == null)
                    {
                        await featureService.AddDocketToFavorite(new DocketFavorite
                        {
                            IsFavorite = featureFavorite.IsFavorite,
                            DocketId = docketId,
                            UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)
                        });

                        return Ok("Successfully added the docket to favorites");
                    }

                    return BadRequest("Docket is already in favorite.");
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
