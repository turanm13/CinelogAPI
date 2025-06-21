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
    public class ActorRepository : BaseRepository<Actor> , IActorRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<Actor> _actors;

        public ActorRepository(AppDbContext context) : base(context)
        {
            _context = context;
            _actors = context.Actors;
        }

        public async Task<IEnumerable<Actor>> GetAllAsync()
        {
            return await _actors.Include(m=>m.MovieActors)
                                .ThenInclude(m=>m.Movie)
                                .Include(s=>s.SeriesActors)
                                .ThenInclude(s=>s.Series)
                                .ToListAsync();
        }

        public async Task<Actor> GetByIdAsync(int id)
        {
            return await _actors.Include(m => m.MovieActors)
                                .ThenInclude(m => m.Movie)
                                .Include(s => s.SeriesActors)
                                .ThenInclude(s => s.Series)
                                .FirstOrDefaultAsync(m=>m.Id == id);
        }
        public async Task<IEnumerable<Actor>> GetAllPaginatedAsync(int page, int take)
        {
            return await _actors.Include(m => m.MovieActors)
                                .ThenInclude(m => m.Movie)
                                .Include(s => s.SeriesActors)
                                .ThenInclude(s => s.Series)
                                .Skip(page * take - take).Take(take)
                                .ToListAsync();
        }

        public async Task<IEnumerable<Actor>> SearchAsync(string searchText)
        {
            return await _actors.Include(m => m.MovieActors)
                                .ThenInclude(m => m.Movie)
                                .Include(s => s.SeriesActors)
                                .ThenInclude(s => s.Series)
                                .Where(m => m.FullName.Trim().ToLower().Contains(searchText.Trim().ToLower()))
                                .ToListAsync();
        }

        

        public async Task<IEnumerable<Actor>> SortByCreatedDateAsync(string order)
        {
            if (string.IsNullOrWhiteSpace(order))
                return await _actors.Include(m => m.MovieActors)
                                    .ThenInclude(m => m.Movie)
                                    .Include(s => s.SeriesActors)
                                    .ThenInclude(s => s.Series)
                                    .ToListAsync();

            return order.ToLower() switch
            {
                "asc" => await _actors.Include(m => m.MovieActors)
                                      .ThenInclude(m => m.Movie)
                                      .Include(s => s.SeriesActors)
                                      .ThenInclude(s => s.Series)
                                      .OrderBy(s => s.CreatedDate)
                                      .ToListAsync(),
                "desc" => await _actors.Include(m => m.MovieActors)
                                       .ThenInclude(m => m.Movie)
                                       .Include(s => s.SeriesActors)
                                       .ThenInclude(s => s.Series)
                                       .OrderByDescending(s => s.CreatedDate)
                                       .ToListAsync(),
                _ => await _actors.Include(m => m.MovieActors)
                                  .ThenInclude(m => m.Movie)
                                  .Include(s => s.SeriesActors)
                                  .ThenInclude(s => s.Series)
                                  .ToListAsync()
            };
        }

        public async Task<IEnumerable<Actor>> GetActorsByMovieAsync(int movieId)
        {
            return await _actors.Include(m => m.MovieActors)
                                .ThenInclude(m => m.Movie)
                                .Where(m => m.MovieActors.Any(m => m.Movie.Id == movieId))
                                .ToListAsync();
        }

        public async Task<IEnumerable<Actor>> GetActorsBySeriesAsync(int seriesId)
        {
            return await _actors.Include(m => m.SeriesActors)
                                .ThenInclude(m => m.Series)
                                .Where(m => m.SeriesActors.Any(m => m.Series.Id == seriesId))
                                .ToListAsync();
        }
    }
}
