using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.DTOs.Actor;
using Service.DTOs.Episode;
using Service.Services;
using Service.Services.Interfaces;

namespace CinelogAPI.Controllers.Admin
{
    public class EpisodeController : BaseAdminController
    {
        private readonly IEpisodeService _episodeService;

        public EpisodeController(IEpisodeService episodeService)
        {
            _episodeService = episodeService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]EpisodeCreateDto request)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            await _episodeService.CreateAsync(request);
            return Ok(request);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(await _episodeService.GetAllAsync());
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            await _episodeService.DeleteAsync(id);
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute]int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var episode = await _episodeService.GetByIdAsync(id);
            return Ok(episode);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] EpisodeUpdateDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await _episodeService.UpdateAsync(id, request);
            return Ok(request);
        }
        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] string searchText)
        {
            return Ok(await _episodeService.SearchAsync(searchText));
        }
        [HttpGet]
        public async Task<IActionResult> GetAllPaginated([FromQuery] int page)
        {
            return Ok(await _episodeService.GetAllPaginatedAsync(page));
        }
        [HttpGet]
        public async Task<IActionResult> GetBySeriesId([FromQuery] int seriesId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(await _episodeService.GetBySeriesIdAsync(seriesId));
        }
    }
}
