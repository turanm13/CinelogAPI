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
    public class WatchlistRepository : BaseRepository<Watchlist>, IWatchlistRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<Watchlist> _watches;
        public WatchlistRepository(AppDbContext context) : base(context)
        {
            _context = context;
            _watches = context.Watchlists;
        }

        public async Task<IEnumerable<Watchlist>> GetAllByUserAsync(string userId)
        {
            return await _watches.Include(m => m.User)
                                   .Include(m => m.Series)
                                   .Include(m => m.Movie)
                                   .Where(m => m.UserId == userId)
                                   .ToListAsync();
        }
        public async Task<Watchlist> GetByIdAsync(int id)
        {
            return await _watches.Include(m => m.User)
                                   .Include(m => m.Series)
                                   .Include(m => m.Movie)
                                   .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<Watchlist>> GetMoviesByUserAsync(string userId)
        {
            return await _watches.Include(m => m.User)
                                   .Include(m => m.Movie)
                                   .Where(m => m.UserId == userId && m.MovieId != null)
                                   .ToListAsync();
        }

        public async Task<IEnumerable<Watchlist>> GetSeriesByUserAsync(string userId)
        {
            return await _watches.Include(m => m.User)
                                   .Include(m => m.Movie)
                                   .Where(m => m.UserId == userId && m.SeriesId != null)
                                   .ToListAsync();
        }

        public async Task<bool> IsInWatchlistAsync(string userId, int? movieId, int? seriesId)
        {
            return await _watches.AnyAsync(f => f.UserId == userId &&
                                    ((movieId != null && f.MovieId == movieId) ||
                                    (seriesId != null && f.SeriesId == seriesId)));
        }
    }
}
