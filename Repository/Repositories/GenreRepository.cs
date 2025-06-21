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
    public class GenreRepository : BaseRepository<Genre>, IGenreRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<Genre> _genres;
        public GenreRepository(AppDbContext context) : base(context)
        {
            _context = context;
            _genres = context.Genres;
        }
        public async Task<IEnumerable<Genre>> GetAllAsync()
        {
            return await _genres.Include(g=>g.MovieGenres)
                                .ThenInclude(g=>g.Movie)
                                .Include(g=>g.SeriesGenres)
                                .ThenInclude(g=>g.Series)
                                .ToListAsync();
        }
        public async Task<Genre> GetByIdAsync(int id)
        {
            return await _genres.Include(g => g.MovieGenres)
                                .ThenInclude(g => g.Movie)
                                .Include(g => g.SeriesGenres)
                                .ThenInclude(g => g.Series)
                                .FirstOrDefaultAsync(g=>g.Id == id);
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _genres.Include(g => g.MovieGenres)
                                .ThenInclude(g => g.Movie)
                                .Include(g => g.SeriesGenres)
                                .ThenInclude(g => g.Series)
                                .AnyAsync(g => g.Name.ToLower() == name.ToLower());
        }

        public async Task<IEnumerable<Genre>> GetAllPaginatedAsync(int page, int take)
        {
            return await _genres.Include(g => g.MovieGenres)
                                .ThenInclude(g => g.Movie)
                                .Include(g => g.SeriesGenres)
                                .ThenInclude(g => g.Series)
                                .Skip(page * take - take)
                                .Take(take).ToListAsync();
        }

        public async Task<IEnumerable<Genre>> SearchAsync(string searchText)
        {
            return await _genres.Include(g => g.MovieGenres)
                                .ThenInclude(g => g.Movie)
                                .Include(g => g.SeriesGenres)
                                .ThenInclude(g => g.Series)
                                .Where(m => m.Name.Trim().ToLower()
                                .Contains(searchText.Trim().ToLower()))
                                .ToListAsync();
        }
    }
}
