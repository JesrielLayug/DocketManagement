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

        [HttpGet("GetUserFavoriteDocket")]
        public async Task<ActionResult<IEnumerable<DTODocketWithRateAndFavorite>>> GetUserFavoriteDocket()
        {
            try
            {
                if (ModelState.IsValid && httpContextAccessor.HttpContext != null)
                {
                    var dockets = await docketService.GetAll();

                    var favorites = await featureService.GetUserFavoriteDockets(httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

                    var users = await userService.GetAll();

                    var rates = await featureService.GetAllRates();

                    var favoriteDockets = dockets.Favorite(users, favorites, rates);

                    return Ok(favoriteDockets);
                }

                return NotFound("No favorite dockets");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetUserCurrentRateToDocket/{docketId}")]
        public async Task<ActionResult<DTOFeatureRate>> GetUserCurrentRateToDocket(string docketId)
        {
            try
            {
                if(ModelState.IsValid && httpContextAccessor.HttpContext != null)
                {
                    var user = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

                    var rate = await featureService.GetExistingRatedOfUser(user, docketId);

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
                        Rate = rate.Rate,
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

        [HttpGet("GetUserRatedDocket")]
        public async Task<ActionResult<IEnumerable<DTOFeatureRate>>> GetUserRatedDocket()
        {
            try
            {
                if (ModelState.IsValid && httpContextAccessor.HttpContext != null)
                {
                    var user = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

                    var rates = await featureService.GetUserRatedDocket(user);

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


        [HttpPost("AddDocketToFavorite")]
        public async Task<IActionResult> AddDocketToFavorite([FromBody] DTOFeatureFavorite featureFavorite)
        {
            try
            {
                if (ModelState.IsValid && httpContextAccessor.HttpContext != null)
                {
                    var currentUser = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

                    await featureService.AddDocketToFavorite(new DocketFavorite
                    {
                        IsFavorite = featureFavorite.IsFavorite,
                        DocketId = featureFavorite.DocketId,
                        UserId = currentUser
                    });

                    return Ok("Successfully added the docket to favorites");
                }

                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AddRateToDocket")]
        public async Task<IActionResult> AddRateToDocket([FromBody] DTOFeatureRate request)
        {
            try
            {
                if (ModelState.IsValid && httpContextAccessor.HttpContext != null)
                {
                    var user = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

                    var userRated = await featureService.ExistingUserRated(user, request.DocketId);

                    if (userRated != null)
                    {

                        userRated.Rate = request.Rate;

                        await featureService.UpdateRateToDocket(userRated);
                    }
                    else
                    {
                        await featureService.AddRateToDocket(new DocketRate
                        {
                            DocketId = request.DocketId,
                            Rate = request.Rate,
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
