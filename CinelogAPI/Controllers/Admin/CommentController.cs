using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.DTOs.Comment;
using Service.Services.Interfaces;
using System.Security.Claims;

namespace CinelogAPI.Controllers.Admin
{
    public class CommentController : BaseAdminController
    {
        private readonly ICommentService _commentService;
        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CommentCreateDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not found.");

            await _commentService.CreateAsync(request, userId);

            return Ok(new { message = "Comment created." });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _commentService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var comment = await _commentService.GetByIdAsync(id);
            return Ok(comment);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute]int id,CommentUpdateDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not found.");

            await _commentService.UpdateAsync(id, request, userId);

            return Ok(new { message = "Comment edited." });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not found.");

            await _commentService.DeleteAsync(id, userId);

            return Ok(new { message = "Comment deleted." });
        }

        [HttpGet("{movieId}")]
        public async Task<IActionResult> GetByMovieId([FromRoute] int movieId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var movieComments = await _commentService.GetByMovieIdAsync(movieId);
            return Ok(movieComments);
        }

        [HttpGet]
        public async Task<IActionResult> GetByUserId()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not found.");

            var userComments  = await _commentService.GetByUserIdAsync(userId);
            return Ok(userComments);
        }
    }


}
