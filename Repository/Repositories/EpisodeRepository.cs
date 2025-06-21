using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class EpisodeRepository : BaseRepository<Episode>, IEpisodeRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<Episode> _episodes;
        public EpisodeRepository(AppDbContext context) : base(context)
        {
            _context = context;
            _episodes = context.Episodes;
        }

        public async Task<IEnumerable<Episode>> GetAllAsync()
        {
            return await _episodes.Include(m => m.Series).ToListAsync();
        }

        public async Task<Episode> GetFirstOrDefaultAsync(Expression<Func<Episode, bool>> predicate)
        {
            return await _context.Episodes.FirstOrDefaultAsync(predicate);
        }

        public async Task<Episode> GetByIdAsync(int id)
        {
            return await _episodes.Include(m => m.Series).FirstOrDefaultAsync(m=>m.Id == id);
        }

        public async Task<IEnumerable<Episode>> SearchAsync(string searchText)
        {
            return await _episodes.Include(m => m.Series)
                                  .Where(m=>m.Title.Contains(searchText)).ToListAsync();
        }

        public async Task<IEnumerable<Episode>> SortByCreateDateAsync(string order)
        {
            if (string.IsNullOrWhiteSpace(order))
                return await _episodes.Include(m => m.Series).ToListAsync();

            return order.ToLower() switch
            {
                "asc" => await _episodes.Include(m => m.Series).OrderBy(s => s.CreatedDate).ToListAsync(),
                "desc" => await _episodes.Include(m => m.Series).OrderByDescending(s => s.CreatedDate).ToListAsync(),
                _ => await _episodes.Include(m => m.Series).ToListAsync()
            };
        }

        public async Task<IEnumerable<Episode>> GetAllPaginatedAsync(int page, int take)
        {
            return await _episodes.Include(m => m.Series).Skip(page * take - take).Take(take).ToListAsync();
        }

        public async Task<IEnumerable<Episode>> GetBySeriesIdAsync(int seriesId)
        {
            return await _episodes.Include(m=>m.Series).Where(m=>m.SeriesId == seriesId).ToListAsync();
        }
    }
}
