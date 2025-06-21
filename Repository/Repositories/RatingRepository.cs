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
    public class RatingRepository : BaseRepository<Rating>, IRatingRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<Rating> _ratings;
        public RatingRepository(AppDbContext context) : base(context)
        {
            _context = context;
            _ratings = context.Ratings;
        }

        public async Task<IEnumerable<Rating>> GetAllByUserAsync(string userId)
        {
            return await _ratings.Include(m => m.User)
                                 .Include(m => m.Series)
                                 .Include(m => m.Movie)
                                 .Where(m => m.UserId == userId).ToListAsync();
        }

        public async Task<Rating> GetByIdAsync(int id)
        {
            return await _ratings.Include(m => m.User)
                                 .Include(m => m.Series)
                                 .Include(m => m.Movie)
                                 .FirstOrDefaultAsync(m=>m.Id == id);
        }

        public async Task<IEnumerable<Rating>> GetMovieRatingsByUserAsync(string userId)
        {
            return await _ratings.Include(m => m.User)
                                 .Include(m => m.Series)
                                 .Include(m => m.Movie)
                                 .Where(m => m.UserId == userId && m.MovieId != null)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Rating>> GetSeriesRatingsByUserAsync(string userId)
        {
            return await _ratings.Include(m => m.User)
                                 .Include(m => m.Series)
                                 .Include(m => m.Movie)
                                 .Where(m => m.UserId == userId && m.SeriesId != null)
                                 .ToListAsync();
        }

        public async Task<bool> IsRatedAsync(string userId, int? movieId, int? seriesId)
        {
            return await _ratings.AnyAsync(f => f.UserId == userId &&
                                    ((movieId != null && f.MovieId == movieId) ||
                                    (seriesId != null && f.SeriesId == seriesId)));
        }
    }
}
