using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.DTOs.Favorite;
using Service.DTOs.Rating;
using Service.Services;
using Service.Services.Interfaces;
using System.Security.Claims;

namespace CinelogAPI.Controllers.UI
{ 
    public class RatingController : BaseUIController
    {
        private readonly IRatingService _ratingService;

        public RatingController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] RatingCreateDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not found.");

            await _ratingService.AddAsync(request, userId);

            return Ok(new { message = "Successfuly added." });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not found.");
            var rating = await _ratingService.GetByIdAsync(id, userId);
            return Ok(rating);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute]int id, [FromBody] RatingUpdateDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not found.");

            await _ratingService.UpdateAsync(id, request, userId);

            return Ok(new { message = "Successfuly updated." });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery]int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not found.");

            await _ratingService.DeleteAsync(id,userId);
            return Ok(new { message = "Successfuly deleted." });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllByUser()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not found.");

            var userRatings =  await _ratingService.GetAllByUserAsync(userId);

            return Ok(userRatings);
        }

        [HttpGet]
        public async Task<IActionResult> GetMovieRatingsByUser()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not found.");

            var userMovieRatings = await _ratingService.GetMovieRatingsByUserAsync(userId);

            return Ok(userMovieRatings);
        }
        
        [HttpGet]
        public async Task<IActionResult> GetSeriesRatingsByUser()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not found.");

            var userSeriesRatings = await _ratingService.GetSeriesRatingsByUserAsync(userId);

            return Ok(userSeriesRatings);
        }

        [HttpGet]
        public async Task<IActionResult> IsRated([FromQuery] int? movieId, [FromQuery] int? seriesId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not found.");

            var isRated = await _ratingService.IsRatedAsync(userId,movieId,seriesId);

            return Ok(new {isRated});
        }
    }
}
