using Docket.Server.Extensions;
using Docket.Server.Models;
using Docket.Server.Services.Contracts;
using Docket.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Linq;
using System.Net;
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

        [HttpGet("GetByCurrentUser")]
        public async Task<IActionResult> GetByCurrentUser()
        {
            try
            {
                if (ModelState.IsValid && httpContextAccessor.HttpContext != null)
                {
                    var currentUser = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

                    var favorites = await favoriteService.GetAll();

                    var dockets = await docketService.GetAll();

                    var rates = await rateService.GetAll();

                    var users = await userService.GetAll();

                    var userFavorites = dockets.UserFavorites(favorites, rates, users, currentUser);
                   
                    return Ok(userFavorites);
                }
                return NotFound();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpGet("GetAverageFavorite/{date}")]
        public async Task<IActionResult> GetAverageFavorite(string date)
        {
            try
            {
                if(httpContextAccessor.HttpContext != null)
                {

                    var user = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

                    var dockets = await docketService.GetByUserId(user);

                    var favorites = await favoriteService.GetAll();

                    var data = dockets
                    .Select(d =>
                    {
                        var decodedDate = WebUtility.UrlDecode(date);
                        return new DTOFavoriteReport
                        {
                            Title = d.Title,
                            SumOfFavoritesForDay = favorites.Count(favorite =>
                            favorite.DocketId == d.Id &&
                            favorite.DateAdded == decodedDate &&
                            favorite.IsFavorite),
                            DocketId = d.Id,
                            DateAddedd = decodedDate,
                            UserId = user
                        };
                    })
                    .ToList();


                    return Ok(data);
                }
                return BadRequest();
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
                    {
                        existingFavorite.IsFavorite = featureFavorite.IsFavorite;
                        await favoriteService.Update(existingFavorite);
                        return Ok("Success");
                    }
                    else
                    {
                        await favoriteService.Add(new Favorite
                        {
                            IsFavorite = featureFavorite.IsFavorite,
                            DocketId = featureFavorite.DocketId,
                            UserId = currentUser,
                            DateAdded = DateTime.Now.ToString("MM/dd/yyyy")
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
