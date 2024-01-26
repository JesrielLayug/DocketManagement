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
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FavoriteController : ControllerBase
    {
        private readonly IDocketService docketService;
        private readonly IUserService userService;
        private readonly IDocketRateService rateService;
        private readonly IDocketFavoriteService favoriteService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public FavoriteController
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
        public async Task<IActionResult> GetAll()
        {
            try
            {
                if (ModelState.IsValid && httpContextAccessor.HttpContext != null)
                {
                    var result = await favoriteService.GetAll();
                    return Ok(result);
                }

                return NotFound("No favorite dockets");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetByUserId")]
        public async Task<IActionResult> GetByCurrentUser()
        {
            try
            {
                if (ModelState.IsValid && httpContextAccessor.HttpContext != null)
                {
                    var currentUser = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

                    var user_favorites = await favoriteService.GetByUserId(currentUser);

                    var user_rates = await rateService.GetByUserId(currentUser);


                    var dockets = await docketService.GetAll();

                    var users = await userService.GetAll();

                    var rates = await rateService.GetAll();

                    var favorites = await favoriteService.GetAll();

                    var dto_dockets = dockets.Convert(users, rates, favorites, currentUser);

                    var userFavoriteDockets = dto_dockets.WithFavorite(user_favorites, user_rates);

                    return Ok(userFavoriteDockets);
                }
                return NotFound();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] DTOFeatureFavorite featureFavorite)
        {
            try
            {
                if (ModelState.IsValid && httpContextAccessor.HttpContext != null)
                {
                    var currentUser = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

                    var existingFavorite = await favoriteService.GetExisting(currentUser, featureFavorite.DocketId);

                    if (existingFavorite != null)
                        return Conflict(existingFavorite);
                    else
                    {
                        await favoriteService.Add(new Favorite
                        {
                            IsFavorite = featureFavorite.IsFavorite,
                            DocketId = featureFavorite.DocketId,
                            UserId = currentUser
                        });

                        return Ok("Successfully added the docket to favorites");
                    }
                }

                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("Delete/{docketId}")]
        public async Task<IActionResult> Delete(string docketId)
        {
            try
            {
                if (ModelState.IsValid && httpContextAccessor.HttpContext != null)
                {
                    var userId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

                    await favoriteService.Remove(docketId, userId);

                    return Ok("Successfully remove from the favorite");
                }

                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
