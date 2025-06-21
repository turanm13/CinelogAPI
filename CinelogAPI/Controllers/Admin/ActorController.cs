using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.DTOs.Actor;
using Service.Services.Interfaces;

namespace CinelogAPI.Controllers.Admin
{
    public class ActorController : BaseAdminController
    {
        private readonly IActorService _actorService;
        private readonly IWebHostEnvironment _env;

        public ActorController(IActorService actorService,
                               IWebHostEnvironment env)
        {
            _actorService = actorService;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok( await _actorService.GetAllAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ActorCreateDto request)
        {
            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            await _actorService.CreateAsync(request, _env.WebRootPath);
            return Ok();
        }
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            await _actorService.DeleteAsync(id);
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult>GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(await _actorService.GetByIdAsync(id));
        }
        [HttpPut("{id}")]
        public async Task<IActionResult>Update ([FromRoute] int id,[FromForm] ActorUpdateDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _actorService.UpdateAsync(id, request, _env.WebRootPath);

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult>Search([FromQuery] string searchText)
        {
            return Ok(await _actorService.SearchAsync(searchText));
        }

        [HttpGet]
        public async Task<IActionResult> SortByCreatedDate([FromQuery] string order)
        {
            return Ok(await _actorService.SortByCreateDateAsync(order));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPaginated([FromQuery] int page)
        {
            return Ok(await _actorService.GetAllPaginatedAsync(page));
        }

        [HttpGet("{movieId}")]
        public async Task<IActionResult> GetActorsByMovie([FromRoute] int movieId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _actorService.GetActorsByMovieAsync(movieId);

            return Ok(result);
        }

        [HttpGet("{seriesId}")]
        public async Task<IActionResult> GetActorsBySeries([FromRoute] int seriesId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _actorService.GetActorsBySeriesAsync(seriesId);

            return Ok(result);
        }

    }
}
