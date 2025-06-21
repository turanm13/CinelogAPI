using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.DTOs.Favorite;
using Service.Services;
using Service.Services.Interfaces;
using System.Security.Claims;

namespace CinelogAPI.Controllers.UI
{
    public class FavoriteController : BaseUIController
    {
        private readonly IFavoriteService _favoriteService;
        public FavoriteController(IFavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] FavoriteCreateDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not found.");

            await _favoriteService.AddAsync(request, userId);

            return Ok(new { message = "Successfuly added." });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllByUser()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not found.");

            var favorites = await _favoriteService.GetAllByUserAsync(userId);
            return Ok(favorites);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not found.");
            var favorite = await _favoriteService.GetByIdAsync(id,userId);
            return Ok(favorite);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery]int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not found.");

            await _favoriteService.DeleteAsync(id, userId);

            return Ok(new { message = "Favorite deleted." });
        }

        [HttpGet]
        public async Task<IActionResult> GetMoviesByUser()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not found.");
            var favoriteMovies = await _favoriteService.GetMoviesByUserAsync(userId);
            return Ok(favoriteMovies);
        }

        [HttpGet]
        public async Task<IActionResult> GetSeriesByUser()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not found.");
            var favoriteSeries = await _favoriteService.GetSeriesByUserAsync(userId);
            return Ok(favoriteSeries);
        }

        [HttpGet]
        public async Task<IActionResult> IsFavorited([FromQuery] int? movieId, [FromQuery] int? seriesId)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not found.");

            var isFavorited = await _favoriteService.IsFavoritedAsync(userId, movieId, seriesId);

            return Ok(new { isFavorited });
        }
    }
}
