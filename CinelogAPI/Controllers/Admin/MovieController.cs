using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.DTOs.Actor;
using Service.DTOs.Movie;
using Service.Services;
using Service.Services.Interfaces;

namespace CinelogAPI.Controllers.Admin
{
    public class MovieController : BaseAdminController
    {
        private readonly IMovieService _movieService;
        private readonly IWebHostEnvironment _env;

        public MovieController(IMovieService movieService,
                               IWebHostEnvironment env)
        {
            _movieService = movieService;
            _env = env;
        }

        [HttpGet]
        public  async Task<IActionResult> GetAll()
        {
            return Ok(await _movieService.GetAllAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] MovieCreateDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _movieService.CreateAsync(request , _env.WebRootPath);
            return Ok(request);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery]int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await _movieService.DeleteAsync(id);
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute]int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var movie = await _movieService.GetByIdAsync(id);
            return Ok(movie);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromForm] MovieUpdateDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await _movieService.UpdateAsync(id, request, _env.WebRootPath);
            return Ok(request);
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] string searchText)
        {
            return Ok(await _movieService.SearchAsync(searchText));
        }
        [HttpGet]
        public async Task<IActionResult> SortByCreateDate([FromQuery] string order)
        {
            return Ok(await _movieService.SortByCreateDateAsync(order));
        }
        [HttpGet]
        public async Task<IActionResult> GetAllPaginated([FromQuery] int page)
        {
            return Ok(await _movieService.GetAllPaginatedAsync(page));
        }

        [HttpGet("{genreId}")]
        public async Task<IActionResult> GetByGenreId([FromRoute] int genreId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var movies = await _movieService.GetByGenreIdAsync(genreId);
            return Ok(movies);
        }

        [HttpGet("{actorId}")]
        public async Task<IActionResult> GetByActorId([FromRoute] int actorId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var movies = await _movieService.GetByActorIdAsync(actorId);
            return Ok(movies);
        }

        [HttpGet("{directorId}")]
        public async Task<IActionResult> GetByDirectorId([FromRoute] int directorId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var movies = await _movieService.GetByDirectorIdAsync(directorId);
            return Ok(movies);
        }

        [HttpGet]
        public async Task<IActionResult> FilterByReleaseYear([FromQuery] int? year)
        {

            return Ok(await _movieService.FilterByReleaseYearAsync(year.Value));
        }

    }
}
