using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Repository.Repositories.Interface;
using Service.DTOs.Favorite;
using Service.Helpers.Exceptions;
using Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<FavoriteService> _logger;

        public FavoriteService(IFavoriteRepository favoriteRepository,
                               IMapper mapper,
                               ILogger<FavoriteService> logger)
        {
            _favoriteRepository = favoriteRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task AddAsync(FavoriteCreateDto request, string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                _logger.LogWarning("UserId is null or empty");
                throw new ArgumentException("UserId cannot be null or empty.");
            }

            if (request == null)
            {
                _logger.LogWarning("FavoriteCreateDto request is null for user {UserId}", userId);
                throw new ArgumentNullException(nameof(request), "Favorite request cannot be null.");
            }

            if (request.MovieId == null && request.SeriesId == null)
            {
                _logger.LogWarning("Neither MovieId nor SeriesId provided by user {UserId}", userId);
                throw new InvalidFavoriteException("The favorite must be related to either a movie or a series.");
            }

            if (request.MovieId != null && request.SeriesId != null)
            {
                _logger.LogWarning("Both MovieId and SeriesId provided. UserId: {UserId}", userId);
                throw new InvalidFavoriteException("Favorite cannot be added to both a movie and a series at the same time.");
            }

            try
            {
                var favorite = _mapper.Map<Favorite>(request);
                favorite.UserId = userId;
                await _favoriteRepository.CreateAsync(favorite);
                _logger.LogInformation("Favorite created by user {UserId} for {Target}", userId, request.MovieId != null ? $"Movie {request.MovieId}" : $"Series {request.SeriesId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating favorite for user {UserId}", userId);
                throw;
            }
        }

        public async Task DeleteAsync(int id, string userId)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Invalid favorite Id provided: {Id}", id);
                throw new ArgumentException("Favorite Id must be a positive integer.");
            }

            if (string.IsNullOrWhiteSpace(userId))
            {
                _logger.LogWarning("UserId is null or empty for favorite deletion with Id: {Id}", id);
                throw new ArgumentException("UserId cannot be null or empty.");
            }

            var favorite = await _favoriteRepository.GetByIdAsync(id);

            if (favorite == null)
            {
                _logger.LogWarning("Favorite not found with Id: {Id} for user {UserId}", id, userId);
                throw new KeyNotFoundException($"Favorite with Id {id} not found.");
            }

            if (favorite.UserId != userId)
            {
                _logger.LogWarning("Unauthorized access attempt. User {UserId} tried to delete favorite {Id} owned by {OwnerId}", userId, id, favorite.UserId);
                throw new UnauthorizedAccessException("You can only delete your own favorites.");
            }

            try
            {
                await _favoriteRepository.DeleteAsync(favorite);
                _logger.LogInformation("Favorite with Id {Id} successfully deleted by user {UserId}", id, userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting favorite with Id {Id} for user {UserId}", id, userId);
                throw;
            }

        }

        public async Task<IEnumerable<FavoriteDto>> GetAllByUserAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                _logger.LogWarning("UserId is null or empty");
                throw new ArgumentException("UserId cannot be null or empty.");
            }

            var userFavorites = await _favoriteRepository.GetAllByUserAsync(userId);

            if (userFavorites == null || !userFavorites.Any())
            {
                _logger.LogInformation("No favorites found for user {UserId}", userId);
                return Enumerable.Empty<FavoriteDto>();
            }

            _logger.LogInformation("Retrieved {Count} favorites for user {UserId}", userFavorites.Count(), userId);
            return _mapper.Map<IEnumerable<FavoriteDto>>(userFavorites);
        }

        public async Task<FavoriteDto> GetByIdAsync(int id, string userId)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Invalid favorite Id provided: {Id}", id);
                throw new ArgumentException("Favorite Id must be a positive integer.");
            }

            if (string.IsNullOrWhiteSpace(userId))
            {
                _logger.LogWarning("UserId is null or empty for favorite retrieval with Id: {Id}", id);
                throw new ArgumentException("UserId cannot be null or empty.");
            }

            var favorite = await _favoriteRepository.GetByIdAsync(id);
            if (favorite == null)
            {
                _logger.LogWarning("Favorite not found with Id: {Id} for user {UserId}", id, userId);
                throw new KeyNotFoundException($"Favorite with Id {id} not found.");
            }

            if (favorite.UserId != userId)
            {
                _logger.LogWarning("Unauthorized access attempt. User {UserId} tried to access favorite {Id} owned by {OwnerId}", userId, id, favorite.UserId);
                throw new UnauthorizedAccessException("You can only access your own favorites.");
            }

            _logger.LogInformation("Favorite with Id {Id} successfully retrieved by user {UserId}", id, userId);
            return _mapper.Map<FavoriteDto>(favorite);
        }

        public async Task<IEnumerable<FavoriteDto>> GetMoviesByUserAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                _logger.LogWarning("UserId is null or empty for favorite movies retrieval");
                throw new ArgumentException("UserId cannot be null or empty.");
            }

            var favoriteMovies = await _favoriteRepository.GetMoviesByUserAsync(userId);

            if (favoriteMovies == null || !favoriteMovies.Any())
            {
                _logger.LogInformation("No favorite movies found for user {UserId}", userId);
                return Enumerable.Empty<FavoriteDto>();
            }

            _logger.LogInformation("Retrieved {Count} favorite movies for user {UserId}", favoriteMovies.Count(), userId);
            return _mapper.Map<IEnumerable<FavoriteDto>>(favoriteMovies);
        }

        public async Task<IEnumerable<FavoriteDto>> GetSeriesByUserAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                _logger.LogWarning("UserId is null or empty for favorite series retrieval");
                throw new ArgumentException("UserId cannot be null or empty.");
            }

            var favoriteSeries = await _favoriteRepository.GetSeriesByUserAsync(userId);

            if (favoriteSeries == null || !favoriteSeries.Any())
            {
                _logger.LogInformation("No favorite series found for user {UserId}", userId);
                return Enumerable.Empty<FavoriteDto>();
            }

            _logger.LogInformation("Retrieved {Count} favorite series for user {UserId}", favoriteSeries.Count(), userId);
            return _mapper.Map<IEnumerable<FavoriteDto>>(favoriteSeries);
        }

        public async Task<bool> IsFavoritedAsync(string userId, int? movieId, int? seriesId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                _logger.LogWarning("UserId is null or empty for favorite check");
                throw new ArgumentException("UserId cannot be null or empty.");
            }

            if (movieId == null && seriesId == null)
            {
                _logger.LogWarning("Neither MovieId nor SeriesId provided for favorite check by user {UserId}", userId);
                throw new ArgumentException("Either MovieId or SeriesId must be provided.");
            }

            if (movieId != null && seriesId != null)
            {
                _logger.LogWarning("Both MovieId and SeriesId provided for favorite check by user {UserId}", userId);
                throw new ArgumentException("Only one of MovieId or SeriesId should be provided.");
            }

            var result = await _favoriteRepository.IsFavoritedAsync(userId, movieId, seriesId);

            _logger.LogInformation("Favorite check for user {UserId} and {Target}: {Result}",
                userId,
                movieId != null ? $"Movie {movieId}" : $"Series {seriesId}",
                result);

            return result;
        }
    }
}
