using AutoMapper;
using Domain.Entities;
using Domain.Entities.Join;
using Microsoft.Extensions.Logging;
using Repository.Repositories.Interface;
using Service.DTOs.Actor;
using Service.Helpers.Responses;
using Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Service.Services
{
    public class ActorService : IActorService
    {
        private readonly IActorRepository _actorRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ActorService> _logger;

        public ActorService(IActorRepository actorRepository,
                            IMapper mapper,
                            ILogger<ActorService> logger)
        {
            _actorRepository = actorRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task CreateAsync(ActorCreateDto actor, string webRootPath)
        {
            if (actor.ImageUrl == null || actor.ImageUrl.Length == 0)
            {
                _logger.LogWarning("Image file not provided in CreateAsync");
                throw new ArgumentException("Image file is required");
            }

            string fileName = Guid.NewGuid().ToString() + "-" + actor.ImageUrl.FileName;
            string imagePath = Path.Combine(webRootPath, "images", fileName);

            try
            {
                _logger.LogInformation("Saving image file to path: {Path}", imagePath);

                using (FileStream stream = new FileStream(imagePath, FileMode.Create))
                {
                    await actor.ImageUrl.CopyToAsync(stream);
                }

                _logger.LogInformation("Image saved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while saving the image file");
                throw new IOException("Image file could not be saved", ex);
            }

            var model = _mapper.Map<Actor>(actor);
            model.ImageUrl = fileName;

            await _actorRepository.CreateAsync(model);

           _logger.LogInformation("Actor created successfully: {Name}", actor.FullName);
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Invalid actor Id provided: {Id}", id);
                throw new ArgumentException("Actor Id must be greater than 0");
            }

            _logger.LogInformation("Attempting to delete actor with Id: {Id}", id);
            var actor = await _actorRepository.GetByIdAsync(id);

            if (actor == null)
            {
                _logger.LogWarning("Actor with id {Id} not found for deletion.", id);
                throw new KeyNotFoundException($"Actor with id {id} not found.");
            }

            try
            {
                await _actorRepository.DeleteAsync(actor);
                _logger.LogInformation("Actor with id {Id} deleted successfully.", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting actor with id {Id}", id);
                throw new IOException("Failed to delete actor", ex);
            }
        }

        public async Task<IEnumerable<ActorDto>> GetActorsByMovieAsync(int movieId)
        {
            var movieActors = await _actorRepository.GetActorsByMovieAsync(movieId);

            if (movieActors == null || !movieActors.Any())
            {
                _logger.LogWarning("No actors found for movieId: {MovieId}", movieId);
                throw new KeyNotFoundException($"No actors found for movie with Id {movieId}");
            }

            _logger.LogInformation("Found {Count} actors for movieId: {MovieId}", movieActors.Count(), movieId);

            return _mapper.Map<IEnumerable<ActorDto>>(movieActors);
        }

        public async Task<IEnumerable<ActorDto>> GetActorsBySeriesAsync(int seriesId)
        {
            var seriesActors = await _actorRepository.GetActorsBySeriesAsync(seriesId);

            if (seriesActors == null || !seriesActors.Any())
            {
                _logger.LogWarning("No actors found for seriesId: {SeriesId}", seriesId);
                throw new KeyNotFoundException($"No actors found for series with Id {seriesId}");
            }

            _logger.LogInformation("Found {Count} actors for seriesId: {SeriesId}", seriesActors.Count(), seriesId);

            return _mapper.Map<IEnumerable<ActorDto>>(seriesActors);
        }

        public async Task<IEnumerable<ActorDto>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<ActorDto>>(await _actorRepository.GetAllAsync());
        }

        public async Task<PaginateResponse<ActorDto>> GetAllPaginatedAsync(int page, int take = 3)
        {
            var paginatedDatas = _mapper.Map<List<ActorDto>>(await _actorRepository.GetAllPaginatedAsync(page, take));

            int count = await _actorRepository.GetCountAsync();

            int totalPage = (int)Math.Ceiling((double)count / take);

            return new PaginateResponse<ActorDto>(paginatedDatas, page, totalPage);
        }

        public async Task<ActorDto> GetByIdAsync(int id)
        {
            var actor = await _actorRepository.GetByIdAsync(id);

            if (actor == null)
            {
                _logger.LogWarning("Actor not found with Id: {Id}", id);
                throw new KeyNotFoundException($"Actor with Id {id} was not found.");
            }

            return _mapper.Map<ActorDto>(actor);
        }

        public async Task<IEnumerable<ActorDto>> SearchAsync(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                _logger.LogWarning("Search text was null or whitespace.");
                throw new ArgumentException("Search text must not be empty.");
            }

            var actors = await _actorRepository.SearchAsync(searchText);

            if (actors == null || !actors.Any())
            {
                _logger.LogInformation("No actors found for search text: {SearchText}", searchText);
                throw new KeyNotFoundException("No actors found matching the search criteria.");
            }

            return _mapper.Map<IEnumerable<ActorDto>>(actors);
        }

        public async Task<IEnumerable<ActorDto>> SortByCreateDateAsync(string order)
        {
            if (string.IsNullOrWhiteSpace(order))
            {
                _logger.LogWarning("Sort order is missing.");
                throw new ArgumentException("Sort order must be specified.");
            }

            order = order.ToLower();
            if (order != "asc" && order != "desc")
            {
                _logger.LogWarning("Invalid sort order provided: {Order}", order);
                throw new ArgumentException("Sort order must be either 'asc' or 'desc'.");
            }

            var actors = await _actorRepository.SortByCreatedDateAsync(order);

            if (actors == null || !actors.Any())
            {
                _logger.LogInformation("No actors found for the given sort order: {Order}", order);
                throw new KeyNotFoundException("No actors found for the given sorting criteria.");
            }

            return _mapper.Map<IEnumerable<ActorDto>>(actors);
        }

        public async Task UpdateAsync(int id, ActorUpdateDto actor, string? webRootPath)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Invalid actor Id provided for update: {Id}", id);
                throw new ArgumentException("Actor Id must be greater than 0");
            }

            var existingActor = await _actorRepository.GetByIdAsync(id);
            if (existingActor == null)
            {
                _logger.LogWarning("Actor with Id {ActorId} not found for update.", id);
                throw new KeyNotFoundException("Actor not found");
            }

            if (!string.IsNullOrEmpty(actor.FullName))
                existingActor.FullName = actor.FullName;

            if (!string.IsNullOrEmpty(actor.Bio))
                existingActor.Bio = actor.Bio;

            if (actor.BirthDate.HasValue)
                existingActor.BirthDate = actor.BirthDate.Value;

            if (actor.ImageUrl != null && !string.IsNullOrEmpty(actor.ImageUrl.FileName))
            {
                try
                {
                    string fileName = Guid.NewGuid().ToString() + "-" + actor.ImageUrl.FileName;
                    string imagePath = Path.Combine(webRootPath!, "images", fileName);

                    using (FileStream stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await actor.ImageUrl.CopyToAsync(stream);
                    }

                    existingActor.ImageUrl = fileName;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Image could not be saved while updating actor Id {ActorId}", id);
                    throw new IOException("Image file could not be saved", ex);
                }
            }

            await _actorRepository.UpdateAsync(existingActor);
            _logger.LogInformation("Actor with Id {ActorId} updated successfully.", id);
        }
    }
}
