using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Repository.Repositories.Interface;
using Service.DTOs.Rating;
using Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class RatingService : IRatingService
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RatingService> _logger;

        public RatingService(IRatingRepository ratingRepository,
                             IMapper mapper,
                             ILogger<RatingService> logger)
        {
            _ratingRepository = ratingRepository;
            _mapper = mapper;
        }

        public async Task AddAsync(RatingCreateDto request, string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("UserId is null or empty during rating creation.");
                throw new UnauthorizedAccessException("User must be authenticated to add a rating.");
            }

            if (request.MovieId == null && request.SeriesId == null)
            {
                _logger.LogWarning("Rating must be related to either a movie or a series.");
                throw new ArgumentException("The rating must be related to either the movie or the series.");
            }

            if (request.MovieId != null && request.SeriesId != null)
            {
                _logger.LogWarning("Rating cannot be added to both a movie and a series at the same time.");
                throw new InvalidOperationException("Rating cannot be added to both a movie and a series at the same time.");
            }

            var rating = _mapper.Map<Rating>(request);
            rating.UserId = userId;

            await _ratingRepository.CreateAsync(rating);

            _logger.LogInformation("Rating successfully created by user {UserId}. MovieId: {MovieId}, SeriesId: {SeriesId}",
                userId, request.MovieId, request.SeriesId);
        }

        public async Task DeleteAsync(int id, string userId)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Invalid rating Id provided: {Id}", id);
                throw new ArgumentException("Rating Id must be a positive integer.");
            }

            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("UserId is null or empty during rating deletion.");
                throw new UnauthorizedAccessException("User must be authenticated to delete a rating.");
            }

            var rating = await _ratingRepository.GetByIdAsync(id);

            if (rating == null)
            {
                _logger.LogWarning("Rating not found with Id: {Id}", id);
                throw new KeyNotFoundException($"Rating with Id {id} not found.");
            }

            if (rating.UserId != userId)
            {
                _logger.LogWarning("Unauthorized delete attempt. User {UserId} tried to delete rating owned by {OwnerId}.", userId, rating.UserId);
                throw new UnauthorizedAccessException("You can only delete your own ratings.");
            }

            await _ratingRepository.DeleteAsync(rating);

            _logger.LogInformation("Rating with Id {Id} successfully deleted by user {UserId}.", id, userId);
        }

        public async Task<IEnumerable<RatingDto>> GetAllByUserAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("UserId is null or empty when attempting to fetch ratings.");
                throw new UnauthorizedAccessException("User must be authenticated to view ratings.");
            }

            var userRatings = await _ratingRepository.GetAllByUserAsync(userId);

            _logger.LogInformation("Fetched {Count} ratings for user {UserId}.", userRatings.Count(), userId);

            return _mapper.Map<IEnumerable<RatingDto>>(userRatings);
        }

        public async Task<RatingDto> GetByIdAsync(int id, string userId)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Invalid rating Id provided: {Id}", id);
                throw new ArgumentException("Rating Id must be a positive integer.");
            }

            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("UserId is null or empty when attempting to retrieve a rating.");
                throw new UnauthorizedAccessException("User must be authenticated to view rating.");
            }

            var rating = await _ratingRepository.GetByIdAsync(id);

            if (rating == null)
            {
                _logger.LogWarning("Rating with Id {Id} not found.", id);
                throw new KeyNotFoundException($"Rating with Id {id} not found.");
            }

            if (rating.UserId != userId)
            {
                _logger.LogWarning("Unauthorized access attempt. User {UserId} tried to access rating owned by {OwnerId}.", userId, rating.UserId);
                throw new UnauthorizedAccessException("You can only access your own ratings.");
            }

            _logger.LogInformation("Rating with Id {Id} successfully retrieved for user {UserId}.", id, userId);

            return _mapper.Map<RatingDto>(rating);
        }

        public async Task<IEnumerable<RatingDto>> GetMovieRatingsByUserAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("UserId is null or empty when attempting to fetch movie ratings.");
                throw new UnauthorizedAccessException("User must be authenticated to view movie ratings.");
            }

            var userMovieRating = await _ratingRepository.GetMovieRatingsByUserAsync(userId);

            _logger.LogInformation("Fetched {Count} movie ratings for user {UserId}.", userMovieRating.Count(), userId);

            return _mapper.Map<IEnumerable<RatingDto>>(userMovieRating);
        }

        public async Task<IEnumerable<RatingDto>> GetSeriesRatingsByUserAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("UserId is null or empty when attempting to fetch series ratings.");
                throw new UnauthorizedAccessException("User must be authenticated to view series ratings.");
            }

            var userSeriesRating = await _ratingRepository.GetSeriesRatingsByUserAsync(userId);

            _logger.LogInformation("Fetched {Count} series ratings for user {UserId}.", userSeriesRating.Count(), userId);

            return _mapper.Map<IEnumerable<RatingDto>>(userSeriesRating);
        }

        public async Task<bool> IsRatedAsync(string userId, int? movieId, int? seriesId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("IsRatedAsync called with null or empty userId.");
                throw new UnauthorizedAccessException("User must be authenticated.");
            }

            if (movieId == null && seriesId == null)
            {
                _logger.LogWarning("IsRatedAsync called without specifying movieId or seriesId.");
                throw new ArgumentException("Either movieId or seriesId must be provided.");
            }

            if (movieId != null && seriesId != null)
            {
                _logger.LogWarning("IsRatedAsync called with both movieId and seriesId: {MovieId}, {SeriesId}", movieId, seriesId);
                throw new ArgumentException("Cannot check rating for both movie and series at the same time.");
            }

            var result = await _ratingRepository.IsRatedAsync(userId, movieId, seriesId);

            _logger.LogInformation("Checked IsRatedAsync for userId: {UserId}, movieId: {MovieId}, seriesId: {SeriesId}. Result: {Result}",
                userId, movieId, seriesId, result);

            return result;
        }

        public async Task UpdateAsync(int id, RatingUpdateDto request, string userId)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Invalid rating Id provided: {Id}", id);
                throw new ArgumentException("Rating Id must be a positive integer.");
            }

            var existingRating = await _ratingRepository.GetByIdAsync(id);

            if (existingRating == null)
            {
                _logger.LogWarning("Rating not found with Id: {Id}", id);
                throw new KeyNotFoundException($"Rating with Id {id} not found.");
            }

            if (existingRating.UserId != userId)
            {
                _logger.LogWarning("Unauthorized rating update attempt. RatingId: {Id}, RequestingUser: {UserId}, ActualOwner: {OwnerId}",
                    id, userId, existingRating.UserId);
                throw new UnauthorizedAccessException("You can only access your own ratings.");
            }

            existingRating.Score = request.Score;

            await _ratingRepository.UpdateAsync(existingRating);

            _logger.LogInformation("Rating with Id {Id} successfully updated by user {UserId}", id, userId);
        }
    }
}
