using AutoMapper;
using Domain.Entities;
using Domain.Entities.Join;
using Microsoft.Extensions.Logging;
using Repository.Repositories;
using Repository.Repositories.Interface;
using Service.DTOs.Movie;
using Service.DTOs.Series;
using Service.Helpers.Responses;
using Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class SeriesService : ISeriesService
    {
        private readonly ISeriesRepository _seriesRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<SeriesService> _logger;

        public SeriesService(ISeriesRepository seriesRepository,
                             IMapper mapper,
                             ILogger<SeriesService> logger)
        {
            _seriesRepository = seriesRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task CreateAsync(SeriesCreateDto series, string webRootPath)
        {
            if (series == null)
            {
                _logger.LogWarning("Series creation failed: input is null.");
                throw new ArgumentNullException(nameof(series), "Series data must not be null.");
            }

            if (series.UploadPoster == null || string.IsNullOrEmpty(series.UploadPoster.FileName))
            {
                _logger.LogWarning("Series creation failed: no poster provided.");
                throw new ArgumentException("Poster file is required for creating a series.");
            }

            string fileName = Guid.NewGuid().ToString() + "-" + series.UploadPoster.FileName;
            string path = Path.Combine(webRootPath, "images", fileName);

            try
            {
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    await series.UploadPoster.CopyToAsync(stream);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while saving the poster for series.");
                throw new IOException("An error occurred while uploading the poster.");
            }

            var model = _mapper.Map<Series>(series);
            model.PosterUrl = fileName;

            await _seriesRepository.CreateAsync(model);

            _logger.LogInformation("Series '{Title}' successfully created with Id: {Id}", model.Title, model.Id);
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Invalid series Id provided: {Id}", id);
                throw new ArgumentException("Series Id must be a positive integer.");
            }

            var series = await _seriesRepository.GetByIdAsync(id);
            if (series == null)
            {
                _logger.LogWarning("Series not found with Id: {Id}", id);
                throw new KeyNotFoundException($"Series with Id {id} not found.");
            }

            await _seriesRepository.DeleteAsync(series);

            _logger.LogInformation("Series with Id {Id} and title '{Title}' successfully deleted.", id, series.Title);
        }

        public async Task<IEnumerable<SeriesDto>> FilterByReleaseYearAsync(int year)
        {
            if (year <= 1800 || year > DateTime.Now.Year)
            {
                _logger.LogWarning("Invalid release year provided: {Year}", year);
                throw new ArgumentException("Invalid release year provided.");
            }

            var seriesList = await _seriesRepository.FilterByReleaseYearAsync(year);

            if (!seriesList.Any())
            {
                _logger.LogInformation("No series found for release year: {Year}", year);
            }

            return _mapper.Map<IEnumerable<SeriesDto>>(seriesList);
        }

        public async Task<IEnumerable<SeriesDto>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<SeriesDto>>(await _seriesRepository.GetAllAsync());
        }

        public async Task<PaginateResponse<SeriesDto>> GetAllPaginatedAsync(int page, int take = 3)
        {
            var paginatedDatas = _mapper.Map<List<SeriesDto>>(await _seriesRepository.GetAllPaginatedAsync(page, take));

            int count = await _seriesRepository.GetCountAsync();

            int totalPage = (int)Math.Ceiling((double)count / take);

            return new PaginateResponse<SeriesDto>(paginatedDatas, page, totalPage);
        }

        public async Task<IEnumerable<SeriesDto>> GetByActorIdAsync(int actorId)
        {
            if (actorId <= 0)
            {
                _logger.LogWarning("Invalid actor Id provided: {ActorId}", actorId);
                throw new ArgumentException("Actor Id must be a positive integer.");
            }

            var series = await _seriesRepository.GetByActorIdAsync(actorId);

            if (!series.Any())
            {
                _logger.LogInformation("No series found for actor Id: {ActorId}", actorId);
            }
            else
            {
                _logger.LogInformation("{Count} series found for actor Id: {ActorId}", series.Count(), actorId);
            }

            return _mapper.Map<IEnumerable<SeriesDto>>(series);
        }

        public async Task<IEnumerable<SeriesDto>> GetByDirectorIdAsync(int directorId)
        {
            if (directorId <= 0)
            {
                _logger.LogWarning("Invalid director Id provided: {DirectorId}", directorId);
                throw new ArgumentException("Director Id must be a positive integer.");
            }

            var series = await _seriesRepository.GetByDirectorIdAsync(directorId);

            if (!series.Any())
            {
                _logger.LogInformation("No series found for director Id: {DirectorId}", directorId);
            }
            else
            {
                _logger.LogInformation("{Count} series found for director Id: {DirectorId}", series.Count(), directorId);
            }

            return _mapper.Map<IEnumerable<SeriesDto>>(series);

        }

        public async Task<IEnumerable<SeriesDto>> GetByGenreIdAsync(int genreId)
        {
            if (genreId <= 0)
            {
                _logger.LogWarning("Invalid genre Id provided: {GenreId}", genreId);
                throw new ArgumentException("Genre Id must be a positive integer.");
            }

            var series = await _seriesRepository.GetByGenreIdAsync(genreId);

            if (!series.Any())
            {
                _logger.LogInformation("No series found for genre Id: {GenreId}", genreId);
            }
            else
            {
                _logger.LogInformation("{Count} series found for genre Id: {GenreId}", series.Count(), genreId);
            }

            return _mapper.Map<IEnumerable<SeriesDto>>(series);

        }

        public async Task<SeriesDto> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Invalid series Id provided: {SeriesId}", id);
                throw new ArgumentException("Series Id must be a positive integer.");
            }

            var series = await _seriesRepository.GetByIdAsync(id);

            if (series == null)
            {
                _logger.LogWarning("Series not found with Id: {SeriesId}", id);
                throw new KeyNotFoundException($"Series with Id {id} not found.");
            }

            _logger.LogInformation("Series with Id {SeriesId} retrieved successfully", id);

            return _mapper.Map<SeriesDto>(series);
        }

        public async Task<IEnumerable<SeriesDto>> SearchAsync(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                _logger.LogWarning("Empty or null search text provided.");
                throw new ArgumentException("Search text must not be empty.");
            }

            var series = await _seriesRepository.SearchAsync(searchText);

            if (!series.Any())
            {
                _logger.LogInformation("No series found matching search text: {SearchText}", searchText);
            }
            else
            {
                _logger.LogInformation("{Count} series found for search text: {SearchText}", series.Count(), searchText);
            }

            return _mapper.Map<IEnumerable<SeriesDto>>(series);
        }

        public async Task<IEnumerable<SeriesDto>> SortByCreateDateAsync(string order)
        {
            if (string.IsNullOrWhiteSpace(order))
            {
                _logger.LogWarning("Sort order is null or empty.");
                throw new ArgumentException("Sort order must be provided.");
            }

            var normalizedOrder = order.Trim().ToLower();

            if (normalizedOrder != "asc" && normalizedOrder != "desc")
            {
                _logger.LogWarning("Invalid sort order provided: {Order}", order);
                throw new ArgumentException("Sort order must be either 'asc' or 'desc'.");
            }

            var sortedSeries = await _seriesRepository.SortByCreatedDateAsync(normalizedOrder);

            _logger.LogInformation("Series sorted by create date in {Order} order.", normalizedOrder);

            return _mapper.Map<IEnumerable<SeriesDto>>(sortedSeries);
        }

        public async Task UpdateAsync(int id, SeriesUpdateDto series, string? webRootPath)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Invalid series Id provided: {Id}", id);
                throw new ArgumentException("Series Id must be a positive integer.");
            }

            var existingSeries = await _seriesRepository.GetByIdAsync(id);

            if (existingSeries == null)
            {
                _logger.LogWarning("Series not found with Id: {Id}", id);
                throw new KeyNotFoundException($"Series with Id {id} not found.");
            }

            _logger.LogInformation("Updating series with Id: {Id}", id);

            if (!string.IsNullOrEmpty(series.Title))
                existingSeries.Title = series.Title;

            if (!string.IsNullOrEmpty(series.Description))
                existingSeries.Description = series.Description;

            if (series.ReleaseDate.HasValue)
                existingSeries.ReleaseDate = series.ReleaseDate.Value;

            if (series.GenresId != null)
            {
                series.GenresId.Clear();
                foreach (var genreId in series.GenresId)
                {
                    existingSeries.SeriesGenres.Add(new SeriesGenre
                    {
                        GenreId = genreId,
                        SeriesId = existingSeries.Id
                    });
                }
            }
            if (series.DirectorsId != null)
            {
                series.DirectorsId.Clear();
                foreach (var directorId in series.DirectorsId)
                {
                    existingSeries.SeriesDirectors.Add(new SeriesDirector
                    {
                        DirectorId = directorId,
                        SeriesId = existingSeries.Id
                    });
                }
            }

            if (series.Actors != null)
            {
                foreach (var actor in series.Actors)
                {
                    var existingActorRelation = existingSeries.SeriesActors
                        .FirstOrDefault(ma => ma.ActorId == actor.ActorId);

                    if (existingActorRelation != null)
                    {
                        if (!string.IsNullOrEmpty(actor.CharacterName))
                            existingActorRelation.CharacterName = actor.CharacterName;
                    }
                    else if (actor.ActorId != null)
                    {
                        existingSeries.SeriesActors.Add(new SeriesActor
                        {
                            ActorId = actor.ActorId.Value,
                            SeriesId = existingSeries.Id,
                            CharacterName = actor.CharacterName
                        });
                    }
                }
            }

            if (series.UploadPoster != null && !string.IsNullOrEmpty(series.UploadPoster.FileName))
            {
                if (string.IsNullOrEmpty(webRootPath))
                {
                    _logger.LogError("Web root path is null. Cannot save poster image.");
                    throw new InvalidOperationException("Web root path is required to save the poster.");
                }

                string fileName = Guid.NewGuid() + "-" + series.UploadPoster.FileName;
                string imagePath = Path.Combine(webRootPath, "images", fileName);

                using (FileStream stream = new FileStream(imagePath, FileMode.Create))
                {
                    await series.UploadPoster.CopyToAsync(stream);
                }

                existingSeries.PosterUrl = fileName;
            }

            await _seriesRepository.UpdateAsync(existingSeries);

            _logger.LogInformation("Series with Id {Id} successfully updated.", id);
        }
    }
}
