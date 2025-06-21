using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.DTOs.Genre;
using Service.Services;
using Service.Services.Interfaces;

namespace CinelogAPI.Controllers.Admin
{
    public class GenreController : BaseAdminController
    {
        private readonly IGenreService _genreService;

        public GenreController(IGenreService genreService)
        {
            _genreService = genreService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _genreService.GetAllAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]GenreCreateDto request)
        {
            if(!ModelState.IsValid) 
                return BadRequest(ModelState);

            await _genreService.CreateAsync(request);
            return Ok(request);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            GenreDto genre = await _genreService.GetByIdAsync(id);

            return Ok(genre);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _genreService.DeleteAsync(id);

            return Ok();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] GenreUpdateDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _genreService.UpdateAsync(id, request);
            return Ok(request);
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] string searchText)
        {
            return Ok(await _genreService.SearchAsync(searchText));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPaginated([FromQuery] int page)
        {
            return Ok(await _genreService.GetAllPaginatedAsync(page));
        }

        [HttpGet]
        public async Task<IActionResult> ExistsByName([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("Name is required.");

            bool exists = await _genreService.ExistsByNameAsync(name);
            return Ok(new { exists ,name});
        }
    }
}
