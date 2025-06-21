using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Repository.Repositories;
using Repository.Repositories.Interface;
using Service.DTOs.Actor;
using Service.DTOs.Genre;
using Service.Helpers.Responses;
using Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class GenreService : IGenreService
    {
        private readonly IGenreRepository _genreRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GenreService> _logger;
        public GenreService(IGenreRepository genreRepository,
                            IMapper mapper,
                            ILogger<GenreService> logger)
        {
            _genreRepository = genreRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task CreateAsync(GenreCreateDto genre)
        {
            if (genre == null)
            {
                _logger.LogWarning("GenreCreateDto is null");
                throw new ArgumentNullException(nameof(genre), "Genre data cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(genre.Name))
            {
                _logger.LogWarning("Genre name is null or empty");
                throw new ArgumentException("Genre name cannot be null or empty.");
            }

            var model = _mapper.Map<Genre>(genre);
            await _genreRepository.CreateAsync(model);

            _logger.LogInformation("Genre '{GenreName}' successfully created", genre.Name);
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Invalid genre Id provided: {Id}", id);
                throw new ArgumentException("Genre Id must be a positive integer.");
            }

            var genre = await _genreRepository.GetByIdAsync(id);
            if (genre == null)
            {
                _logger.LogWarning("Genre not found with Id: {Id}", id);
                throw new KeyNotFoundException($"Genre with Id {id} not found.");
            }

            await _genreRepository.DeleteAsync(genre);

            _logger.LogInformation("Genre with Id {Id} and name '{GenreName}' successfully deleted", id, genre.Name);
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                _logger.LogWarning("Genre name is null or empty for existence check");
                throw new ArgumentException("Genre name cannot be null or empty.");
            }

            var result = await _genreRepository.ExistsByNameAsync(name);

            _logger.LogInformation("Genre existence check for name '{GenreName}': {Result}", name, result);

            return result;
        }

        public async Task<IEnumerable<GenreDto>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<GenreDto>>(await _genreRepository.GetAllAsync());
        }

        public async Task<PaginateResponse<GenreDto>> GetAllPaginatedAsync(int page, int take = 10)
        {
            var paginatedDatas = _mapper.Map<List<GenreDto>>(await _genreRepository.GetAllPaginatedAsync(page, take));

            int count = await _genreRepository.GetCountAsync();

            int totalPage = (int)Math.Ceiling((double)count / take);

            return new PaginateResponse<GenreDto>(paginatedDatas, page, totalPage);
        }

        public async Task<GenreDto> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Invalid genre Id provided: {Id}", id);
                throw new ArgumentException("Genre Id must be a positive integer.");
            }

            var genre = await _genreRepository.GetByIdAsync(id);
            if (genre == null)
            {
                _logger.LogWarning("Genre not found with Id: {Id}", id);
                throw new KeyNotFoundException($"Genre with Id {id} not found.");
            }

            _logger.LogInformation("Genre with Id {Id} and name '{GenreName}' successfully retrieved", id, genre.Name);
            return _mapper.Map<GenreDto>(genre);
        }

        public async Task<IEnumerable<GenreDto>> SearchAsync(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                _logger.LogWarning("Search text is null or empty");
                throw new ArgumentException("Search text cannot be null or empty.");
            }

            var genres = await _genreRepository.SearchAsync(searchText);

            if (genres == null || !genres.Any())
            {
                _logger.LogInformation("No genres found for search text: '{SearchText}'", searchText);
                return Enumerable.Empty<GenreDto>();
            }

            _logger.LogInformation("Found {Count} genres for search text: '{SearchText}'", genres.Count(), searchText);
            return _mapper.Map<IEnumerable<GenreDto>>(genres);
        }

        public async Task UpdateAsync(int id, GenreUpdateDto genre)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Invalid genre Id provided: {Id}", id);
                throw new ArgumentException("Genre Id must be a positive integer.");
            }

            if (genre == null)
            {
                _logger.LogWarning("Genre update data is null for Id: {Id}", id);
                throw new ArgumentNullException(nameof(genre), "Genre update data cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(genre.Name))
            {
                _logger.LogWarning("Genre name is null or empty for Id: {Id}", id);
                throw new ArgumentException("Genre name cannot be null or empty.");
            }

            var existingGenre = await _genreRepository.GetByIdAsync(id);
            if (existingGenre == null)
            {
                _logger.LogWarning("Genre not found with Id: {Id}", id);
                throw new KeyNotFoundException($"Genre with Id {id} not found.");
            }

            _logger.LogInformation("Updating genre with Id: {Id} from '{OldName}' to '{NewName}'", id, existingGenre.Name, genre.Name);

            existingGenre.Name = genre.Name;
            await _genreRepository.UpdateAsync(existingGenre);

            _logger.LogInformation("Genre with Id {Id} successfully updated", id);
        }
    }
}
