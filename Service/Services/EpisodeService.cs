using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Repository.Repositories;
using Repository.Repositories.Interface;
using Service.DTOs.Episode;
using Service.DTOs.Series;
using Service.Helpers.Responses;
using Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class EpisodeService : IEpisodeService
    {
        private readonly IEpisodeRepository _episodeRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<EpisodeService> _logger;

        public EpisodeService(IEpisodeRepository episodeRepository,
                              IMapper mapper,
                              ILogger<EpisodeService> logger)
        {
            _episodeRepository = episodeRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task CreateAsync(EpisodeCreateDto episode)
        {
            if (episode == null)
            {
                _logger.LogWarning("Episode data not provided in CreateAsync");
                throw new ArgumentNullException(nameof(episode), "Episode data is required");
            }

            if (episode.SeriesId <= 0)
            {
                _logger.LogWarning("Invalid series Id provided: {SeriesId}", episode.SeriesId);
                throw new ArgumentException("Series Id must be greater than 0");
            }

            if (episode.SeasonNumber <= 0)
            {
                _logger.LogWarning("Invalid season number provided: {SeasonNumber}", episode.SeasonNumber);
                throw new ArgumentException("Season number must be greater than 0");
            }

            if (episode.EpisodeNumber <= 0)
            {
                _logger.LogWarning("Invalid episode number provided: {EpisodeNumber}", episode.EpisodeNumber);
                throw new ArgumentException("Episode number must be greater than 0");
            }

            _logger.LogInformation("Creating episode {EpisodeNumber} for season {SeasonNumber} in series {SeriesId}",
                                  episode.EpisodeNumber, episode.SeasonNumber, episode.SeriesId);

            var existingEpisode = await _episodeRepository.GetFirstOrDefaultAsync(e => e.SeriesId == episode.SeriesId &&
                                                                                       e.SeasonNumber == episode.SeasonNumber &&
                                                                                       e.EpisodeNumber == episode.EpisodeNumber);
            if (existingEpisode != null)
            {
                _logger.LogWarning("Episode {EpisodeNumber} already exists in Season {SeasonNumber} for series {SeriesId}",
                                  episode.EpisodeNumber, episode.SeasonNumber, episode.SeriesId);
                throw new InvalidOperationException($"Episode {episode.EpisodeNumber} already exists in Season {episode.SeasonNumber} for this series.");
            }

            try
            {
                var model = _mapper.Map<Episode>(episode);
                await _episodeRepository.CreateAsync(model);
                _logger.LogInformation("Episode created successfully: Series {SeriesId}, Season {SeasonNumber}, Episode {EpisodeNumber}",
                                      episode.SeriesId, episode.SeasonNumber, episode.EpisodeNumber);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating episode for series {SeriesId}", episode.SeriesId);
                throw new InvalidOperationException("Failed to create episode", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Invalid episode Id provided: {Id}", id);
                throw new ArgumentException("Episode Id must be greater than 0");
            }

            _logger.LogInformation("Attempting to delete episode with Id: {Id}", id);

            var episode = await _episodeRepository.GetByIdAsync(id);

            if (episode == null)
            {
                _logger.LogWarning("Episode not found with Id: {Id}", id);
                throw new KeyNotFoundException($"Episode with Id {id} not found");
            }

            try
            {
                await _episodeRepository.DeleteAsync(episode);
                _logger.LogInformation("Episode deleted successfully: Id {Id}, Title: {Title}", id, episode.Title);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting episode with Id: {Id}", id);
                throw new InvalidOperationException("Failed to delete episode", ex);
            }
        }

        public async Task<IEnumerable<EpisodeDto>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<EpisodeDto>>(await _episodeRepository.GetAllAsync());
        }

        public async  Task<PaginateResponse<EpisodeDto>> GetAllPaginatedAsync(int page, int take = 3)
        {
            var paginatedDatas = _mapper.Map<List<EpisodeDto>>(await _episodeRepository.GetAllPaginatedAsync(page, take));

            int count = await _episodeRepository.GetCountAsync();

            int totalPage = (int)Math.Ceiling((double)count / take);

            return new PaginateResponse<EpisodeDto>(paginatedDatas, page, totalPage);
        }

        public async Task<EpisodeDto> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Invalid episode Id provided: {Id}", id);
                throw new ArgumentException("Episode Id must be greater than 0");
            }

            _logger.LogInformation("Retrieving episode with Id: {Id}", id);

            var episode = await _episodeRepository.GetByIdAsync(id);

            if (episode == null)
            {
                _logger.LogWarning("Episode not found with Id: {Id}", id);
                throw new KeyNotFoundException($"Episode with Id {id} not found");
            }

            _logger.LogInformation("Episode retrieved successfully: Id {Id}, Title: {Title}", id, episode.Title);
            return _mapper.Map<EpisodeDto>(episode);
        }

        public async Task<IEnumerable<EpisodeDto>> GetBySeriesIdAsync(int seriesId)
        {
            if (seriesId <= 0)
            {
                _logger.LogWarning("Invalid series Id provided: {SeriesId}", seriesId);
                throw new ArgumentException("Series Id must be greater than 0");
            }

            _logger.LogInformation("Retrieving episodes for series with Id: {SeriesId}", seriesId);

            var episodes = await _episodeRepository.GetBySeriesIdAsync(seriesId);

            if (episodes == null || !episodes.Any())
            {
                _logger.LogInformation("No episodes found for series Id: {SeriesId}", seriesId);
                throw new KeyNotFoundException($"No episodes found for series with Id {seriesId}");
            }

            var result = _mapper.Map<IEnumerable<EpisodeDto>>(episodes);
            _logger.LogInformation("Retrieved {Count} episodes for series Id: {SeriesId}", result.Count(), seriesId);

            return result;
        }

        public async Task<IEnumerable<EpisodeDto>> SearchAsync(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                _logger.LogWarning("Search text was null or whitespace.");
                throw new ArgumentException("Search text must not be empty.");
            }

            var episodes = await _episodeRepository.SearchAsync(searchText);

            if (episodes == null || !episodes.Any())
            {
                _logger.LogInformation("No episodes found for search text: {SearchText}", searchText);
                throw new KeyNotFoundException("No episodes found matching the search criteria.");
            }

            return _mapper.Map<IEnumerable<EpisodeDto>>(episodes);
        }

        public async Task<IEnumerable<EpisodeDto>> SortByCreateDateAsync(string order)
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

            var episodes = await _episodeRepository.SortByCreateDateAsync(order);
            if (episodes == null || !episodes.Any())
            {
                _logger.LogInformation("No episodes found for the given sort order: {Order}", order);
                throw new KeyNotFoundException("No episodes found for the given sorting criteria.");
            }

            return _mapper.Map<IEnumerable<EpisodeDto>>(episodes);
        }

        public async Task UpdateAsync(int id, EpisodeUpdateDto episode)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Invalid episode Id provided: {Id}", id);
                throw new ArgumentException("Episode Id must be a positive integer.");
            }

            if (episode == null)
            {
                _logger.LogWarning("Episode update data is null for Id: {Id}", id);
                throw new ArgumentNullException(nameof(episode), "Episode update data cannot be null.");
            }

            var existingEpisode = await _episodeRepository.GetByIdAsync(id);
            if (existingEpisode == null)
            {
                _logger.LogWarning("Episode not found with Id: {Id}", id);
                throw new KeyNotFoundException($"Episode with Id {id} not found.");
            }

            _logger.LogInformation("Updating episode with Id: {Id}", id);

            if (episode.SeriesId.HasValue)
                existingEpisode.SeriesId = episode.SeriesId.Value;

            if (episode.SeasonNumber.HasValue)
                existingEpisode.SeasonNumber = episode.SeasonNumber.Value;

            if (episode.EpisodeNumber.HasValue)
                existingEpisode.EpisodeNumber = episode.EpisodeNumber.Value;

            if (!string.IsNullOrWhiteSpace(episode.Title))
                existingEpisode.Title = episode.Title;

            if (episode.ReleaseDate.HasValue)
                existingEpisode.ReleaseDate = episode.ReleaseDate.Value;

            if (episode.Duration.HasValue)
                existingEpisode.Duration = episode.Duration.Value;

            try
            {
                await _episodeRepository.UpdateAsync(existingEpisode);
                _logger.LogInformation("Episode successfully updated with Id: {Id}", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating episode with Id: {Id}", id);
                throw;
            }
        }
    }
}
