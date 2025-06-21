using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.DTOs.Movie;
using Service.DTOs.Series;
using Service.Services;
using Service.Services.Interfaces;

namespace CinelogAPI.Controllers.Admin
{
    public class SeriesController : BaseAdminController
    {
        private readonly ISeriesService _seriesService;
        private readonly IWebHostEnvironment _env;

        public SeriesController(ISeriesService seriesService,
                                IWebHostEnvironment env)
        {
            _seriesService = seriesService;
            _env = env;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] SeriesCreateDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _seriesService.CreateAsync(request, _env.WebRootPath);
            return Ok(request);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _seriesService.GetAllAsync());
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await _seriesService.DeleteAsync(id);
            return Ok();
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var series = await _seriesService.GetByIdAsync(id);
            return Ok(series);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromForm] SeriesUpdateDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await _seriesService.UpdateAsync(id, request, _env.WebRootPath);
            return Ok(request);
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] string searchText)
        {
            return Ok(await _seriesService.SearchAsync(searchText));
        }
        [HttpGet]
        public async Task<IActionResult> SortByCreateDate([FromQuery] string order)
        {
            return Ok(await _seriesService.SortByCreateDateAsync(order));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPaginated([FromQuery] int page)
        {
            return Ok(await _seriesService.GetAllPaginatedAsync(page));
        }

        [HttpGet("{genreId}")]
        public async Task<IActionResult> GetByGenreId([FromRoute] int genreId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var series = await _seriesService.GetByGenreIdAsync(genreId);
            return Ok(series);
        }
        [HttpGet("{actorId}")]
        public async Task<IActionResult> GetByActorId([FromRoute] int actorId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var series = await _seriesService.GetByActorIdAsync(actorId);
            return Ok(series);
        }
        [HttpGet("{directorId}")]
        public async Task<IActionResult> GetByDirectorId([FromRoute] int directorId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var series = await _seriesService.GetByDirectorIdAsync(directorId);
            return Ok(series);
        }

        [HttpGet]
        public async Task<IActionResult> FilterByReleaseYear([FromQuery] int? year)
        {

            return Ok(await _seriesService.FilterByReleaseYearAsync(year.Value));
        }
    }
}
