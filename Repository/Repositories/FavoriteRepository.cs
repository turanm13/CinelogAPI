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
    public class FavoriteRepository : BaseRepository<Favorite>, IFavoriteRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<Favorite> _favorites;
        public FavoriteRepository(AppDbContext context) : base(context)
        {
            _context = context;
            _favorites = context.Favorites;
        }

        public async Task<IEnumerable<Favorite>> GetAllByUserAsync(string userId)
        {
            return await _favorites.Include(m=>m.User)
                                   .Include(m=>m.Series)
                                   .Include(m=>m.Movie)
                                   .Where(m=>m.UserId == userId)
                                   .ToListAsync();
        }

        public async Task<Favorite> GetByIdAsync(int id)
        {
            return await _favorites.Include(m => m.User)
                                   .Include(m => m.Series)
                                   .Include(m => m.Movie)
                                   .FirstOrDefaultAsync(m=>m.Id == id);
        }

        public async Task<IEnumerable<Favorite>> GetMoviesByUserAsync(string userId)
        {
            return await _favorites.Include(m => m.User)
                                   .Include(m => m.Movie)
                                   .Where(m => m.UserId == userId && m.MovieId != null)
                                   .ToListAsync();
        }

        public async Task<IEnumerable<Favorite>> GetSeriesByUserAsync(string userId)
        {
            return await _favorites.Include(m => m.User)
                                   .Include(m => m.Movie)
                                   .Where(m => m.UserId == userId && m.SeriesId != null)
                                   .ToListAsync();
        }

        public async Task<bool> IsFavoritedAsync(string userId, int? movieId, int? seriesId)
        {
            return await _favorites.AnyAsync(f => f.UserId == userId &&
                                    ((movieId != null && f.MovieId == movieId) ||
                                    (seriesId != null && f.SeriesId == seriesId)));
        }
    }
}
