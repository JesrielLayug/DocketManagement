﻿using Docket.Server.Models;
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

        [HttpGet("GetUserRatedDocket")]
        public async Task<ActionResult<IEnumerable<DTODocketWithRate>>> GetUserRatedDocket()
        {
            try
            {
                if (ModelState.IsValid && httpContextAccessor.HttpContext != null)
                {
                    var rates = await featureService.GetUserRatedDocket(httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

                    var dockets = await docketService.GetAll();

                    var users = await userService.GetAll();

                    var ratedDockets = dockets.Docket(users, rates);

                    return Ok(ratedDockets);
                }
                return NotFound("No rated dockets found");
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
                    var currentUser = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

                    var existingFavorite = await featureService.GetExistingFavoriteDocket(currentUser, docketId);
                    if (existingFavorite == null)
                    {
                        await featureService.AddDocketToFavorite(new DocketFavorite
                        {
                            IsFavorite = featureFavorite.IsFavorite,
                            DocketId = docketId,
                            UserId = currentUser
                        });

                        return Ok("Successfully added the docket to favorites");
                    }

                    return BadRequest("Docket is already in favorite.");
                }

                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("RateDocket")]
        public async Task<IActionResult> AddRateToDocket([FromBody] DTOFeatureAddRate request)
        {
            try
            {
                if (ModelState.IsValid && httpContextAccessor.HttpContext != null)
                {

                    var existingRate = await featureService.GetByDocketIdFromRate(request.DocketId);
                    
                    existingRate.Rate = (existingRate.Rate + request.Rate) / 2;
                    existingRate.DocketId = request.DocketId;

                    await featureService.UpdateRateToDocket(request.DocketId, existingRate);
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
