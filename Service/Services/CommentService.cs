using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Repository.Repositories.Interface;
using Service.DTOs.Comment;
using Service.Helpers.Exceptions;
using Service.Services.Interfaces;

namespace Service.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CommentService> _logger;

        public CommentService(ICommentRepository commentRepository,
                              IMapper mapper,
                              ILogger<CommentService> logger)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task CreateAsync(CommentCreateDto request, string userId)
        {
            if(string.IsNullOrWhiteSpace(request.Content))
            {
                _logger.LogWarning("Comment content is empty for user {UserId}", userId);
                throw new InvalidCommentException("Comment cannot be empty.");
            }

            if (request.MovieId == null && request.SeriesId == null)
            {
                _logger.LogWarning("Neither MovieId nor SeriesId provIded by user {UserId}", userId);
                throw new InvalidCommentException("The comment must be related to either a movie or a series.");
            }

            if (request.MovieId != null && request.SeriesId != null)
            {
                _logger.LogWarning("Both MovieId and SeriesId provIded. UserId: {UserId}", userId);
                throw new InvalidCommentException("Comments cannot be added to both a movie and a series at the same time.");
            }

            var comment = _mapper.Map<Comment>(request);
            comment.UserId = userId;

            await _commentRepository.CreateAsync(comment);
            _logger.LogInformation("Comment created by user {UserId} for {Target}", userId, request.MovieId != null ? $"Movie {request.MovieId}" : $"Series {request.SeriesId}");
        }

        public async Task DeleteAsync(int Id, string userId)
        {
            var comment = await _commentRepository.GetByIdAsync(Id);

            if (comment == null)
            {
                _logger.LogWarning("Delete failed: Comment with Id {CommentId} not found.", Id);
                throw new KeyNotFoundException("Comment not found");
            }

            if (comment.UserId != userId)
            {
                _logger.LogWarning("Unauthorized delete attempt: User {UserId} tried to delete comment {CommentId} owned by {OwnerId}.", userId, Id, comment.UserId);
                throw new UnauthorizedAccessException("You can only delete your own comment.");
            }

            await _commentRepository.DeleteAsync(comment);

            _logger.LogInformation("Comment {CommentId} deleted by user {UserId}.", Id, userId);
        }

        public async Task<IEnumerable<CommentDto>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<CommentDto>>(await _commentRepository.GetAllAsync());
        }

        public async Task<CommentDto> GetByIdAsync(int Id)
        {
            var comment = await _commentRepository.GetByIdAsync(Id);

            if (comment == null)
            {
                _logger.LogWarning("GetById failed: Comment with Id {CommentId} not found.", Id);
                throw new KeyNotFoundException("Comment not found");
            }

            _logger.LogInformation("Comment with Id {CommentId} retrieved successfully.", Id);
            return _mapper.Map<CommentDto>(comment);
        }

        public async Task<IEnumerable<CommentDto>> GetByMovieIdAsync(int movieId)
        {
            var movieComments = await _commentRepository.GetByMovieIdAsync(movieId);

            if (movieComments == null || !movieComments.Any())
            {
                _logger.LogInformation("No comments found for movie with Id {MovieId}.", movieId);
                return Enumerable.Empty<CommentDto>();
            }

            _logger.LogInformation("{Count} comments found for movie with Id {MovieId}.", movieComments.Count(), movieId);

            return _mapper.Map<IEnumerable<CommentDto>>(movieComments);
        }

        public async Task<IEnumerable<CommentDto>> GetByUserIdAsync(string userId)
        {
            var userComments = await _commentRepository.GetByUserIdAsync(userId);

            if (userComments == null || !userComments.Any())
            {
                _logger.LogInformation("No comments found for user with Id {UserId}.", userId);
                return Enumerable.Empty<CommentDto>();
            }

            _logger.LogInformation("{Count} comments found for user with Id {UserId}.", userComments.Count(), userId);

            return _mapper.Map<IEnumerable<CommentDto>>(userComments);
        }

        public async Task UpdateAsync(int Id, CommentUpdateDto request, string userId)
        {
            var existingComment = await _commentRepository.GetByIdAsync(Id);

            if (existingComment == null)
            {
                _logger.LogWarning("Update failed: Comment with Id {CommentId} not found.", Id);
                throw new KeyNotFoundException("Comment not found");
            }

            if (existingComment.UserId != userId)
            {
                _logger.LogWarning("Unauthorized edit attempt: User {UserId} tried to edit comment {CommentId} owned by {OwnerId}.", userId, Id, existingComment.UserId);
                throw new UnauthorizedAccessException("You can only edit your own comment.");
            }

            if (!string.IsNullOrWhiteSpace(request.Content))
            {
                existingComment.Content = request.Content;
                await _commentRepository.UpdateAsync(existingComment);
                _logger.LogInformation("Comment with Id {CommentId} updated by user {UserId}.", Id, userId);
            }
        }
    }
}
