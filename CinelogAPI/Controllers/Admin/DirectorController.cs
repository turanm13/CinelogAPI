using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.DTOs.Actor;
using Service.DTOs.Director;
using Service.Services;
using Service.Services.Interfaces;

namespace CinelogAPI.Controllers.Admin
{
    public class DirectorController : BaseAdminController
    {
        private readonly IDirectorService _directorService;
        private readonly IWebHostEnvironment _env;

        public DirectorController(IDirectorService directorService,
                                  IWebHostEnvironment env)
        {
            _directorService = directorService;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _directorService.GetAllAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm]DirectorCreateDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _directorService.CreateAsync(request, _env.WebRootPath);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id) 
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _directorService.DeleteAsync(id);
            return Ok();
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _directorService.GetByIdAsync(id));
        }

        [HttpGet("{movieId}")]
        public async Task<IActionResult> GetDirectorsByMovie([FromRoute] int movieId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var directorMovie =await _directorService.GetDirectorsByMovieAsync(movieId);
            return Ok(directorMovie);
        }

        [HttpGet("{seriesId}")]
        public async Task<IActionResult> GetDirectorsBySeries([FromRoute] int seriesId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var directorSeries = await _directorService.GetDirectorsBySeriesAsync(seriesId);
            return Ok(directorSeries);
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromForm] DirectorUpdateDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await _directorService.UpdateAsync(id, request, _env.WebRootPath);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] string searchText)
        {
            return Ok(await _directorService.SearchAsync(searchText));
        }

        [HttpGet]
        public async Task<IActionResult> SortByCreateDate([FromQuery] string order)
        {
            return Ok(await _directorService.SortByCreateDateAsync(order));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPaginated([FromQuery] int page)
        {
            return Ok(await _directorService.GetAllPaginatedAsync(page));
        }
    }
}
