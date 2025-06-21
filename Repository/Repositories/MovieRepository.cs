using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class MovieRepository : BaseRepository<Movie>, IMovieRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<Movie> _movies;
        public MovieRepository(AppDbContext context) : base(context)
        {
            _context = context;
            _movies = context.Movies;
        }

        public async Task<IEnumerable<Movie>> FilterByReleaseYearAsync(int year)
        {
            return await _movies.Include(m => m.MovieGenres)
                                .ThenInclude(mg => mg.Genre)
                                .Include(m => m.MovieDirectors)
                                .ThenInclude(md => md.Director)
                                .Include(m => m.MovieActors)
                                .ThenInclude(ma => ma.Actor)
                                .Include(m=>m.Comments)
                                .Include(m=>m.Ratings)
                                .Where(m => m.ReleaseDate.Year == year)
                                .ToListAsync();
        }

        public async Task<IEnumerable<Movie>> GetAllAsync()
        {
            return await _movies.Include(m => m.MovieGenres)
                                .ThenInclude(mg => mg.Genre)
                                .Include(m => m.MovieDirectors)
                                .ThenInclude(md => md.Director)
                                .Include(m => m.MovieActors)
                                .ThenInclude(ma => ma.Actor)
                                .Include(m => m.Comments)
                                .ThenInclude(m => m.User)
                                .Include(m => m.Ratings)
                                .ToListAsync();
        }

        public async Task<IEnumerable<Movie>> GetAllPaginatedAsync(int page, int take)
        {
            return await _movies.Include(m => m.MovieGenres)
                                .ThenInclude(mg => mg.Genre)
                                .Include(m => m.MovieDirectors)
                                .ThenInclude(md => md.Director)
                                .Include(m => m.MovieActors)
                                .ThenInclude(ma => ma.Actor)
                                .Include(m => m.Comments)
                                .ThenInclude(m => m.User)
                                .Include(m => m.Ratings)
                                .Skip(page * take - take)
                                .Take(take).ToListAsync();
        }

        public async Task<IEnumerable<Movie>> GetByActorIdAsync(int actorId)
        {
            return await _movies.Include(m => m.MovieGenres)
                                .ThenInclude(mg => mg.Genre)
                                .Include(m => m.MovieDirectors)
                                .ThenInclude(md => md.Director)
                                .Include(m => m.MovieActors)
                                .ThenInclude(ma => ma.Actor)
                                .Include(m => m.Comments)
                                .ThenInclude(m => m.User)
                                .Include(m => m.Ratings)
                                .Where(m => m.MovieActors
                                .Any(m => m.ActorId == actorId))
                                .ToListAsync();
        }

        public async Task<IEnumerable<Movie>> GetByDirectorIdAsync(int directorId)
        {
            return await _movies.Include(m => m.MovieGenres)
                                .ThenInclude(mg => mg.Genre)
                                .Include(m => m.MovieDirectors)
                                .ThenInclude(md => md.Director)
                                .Include(m => m.MovieActors)
                                .ThenInclude(ma => ma.Actor)
                                .Include(m => m.Comments)
                                .ThenInclude(m => m.User)
                                .Include(m => m.Ratings)
                                .Where(m => m.MovieDirectors
                                .Any(m => m.DirectorId == directorId))
                                .ToListAsync();
        }

        public async Task<IEnumerable<Movie>> GetByGenreIdAsync(int genreId)
        {
            return await _movies.Include(m => m.MovieGenres)
                                .ThenInclude(mg => mg.Genre)
                                .Include(m => m.MovieDirectors)
                                .ThenInclude(md => md.Director)
                                .Include(m => m.MovieActors)
                                .ThenInclude(ma => ma.Actor)
                                .Include(m => m.Comments)
                                .ThenInclude(m => m.User)
                                .Include(m => m.Ratings)
                                .Where(m => m.MovieGenres
                                .Any(m => m.GenreId == genreId))
                                .ToListAsync();
        }

        public async Task<Movie> GetByIdAsync(int id)
        {
            return await _movies.Include(m => m.MovieGenres)
                                .ThenInclude(mg => mg.Genre)
                                .Include(m => m.MovieDirectors)
                                .ThenInclude(md => md.Director)
                                .Include(m => m.MovieActors)
                                .ThenInclude(ma => ma.Actor)
                                .Include(m => m.Comments)
                                .ThenInclude(m => m.User)
                                .Include(m => m.Ratings)
                                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<Movie>> SearchAsync(string searchText)
        {
            return _movies.Include(m => m.MovieGenres)
                          .ThenInclude(mg => mg.Genre)
                          .Include(m => m.MovieDirectors)
                          .ThenInclude(md => md.Director)
                          .Include(m => m.MovieActors)
                          .ThenInclude(ma => ma.Actor)
                          .Include(m => m.Comments)
                          .ThenInclude(m => m.User)
                          .Include(m => m.Ratings)
                          .Where(m => m.Title.Contains(searchText) || m.MovieGenres
                          .Any(mg => mg.Genre.Name.Contains(searchText)));
        }

        public async Task<IEnumerable<Movie>> SortByCreatedDateAsync(string order)
        {
            if (string.IsNullOrWhiteSpace(order))
                return await _movies.Include(m => m.MovieGenres)
                                    .ThenInclude(mg => mg.Genre)
                                    .Include(m => m.MovieDirectors)
                                    .ThenInclude(md => md.Director)
                                    .Include(m => m.MovieActors)
                                    .ThenInclude(ma => ma.Actor)
                                    .Include(m => m.Comments)
                                    .ThenInclude(m => m.User)
                                    .Include(m => m.Ratings)
                                    .ToListAsync();

            return order.ToLower() switch
            {
                "asc" => await _movies.Include(m => m.MovieGenres)
                                      .ThenInclude(mg => mg.Genre)
                                      .Include(m => m.MovieDirectors)
                                      .ThenInclude(md => md.Director)
                                      .Include(m => m.MovieActors)
                                      .ThenInclude(ma => ma.Actor)
                                      .Include(m => m.Comments)
                                      .ThenInclude(m => m.User)
                                      .Include(m => m.Ratings)
                                      .OrderBy(s => s.CreatedDate)
                                      .ToListAsync(),

                "desc" => await _movies.Include(m => m.MovieGenres)
                                       .ThenInclude(mg => mg.Genre)
                                       .Include(m => m.MovieDirectors)
                                       .ThenInclude(md => md.Director)
                                       .Include(m => m.MovieActors)
                                       .ThenInclude(ma => ma.Actor)
                                       .Include(m => m.Comments)
                                       .ThenInclude(m => m.User)
                                       .Include(m => m.Ratings)
                                       .OrderByDescending(s => s.CreatedDate)
                                       .ToListAsync(),

                _ => await   _movies.Include(m => m.MovieGenres)
                                    .ThenInclude(mg => mg.Genre)
                                    .Include(m => m.MovieDirectors)
                                    .ThenInclude(md => md.Director)
                                    .Include(m => m.MovieActors)
                                    .ThenInclude(ma => ma.Actor)
                                    .Include(m => m.Comments)
                                    .ThenInclude(m => m.User)
                                    .Include(m => m.Ratings)
                                    .ToListAsync()
            };
        }
    }
}
