using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.DTOs.Favorite;
using Service.DTOs.Watchlist;
using Service.Services;
using Service.Services.Interfaces;
using System.Security.Claims;

namespace CinelogAPI.Controllers.UI
{
    public class WatchlistController : BaseUIController
    {
        private readonly IWatchlistService _watchlistService;

        public WatchlistController(IWatchlistService watchlistService)
        {
            _watchlistService = watchlistService;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] WatchlistCreateDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not found.");

            await _watchlistService.AddAsync(request, userId);

            return Ok(new { message =  "Successfuly added." });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllByUser()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not found.");

            var watchlists = await _watchlistService.GetAllByUserAsync(userId);
            return Ok(watchlists);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not found.");
            var watchlist = await _watchlistService.GetByIdAsync(id, userId);
            return Ok(watchlist);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not found.");

            await _watchlistService.DeleteAsync(id, userId);

            return Ok(new { message = "Watchlist deleted." });
        }

        [HttpGet]
        public async Task<IActionResult> GetMoviesByUser()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not found.");
            var watchlistMovies = await _watchlistService.GetMoviesByUserAsync(userId);
            return Ok(watchlistMovies);
        }

        [HttpGet]
        public async Task<IActionResult> GetSeriesByUser()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not found.");
            var watchlistSeries = await _watchlistService.GetSeriesByUserAsync(userId);
            return Ok(watchlistSeries);
        }

        [HttpGet]
        public async Task<IActionResult> IsInWatchlist([FromQuery] int? movieId, [FromQuery] int? seriesId)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not found.");

            var isWatchlist = await _watchlistService.IsInWatchlistAsync(userId, movieId, seriesId);

            return Ok(new { isWatchlist });
        }

    }
}
