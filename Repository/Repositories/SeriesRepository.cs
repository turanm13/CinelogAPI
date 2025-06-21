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
    public class SeriesRepository : BaseRepository<Series>, ISeriesRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<Series> _series;
        public SeriesRepository(AppDbContext context) : base(context)
        {
            _context = context;
            _series = context.Series;
        }

        public async Task<IEnumerable<Series>> FilterByReleaseYearAsync(int year)
        {
            return await _series.Include(m => m.SeriesGenres)
                                .ThenInclude(mg => mg.Genre)
                                .Include(m => m.SeriesDirectors)
                                .ThenInclude(md => md.Director)
                                .Include(m => m.SeriesActors)
                                .ThenInclude(ma => ma.Actor)
                                .Include(s=>s.Comments)
                                .ThenInclude(s=>s.User)
                                .Include(s=>s.Ratings)
                                .Where(m => m.ReleaseDate.Year == year)
                                .ToListAsync();
        }

        public async Task<IEnumerable<Series>> GetAllAsync()
        {
            return await _series.Include(m => m.SeriesGenres)
                                .ThenInclude(mg => mg.Genre)
                                .Include(m => m.SeriesDirectors)
                                .ThenInclude(md => md.Director)
                                .Include(m => m.SeriesActors)
                                .ThenInclude(ma => ma.Actor)
                                .Include(s => s.Comments)
                                .ThenInclude(s => s.User)
                                .Include(s => s.Ratings)
                                .ToListAsync();
        }

        public async Task<IEnumerable<Series>> GetAllPaginatedAsync(int page, int take)
        {
            return await _series.Include(m => m.SeriesGenres)
                                .ThenInclude(mg => mg.Genre)
                                .Include(m => m.SeriesDirectors)
                                .ThenInclude(md => md.Director)
                                .Include(m => m.SeriesActors)
                                .ThenInclude(ma => ma.Actor)
                                .Include(s => s.Comments)
                                .ThenInclude(s => s.User)
                                .Include(s => s.Ratings)
                                .Skip(page * take - take)
                                .Take(take).ToListAsync();
        }

        public async Task<IEnumerable<Series>> GetByActorIdAsync(int actorId)
        {
            return await _series.Include(m => m.SeriesGenres)
                                .ThenInclude(mg => mg.Genre)
                                .Include(m => m.SeriesDirectors)
                                .ThenInclude(md => md.Director)
                                .Include(m => m.SeriesActors)
                                .ThenInclude(ma => ma.Actor)
                                .Include(s => s.Comments)
                                .ThenInclude(s => s.User)
                                .Include(s => s.Ratings)
                                .Where(m => m.SeriesActors
                                .Any(m => m.ActorId == actorId))
                                .ToListAsync();
        }

        public async Task<IEnumerable<Series>> GetByDirectorIdAsync(int directorId)
        {
            return await _series.Include(m => m.SeriesGenres)
                                .ThenInclude(mg => mg.Genre)
                                .Include(m => m.SeriesDirectors)
                                .ThenInclude(md => md.Director)
                                .Include(m => m.SeriesActors)
                                .ThenInclude(ma => ma.Actor)
                                .Include(s => s.Comments)
                                .ThenInclude(s => s.User)
                                .Include(s => s.Ratings)
                                .Where(m => m.SeriesDirectors
                                .Any(m => m.DirectorId == directorId))
                                .ToListAsync();
        }

        public async Task<IEnumerable<Series>> GetByGenreIdAsync(int genreId)
        {
            return await _series.Include(m => m.SeriesGenres)
                                .ThenInclude(mg => mg.Genre)
                                .Include(m => m.SeriesDirectors)
                                .ThenInclude(md => md.Director)
                                .Include(m => m.SeriesActors)
                                .ThenInclude(ma => ma.Actor)
                                .Include(s => s.Comments)
                                .ThenInclude(s => s.User)
                                .Include(s => s.Ratings)
                                .Where(m => m.SeriesGenres
                                .Any(m => m.GenreId == genreId))
                                .ToListAsync();
        }

        public async Task<Series> GetByIdAsync(int id)
        {
            return await _series.Include(m => m.SeriesGenres)
                                .ThenInclude(mg => mg.Genre)
                                .Include(m => m.SeriesDirectors)
                                .ThenInclude(md => md.Director)
                                .Include(m => m.SeriesActors)
                                .ThenInclude(ma => ma.Actor)
                                .Include(s => s.Comments)
                                .ThenInclude(s => s.User)
                                .Include(s => s.Ratings)
                                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<Series>> SearchAsync(string searchText)
        {
            return _series.Include(m => m.SeriesGenres)
                          .ThenInclude(mg => mg.Genre)
                          .Include(m => m.SeriesDirectors)
                          .ThenInclude(md => md.Director)
                          .Include(m => m.SeriesActors)
                          .ThenInclude(ma => ma.Actor)
                          .Include(s => s.Comments)
                          .ThenInclude(s => s.User)
                          .Include(s => s.Ratings)
                          .Where(m => m.Title.Contains(searchText) ||  m.SeriesGenres
                          .Any(mg => mg.Genre.Name.Contains(searchText)));
        }

        public async Task<IEnumerable<Series>> SortByCreatedDateAsync(string order)
        {
            if (string.IsNullOrWhiteSpace(order))
                return await _series.Include(m => m.SeriesGenres).ThenInclude(mg => mg.Genre)
                                    .Include(m => m.SeriesDirectors).ThenInclude(md => md.Director)
                                    .Include(m => m.SeriesActors).ThenInclude(ma => ma.Actor)
                                    .Include(s => s.Comments)
                                    .ThenInclude(s => s.User)
                                    .Include(s => s.Ratings)
                                    .ToListAsync();

            return order.ToLower() switch
            {
                "asc" => await _series.Include(m => m.SeriesGenres)
                                      .ThenInclude(mg => mg.Genre)
                                      .Include(m => m.SeriesDirectors)
                                      .ThenInclude(md => md.Director)
                                      .Include(m => m.SeriesActors)
                                      .ThenInclude(ma => ma.Actor)
                                      .Include(s => s.Comments)
                                      .ThenInclude(s => s.User)
                                      .Include(s => s.Ratings)
                                      .OrderBy(s => s.CreatedDate)
                                      .ToListAsync(),

                "desc" => await _series.Include(m => m.SeriesGenres)
                                       .ThenInclude(mg => mg.Genre)
                                       .Include(m => m.SeriesDirectors)
                                       .ThenInclude(md => md.Director)
                                       .Include(m => m.SeriesActors)
                                       .ThenInclude(ma => ma.Actor)
                                       .Include(s => s.Comments)
                                       .ThenInclude(s => s.User)
                                       .Include(s => s.Ratings)
                                       .OrderByDescending(s => s.CreatedDate)
                                       .ToListAsync(),

                _ => await _series.Include(m => m.SeriesGenres)
                                  .ThenInclude(mg => mg.Genre)
                                  .Include(m => m.SeriesDirectors)
                                  .ThenInclude(md => md.Director)
                                  .Include(m => m.SeriesActors)
                                  .ThenInclude(ma => ma.Actor)
                                  .Include(s => s.Comments)
                                  .ThenInclude(s => s.User)
                                  .Include(s => s.Ratings)
                                  .ToListAsync()
            };
        }
    }
}
