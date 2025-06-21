using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Repository.Repositories;
using Repository.Repositories.Interface;
using Service.DTOs.Favorite;
using Service.DTOs.Watchlist;
using Service.Helpers.Exceptions;
using Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class WatchlistService : IWatchlistService
    {
        private readonly IWatchlistRepository _watchlistRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<WatchlistService> _logger;

        public WatchlistService(IWatchlistRepository watchlistRepository, 
                                IMapper mapper,
                                ILogger<WatchlistService> logger)
        {
            _watchlistRepository = watchlistRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task AddAsync(WatchlistCreateDto request, string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                _logger.LogWarning("UserId is null or empty");
                throw new ArgumentException("UserId cannot be null or empty.");
            }

            if (request == null)
            {
                _logger.LogWarning("WatchlistCreateDto request is null for user {UserId}", userId);
                throw new ArgumentNullException(nameof(request), "Watchlist request cannot be null.");
            }

            if (request.MovieId == null && request.SeriesId == null)
            {
                _logger.LogWarning("Neither MovieId nor SeriesId provided by user {UserId}", userId);
                throw new InvalidFavoriteException("The watchlist must be related to either a movie or a series.");
            }

            if (request.MovieId != null && request.SeriesId != null)
            {
                _logger.LogWarning("Both MovieId and SeriesId provided. UserId: {UserId}", userId);
                throw new InvalidFavoriteException("Watchlist cannot be added to both a movie and a series at the same time.");
            }

            try
            {
                var watchlist = _mapper.Map<Watchlist>(request);
                watchlist.UserId = userId;
                await _watchlistRepository.CreateAsync(watchlist);
                _logger.LogInformation("Watchlist created by user {UserId} for {Target}", userId, request.MovieId != null ? $"Movie {request.MovieId}" : $"Series {request.SeriesId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating watchlist for user {UserId}", userId);
                throw;
            }
            
        }

        public async Task DeleteAsync(int id, string userId)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Invalid watchlist Id provided: {Id}", id);
                throw new ArgumentException("Watchlist Id must be a positive integer.");
            }

            if (string.IsNullOrWhiteSpace(userId))
            {
                _logger.LogWarning("UserId is null or empty for watchlist deletion with Id: {Id}", id);
                throw new ArgumentException("UserId cannot be null or empty.");
            }

            var watchlist = await _watchlistRepository.GetByIdAsync(id);

            if (watchlist == null)
            {
                _logger.LogWarning("Watchlist not found with Id: {Id} for user {UserId}", id, userId);
                throw new KeyNotFoundException($"Watchlist with Id {id} not found.");
            }

            if (watchlist.UserId != userId)
            {
                _logger.LogWarning("Unauthorized access attempt. User {UserId} tried to delete watchlist {Id} owned by {OwnerId}", userId, id, watchlist.UserId);
                throw new UnauthorizedAccessException("You can only delete your own watchlists.");
            }

            try
            {
                await _watchlistRepository.DeleteAsync(watchlist);
                _logger.LogInformation("Watchlist with Id {Id} successfully deleted by user {UserId}", id, userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting watchlist with Id {Id} for user {UserId}", id, userId);
                throw;
            }
            
        }

        public async Task<IEnumerable<WatchlistDto>> GetAllByUserAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                _logger.LogWarning("UserId is null or empty");
                throw new ArgumentException("UserId cannot be null or empty.");
            }

            var userWatchlists = await _watchlistRepository.GetAllByUserAsync(userId);

            if (userWatchlists == null || !userWatchlists.Any())
            {
                _logger.LogInformation("No watchlists found for user {UserId}", userId);
                return Enumerable.Empty<WatchlistDto>();
            }

            _logger.LogInformation("Retrieved {Count} watchlists for user {UserId}", userWatchlists.Count(), userId);

            return _mapper.Map<IEnumerable<WatchlistDto>>(userWatchlists);
        }

        public async Task<WatchlistDto> GetByIdAsync(int id, string userId)
        {

            if (id <= 0)
            {
                _logger.LogWarning("Invalid watchlist Id provided: {Id}", id);
                throw new ArgumentException("Watchlist Id must be a positive integer.");
            }

            if (string.IsNullOrWhiteSpace(userId))
            {
                _logger.LogWarning("UserId is null or empty for watchlist retrieval with Id: {Id}", id);
                throw new ArgumentException("UserId cannot be null or empty.");
            }

            var watchlist = await _watchlistRepository.GetByIdAsync(id);

            if (watchlist == null)
            {
                _logger.LogWarning("Watchlist not found with Id: {Id} for user {UserId}", id, userId);
                throw new KeyNotFoundException($"Watchlist with Id {id} not found.");
            }

            if (watchlist.UserId != userId)
            {
                _logger.LogWarning("Unauthorized access attempt. User {UserId} tried to access watchlist {Id} owned by {OwnerId}", userId, id, watchlist.UserId);
                throw new UnauthorizedAccessException("You can only access your own watchlists.");
            }

            _logger.LogInformation("Watchlist with Id {Id} successfully retrieved by user {UserId}", id, userId);

            return _mapper.Map<WatchlistDto>(watchlist);

        }

        public async Task<IEnumerable<WatchlistDto>> GetMoviesByUserAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                _logger.LogWarning("UserId is null or empty for watchlist movies retrieval");
                throw new ArgumentException("UserId cannot be null or empty.");
            }

            var watchlistMovies = await _watchlistRepository.GetMoviesByUserAsync(userId);

            if (watchlistMovies == null || !watchlistMovies.Any())
            {
                _logger.LogInformation("No watchlist movies found for user {UserId}", userId);
                return Enumerable.Empty<WatchlistDto>();
            }

            _logger.LogInformation("Retrieved {Count} watchlist movies for user {UserId}", watchlistMovies.Count(), userId);

            return _mapper.Map<IEnumerable<WatchlistDto>>(watchlistMovies);
        }

        public async Task<IEnumerable<WatchlistDto>> GetSeriesByUserAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                _logger.LogWarning("UserId is null or empty for watchlist series retrieval");
                throw new ArgumentException("UserId cannot be null or empty.");
            }

            var watchlistSeries = await _watchlistRepository.GetSeriesByUserAsync(userId);

            if (watchlistSeries == null || !watchlistSeries.Any())
            {
                _logger.LogInformation("No watchlist series found for user {UserId}", userId);
                return Enumerable.Empty<WatchlistDto>();
            }

            _logger.LogInformation("Retrieved {Count} watchlist series for user {UserId}", watchlistSeries.Count(), userId);

            return _mapper.Map<IEnumerable<WatchlistDto>>(watchlistSeries);
        }

        public async Task<bool> IsInWatchlistAsync(string userId, int? movieId, int? seriesId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                _logger.LogWarning("UserId is null or empty for watchlist check");
                throw new ArgumentException("UserId cannot be null or empty.");
            }

            if (movieId == null && seriesId == null)
            {
                _logger.LogWarning("Neither MovieId nor SeriesId provided for watchlist check by user {UserId}", userId);
                throw new ArgumentException("Either MovieId or SeriesId must be provided.");
            }

            if (movieId != null && seriesId != null)
            {
                _logger.LogWarning("Both MovieId and SeriesId provided for watchlist check by user {UserId}", userId);
                throw new ArgumentException("Only one of MovieId or SeriesId should be provided.");
            }

            var result = await _watchlistRepository.IsInWatchlistAsync(userId, movieId, seriesId);

            _logger.LogInformation("Watchlist check for user {UserId} and {Target}: {Result}",
                userId,
                movieId != null ? $"Movie {movieId}" : $"Series {seriesId}",
                result);

            return result;
        }
    }
}
