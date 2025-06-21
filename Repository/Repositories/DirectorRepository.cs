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
    public class DirectorRepository : BaseRepository<Director>, IDirectorRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<Director> _directors;
        public DirectorRepository(AppDbContext context) : base(context)
        {
            _context = context;
            _directors = context.Directors;
        }
        public async Task<IEnumerable<Director>> GetAllAsync()
        {
            return await _directors.Include(d=>d.MovieDirectors)
                                   .ThenInclude(d=>d.Movie)
                                   .Include(d=>d.SeriesDirectors)
                                   .ThenInclude(d=>d.Series)
                                   .ToListAsync();
        }
        public async Task<Director> GetByIdAsync(int id)
        {
            return await _directors.Include(d => d.MovieDirectors)
                                   .ThenInclude(d => d.Movie)
                                   .Include(d => d.SeriesDirectors)
                                   .ThenInclude(d => d.Series)
                                   .FirstOrDefaultAsync(d => d.Id == id);
        }
        public async Task<IEnumerable<Director>> GetAllPaginatedAsync(int page, int take)
        {
            return await _directors.Include(d => d.MovieDirectors)
                                   .ThenInclude(d => d.Movie)
                                   .Include(d => d.SeriesDirectors)
                                   .ThenInclude(d => d.Series)
                                   .Skip(page * take - take)
                                   .Take(take).ToListAsync();
        }

        public async Task<IEnumerable<Director>> SearchAsync(string searchText)
        {
            return await _directors.Include(d => d.MovieDirectors)
                                   .ThenInclude(d => d.Movie)
                                   .Include(d => d.SeriesDirectors)
                                   .ThenInclude(d => d.Series)
                                   .Where(m => m.FullName.Trim().ToLower()
                                   .Contains(searchText.Trim().ToLower()))
                                   .ToListAsync();
        }

        public async Task<IEnumerable<Director>> SortByCreatedDateAsync(string order)
        {
            if (string.IsNullOrWhiteSpace(order))
                return await _directors.Include(d => d.MovieDirectors)
                                       .ThenInclude(d => d.Movie)
                                       .Include(d => d.SeriesDirectors)
                                       .ThenInclude(d => d.Series)
                                       .ToListAsync();

            return order.ToLower() switch
            {
                "asc" => await _directors.Include(d => d.MovieDirectors)
                                         .ThenInclude(d => d.Movie)
                                         .Include(d => d.SeriesDirectors)
                                         .ThenInclude(d => d.Series).OrderBy(s => s.CreatedDate).ToListAsync(),
                "desc" => await _directors.Include(d => d.MovieDirectors)
                                          .ThenInclude(d => d.Movie)
                                          .Include(d => d.SeriesDirectors)
                                          .ThenInclude(d => d.Series)
                                          .OrderByDescending(s => s.CreatedDate)
                                          .ToListAsync(),
                _ => await _directors.Include(d => d.MovieDirectors)
                                     .ThenInclude(d => d.Movie)
                                     .Include(d => d.SeriesDirectors)
                                     .ThenInclude(d => d.Series)
                                     .ToListAsync()
            };
        }

        public async Task<IEnumerable<Director>> GetDirectorsByMovieAsync(int movieId)
        {
            return await _directors.Include(d => d.MovieDirectors)
                                   .ThenInclude(d => d.Movie)
                                   .Where(d => d.MovieDirectors.Any(d => d.MovieId == movieId))
                                   .ToListAsync();
        }

        public async Task<IEnumerable<Director>> GetDirectorsBySeriesAsync(int seriesId)
        {
            return await _directors.Include(d => d.SeriesDirectors)
                                   .ThenInclude(d => d.Series)
                                   .Where(d => d.SeriesDirectors.Any(d => d.SeriesId == seriesId))
                                   .ToListAsync();
        }
    }
}
