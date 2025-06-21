using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Repository.Repositories;
using Repository.Repositories.Interface;
using Service.DTOs.Actor;
using Service.DTOs.Director;
using Service.Helpers.Responses;
using Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class DirectorService : IDirectorService
    {
        private readonly IDirectorRepository _directorRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<DirectorService> _logger;

        public DirectorService(IDirectorRepository directorRepository,
                               IMapper mapper,
                               ILogger<DirectorService> logger)
        {
            _directorRepository = directorRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task CreateAsync(DirectorCreateDto director, string webRootPath)
        {
            if (director == null)
            {
                _logger.LogWarning("Director data not provided in CreateAsync");
                throw new ArgumentNullException(nameof(director), "Director data is required");
            }

            if (director.PhotoUrl == null || director.PhotoUrl.Length == 0)
            {
                _logger.LogWarning("Photo file not provided in CreateAsync");
                throw new ArgumentException("Photo file is required");
            }

            string fileName = Guid.NewGuid().ToString() + "-" + director.PhotoUrl.FileName;
            string imagePath = Path.Combine(webRootPath, "images", fileName);

            try
            {
                _logger.LogInformation("Saving photo file to path: {Path}", imagePath);
                using (FileStream stream = new FileStream(imagePath, FileMode.Create))
                {
                    await director.PhotoUrl.CopyToAsync(stream);
                }
                _logger.LogInformation("Photo saved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while saving the photo file");
                throw new IOException("Photo file could not be saved", ex);
            }

            var model = _mapper.Map<Director>(director);
            model.PhotoUrl = fileName;
            await _directorRepository.CreateAsync(model);
            _logger.LogInformation("Director created successfully: {Name}", director.FullName);
        }

        public async Task DeleteAsync(int Id)
        {
            if (Id <= 0)
            {
                _logger.LogWarning("Invalid director Id provided: {Id}", Id);
                throw new ArgumentException("Director Id must be greater than 0");
            }

            _logger.LogInformation("Attempting to delete director with Id: {Id}", Id);

            var director = await _directorRepository.GetByIdAsync(Id);

            if (director == null)
            {
                _logger.LogWarning("Director not found with Id: {Id}", Id);
                throw new KeyNotFoundException($"Director with Id {Id} not found");
            }

            try
            {
                await _directorRepository.DeleteAsync(director);
                _logger.LogInformation("Director deleted successfully: Id {Id}, Name: {Name}", Id, director.FullName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting director with Id: {Id}", Id);
                throw new InvalidOperationException("Failed to delete director", ex);
            }
        }

        public async Task<IEnumerable<DirectorDto>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<DirectorDto>>(await _directorRepository.GetAllAsync());
        }

        public async Task<PaginateResponse<DirectorDto>> GetAllPaginatedAsync(int page, int take = 3)
        {
            var paginatedDatas = _mapper.Map<List<DirectorDto>>(await _directorRepository.GetAllPaginatedAsync(page, take));

            int count = await _directorRepository.GetCountAsync();

            int totalPage = (int)Math.Ceiling((double)count / take);

            return new PaginateResponse<DirectorDto>(paginatedDatas, page, totalPage);
        }

        public async Task<DirectorDto> GetByIdAsync(int Id)
        {
            if (Id <= 0)
            {
                _logger.LogWarning("Invalid director Id provided: {Id}", Id);
                throw new ArgumentException("Director Id must be greater than 0");
            }

            _logger.LogInformation("Retrieving director with Id: {Id}", Id);

            var director = await _directorRepository.GetByIdAsync(Id);

            if (director == null)
            {
                _logger.LogWarning("Director not found with Id: {Id}", Id);
                throw new KeyNotFoundException($"Director with Id {Id} not found");
            }

            _logger.LogInformation("Director retrieved successfully: Id {Id}, Name: {Name}", Id, director.FullName);
            return _mapper.Map<DirectorDto>(director);
        }

        public async Task<IEnumerable<DirectorDto>> GetDirectorsByMovieAsync(int movieId)
        {
            if (movieId <= 0)
            {
                _logger.LogWarning("Invalid movie Id provided: {MovieId}", movieId);
                throw new ArgumentException("Movie Id must be greater than 0");
            }

            _logger.LogInformation("Retrieving directors for movie with Id: {MovieId}", movieId);

            var directorMovies = await _directorRepository.GetDirectorsByMovieAsync(movieId);

            if (directorMovies == null)
            {
                _logger.LogWarning("Movie not found with Id: {MovieId}", movieId);
                throw new KeyNotFoundException($"Movie with Id {movieId} not found");
            }

            var directors = _mapper.Map<IEnumerable<DirectorDto>>(directorMovies);
            _logger.LogInformation("Retrieved {Count} directors for movie Id: {MovieId}", directors.Count(), movieId);

            return directors;
        }

        public async Task<IEnumerable<DirectorDto>> GetDirectorsBySeriesAsync(int seriesId)
        {
            if (seriesId <= 0)
            {
                _logger.LogWarning("Invalid series Id provided: {SeriesId}", seriesId);
                throw new ArgumentException("Series Id must be greater than 0");
            }

            _logger.LogInformation("Retrieving directors for series with Id: {SeriesId}", seriesId);

            var directorSeries = await _directorRepository.GetDirectorsBySeriesAsync(seriesId);

            if (directorSeries == null)
            {
                _logger.LogWarning("Series not found with Id: {SeriesId}", seriesId);
                throw new KeyNotFoundException($"Series with Id {seriesId} not found");
            }

            var directors = _mapper.Map<IEnumerable<DirectorDto>>(directorSeries);
            _logger.LogInformation("Retrieved {Count} directors for series Id: {SeriesId}", directors.Count(), seriesId);

            return directors;
        }

        public async Task<IEnumerable<DirectorDto>> SearchAsync(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                _logger.LogWarning("Search text was null or whitespace.");
                throw new ArgumentException("Search text must not be empty.");
            }

            var directors = await _directorRepository.SearchAsync(searchText);

            if (directors == null || !directors.Any())
            {
                _logger.LogInformation("No directors found for search text: {SearchText}", searchText);
                throw new KeyNotFoundException("No directors found matching the search criteria.");
            }

            return _mapper.Map<IEnumerable<DirectorDto>>(directors);
        }

        public async Task<IEnumerable<DirectorDto>> SortByCreateDateAsync(string order)
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

            var directors = await _directorRepository.SortByCreatedDateAsync(order);

            if (directors == null || !directors.Any())
            {
                _logger.LogInformation("No directors found for the given sort order: {Order}", order);
                throw new KeyNotFoundException("No directors found for the given sorting criteria.");
            }

            return _mapper.Map<IEnumerable<DirectorDto>>(directors);
        }

        public async Task UpdateAsync(int Id, DirectorUpdateDto director, string? webRootPath)
        {
            if (Id <= 0)
            {
                _logger.LogWarning("Invalid director Id provided for update: {Id}", Id);
                throw new ArgumentException("Director Id must be greater than 0");
            }

            var existingDirector = await _directorRepository.GetByIdAsync(Id);
            if (existingDirector == null)
            {
                _logger.LogWarning("Director with Id {DirectorId} not found for update.", Id);
                throw new KeyNotFoundException("Director not found");
            }

            if (!string.IsNullOrEmpty(director.FullName))
                existingDirector.FullName = director.FullName;
            if (!string.IsNullOrEmpty(director.Bio))
                existingDirector.Bio = director.Bio;
            if (director.BirthDate.HasValue)
                existingDirector.BirthDate = director.BirthDate.Value;

            if (director.PhotoUrl != null && !string.IsNullOrEmpty(director.PhotoUrl.FileName))
            {
                try
                {
                    string fileName = Guid.NewGuid().ToString() + "-" + director.PhotoUrl.FileName;
                    string imagePath = Path.Combine(webRootPath!, "images", fileName);
                    using (FileStream stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await director.PhotoUrl.CopyToAsync(stream);
                    }
                    existingDirector.PhotoUrl = fileName;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Photo could not be saved while updating director Id {DirectorId}", Id);
                    throw new IOException("Photo file could not be saved", ex);
                }
            }

            await _directorRepository.UpdateAsync(existingDirector);
            _logger.LogInformation("Director with Id {DirectorId} updated successfully.", Id);
        }
    }
}
