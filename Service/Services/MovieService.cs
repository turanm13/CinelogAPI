using AutoMapper;
using Domain.Entities;
using Domain.Entities.Join;
using Microsoft.Extensions.Logging;
using Repository.Repositories;
using Repository.Repositories.Interface;
using Service.DTOs.Actor;
using Service.DTOs.Movie;
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
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<MovieService> _logger;
        public MovieService(IMovieRepository movieRepository,
                            IMapper mapper,
                            ILogger<MovieService> logger)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task CreateAsync(MovieCreateDto movie, string webRootPath)
        {
            if (movie == null)
            {
                _logger.LogWarning("MovieCreateDto is null");
                throw new ArgumentNullException(nameof(movie), "Movie data cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(webRootPath))
            {
                _logger.LogWarning("WebRootPath is null or empty");
                throw new ArgumentException("WebRootPath cannot be null or empty.");
            }

            if (movie.UploadPoster == null)
            {
                _logger.LogWarning("Movie poster is null for title: '{Title}'", movie.Title);
                throw new ArgumentException("Movie poster cannot be null.");
            }

            string fileName = Guid.NewGuid().ToString() + "-" + movie.UploadPoster.FileName;
            string path = Path.Combine(webRootPath, "images", fileName);

            _logger.LogInformation("Creating movie '{Title}' with poster file: '{FileName}'", movie.Title, fileName);

            string imagesDir = Path.Combine(webRootPath, "images");
            if (!Directory.Exists(imagesDir))
            {
                Directory.CreateDirectory(imagesDir);
                _logger.LogInformation("Created images directory: '{Directory}'", imagesDir);
            }

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await movie.UploadPoster.CopyToAsync(stream);
            }

            var model = _mapper.Map<Movie>(movie);
            model.PosterUrl = fileName;
            await _movieRepository.CreateAsync(model);

            _logger.LogInformation("Movie '{Title}' successfully created with poster: '{PosterUrl}'", movie.Title, fileName);
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Invalid movie Id provided: {Id}", id);
                throw new ArgumentException("Movie Id must be a positive integer.");
            }

            var movie = await _movieRepository.GetByIdAsync(id);
            if (movie == null)
            {
                _logger.LogWarning("Movie not found with Id: {Id}", id);
                throw new KeyNotFoundException($"Movie with Id {id} not found.");
            }

            await _movieRepository.DeleteAsync(movie);

            _logger.LogInformation("Movie with Id {Id} and title '{MovieTitle}' successfully deleted", id, movie.Title);
        }

        public async Task<IEnumerable<MovieDto>> FilterByReleaseYearAsync(int year)
        {

            if (year <= 1800 || year > DateTime.UtcNow.Year)
            {
                _logger.LogWarning("Invalid release year provided: {Year}", year);
                throw new ArgumentException("Provided year is out of acceptable range.");
            }

            var movies = await _movieRepository.FilterByReleaseYearAsync(year);

            if (!movies.Any())
            {
                _logger.LogInformation("No movies found for the year: {Year}", year);
            }
            else
            {
                _logger.LogInformation("{Count} movie(s) found for the year: {Year}", movies.Count(), year);
            }

            return _mapper.Map<IEnumerable<MovieDto>>(movies);
        }

        public async Task<IEnumerable<MovieDto>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<MovieDto>>(await _movieRepository.GetAllAsync());
        }

        public async Task<PaginateResponse<MovieDto>> GetAllPaginatedAsync(int page, int take = 20)
        {
            var paginatedDatas = _mapper.Map<List<MovieDto>>(await _movieRepository.GetAllPaginatedAsync(page, take));

            int count = await _movieRepository.GetCountAsync();

            int totalPage = (int)Math.Ceiling((double)count / take);

            return new PaginateResponse<MovieDto>(paginatedDatas, page, totalPage);
        }

        public async Task<IEnumerable<MovieDto>> GetByActorIdAsync(int actorId)
        {
            if (actorId <= 0)
            {
                _logger.LogWarning("Invalid actor Id provided: {ActorId}", actorId);
                throw new ArgumentException("Actor Id must be a positive integer.");
            }

            var movies = await _movieRepository.GetByActorIdAsync(actorId);

            if (!movies.Any())
            {
                _logger.LogInformation("No movies found for actor Id: {ActorId}", actorId);
            }
            else
            {
                _logger.LogInformation("{Count} movie(s) found for actor Id: {ActorId}", movies.Count(), actorId);
            }

            return _mapper.Map<IEnumerable<MovieDto>>(movies);
        }

        public async Task<IEnumerable<MovieDto>> GetByDirectorIdAsync(int directorId)
        {
            if (directorId <= 0)
            {
                _logger.LogWarning("Invalid director Id provided: {DirectorId}", directorId);
                throw new ArgumentException("Director Id must be a positive integer.");
            }

            var movies = await _movieRepository.GetByDirectorIdAsync(directorId);

            if (!movies.Any())
            {
                _logger.LogInformation("No movies found for director Id: {DirectorId}", directorId);
            }
            else
            {
                _logger.LogInformation("{Count} movie(s) found for director Id: {DirectorId}", movies.Count(), directorId);
            }

            return _mapper.Map<IEnumerable<MovieDto>>(movies);
        }

        public async Task<IEnumerable<MovieDto>> GetByGenreIdAsync(int genreId)
        {
            if (genreId <= 0)
            {
                _logger.LogWarning("Invalid genre Id provided: {GenreId}", genreId);
                throw new ArgumentException("Genre Id must be a positive integer.");
            }

            var movies = await _movieRepository.GetByGenreIdAsync(genreId);

            if (!movies.Any())
            {
                _logger.LogInformation("No movies found for genre Id: {GenreId}", genreId);
            }
            else
            {
                _logger.LogInformation("{Count} movie(s) found for genre Id: {GenreId}", movies.Count(), genreId);
            }

            return _mapper.Map<IEnumerable<MovieDto>>(movies);
        }

        public async Task<MovieDto> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Invalid movie Id provided: {MovieId}", id);
                throw new ArgumentException("Movie Id must be a positive integer.");
            }

            var movie = await _movieRepository.GetByIdAsync(id);

            if (movie == null)
            {
                _logger.LogWarning("Movie not found with Id: {MovieId}", id);
                throw new KeyNotFoundException($"Movie with Id {id} not found.");
            }

            _logger.LogInformation("Movie with Id {MovieId} retrieved successfully", id);

            return _mapper.Map<MovieDto>(movie);
        }

        public async Task<IEnumerable<MovieDto>> SearchAsync(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                _logger.LogWarning("Empty or null search text provided.");
                throw new ArgumentException("Search text must not be empty.");
            }

            var movies = await _movieRepository.SearchAsync(searchText);

            if (!movies.Any())
            {
                _logger.LogInformation("No movies found matching search text: {SearchText}", searchText);
            }
            else
            {
                _logger.LogInformation("{Count} movie(s) found for search text: {SearchText}", movies.Count(), searchText);
            }

            return _mapper.Map<IEnumerable<MovieDto>>(movies);
        }

        public async Task<IEnumerable<MovieDto>> SortByCreateDateAsync(string order)
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

            var sortedMovies = await _movieRepository.SortByCreatedDateAsync(normalizedOrder);

            _logger.LogInformation("Movies sorted by create date in {Order} order.", normalizedOrder);

            return _mapper.Map<IEnumerable<MovieDto>>(sortedMovies);
        }

        public async Task UpdateAsync(int id, MovieUpdateDto movie, string? webRootPath)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Invalid movie Id provided: {Id}", id);
                throw new ArgumentException("Movie Id must be a positive integer.");
            }

            var existingMovie = await _movieRepository.GetByIdAsync(id);
            if (existingMovie == null)
            {
                _logger.LogWarning("Movie not found with Id: {Id}", id);
                throw new KeyNotFoundException($"Movie with Id {id} not found.");
            }

            _logger.LogInformation("Updating movie with Id: {Id}", id);

            if (!string.IsNullOrEmpty(movie.Title))
                existingMovie.Title = movie.Title;

            if (!string.IsNullOrEmpty(movie.Description))
                existingMovie.Description = movie.Description;

            if (movie.ReleaseDate.HasValue)
                existingMovie.ReleaseDate = movie.ReleaseDate.Value;

            if (movie.Duration.HasValue)
                existingMovie.Duration = movie.Duration.Value;

            if (movie.GenresId != null && movie.GenresId.Any())
            {
                existingMovie.MovieGenres.Clear();
                foreach (var genreId in movie.GenresId)
                {
                    existingMovie.MovieGenres.Add(new MovieGenre
                    {
                        GenreId = genreId,
                        MovieId = existingMovie.Id
                    });
                }
            }

            if (movie.DirectorsId != null && movie.DirectorsId.Any())
            {
                existingMovie.MovieDirectors.Clear();
                foreach (var directorId in movie.DirectorsId)
                {
                    existingMovie.MovieDirectors.Add(new MovieDirector
                    {
                        DirectorId = directorId,
                        MovieId = existingMovie.Id
                    });
                }
            }

            if (movie.Actors != null && movie.Actors.Any())
            {
                foreach (var actor in movie.Actors)
                {
                    var existingActorRelation = existingMovie.MovieActors
                        .FirstOrDefault(ma => ma.ActorId == actor.ActorId);

                    if (existingActorRelation != null)
                    {
                        if (!string.IsNullOrEmpty(actor.CharacterName))
                            existingActorRelation.CharacterName = actor.CharacterName;
                    }
                    else if (actor.ActorId != null)
                    {
                        existingMovie.MovieActors.Add(new MovieActor
                        {
                            ActorId = actor.ActorId.Value,
                            MovieId = existingMovie.Id,
                            CharacterName = actor.CharacterName
                        });
                    }
                }
            }

            if (movie.UploadPoster != null && !string.IsNullOrEmpty(movie.UploadPoster.FileName))
            {
                if (string.IsNullOrEmpty(webRootPath))
                {
                    _logger.LogError("Web root path is null. Cannot save poster image.");
                    throw new InvalidOperationException("Web root path is required to save the poster.");
                }

                string fileName = Guid.NewGuid() + "-" + movie.UploadPoster.FileName;
                string imagePath = Path.Combine(webRootPath, "images", fileName);

                using (FileStream stream = new FileStream(imagePath, FileMode.Create))
                {
                    await movie.UploadPoster.CopyToAsync(stream);
                }

                existingMovie.PosterUrl = fileName;
            }

            await _movieRepository.UpdateAsync(existingMovie);

            _logger.LogInformation("Movie with Id {Id} successfully updated.", id);
        }
    }
}
