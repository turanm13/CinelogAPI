using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.Interface
{
    public interface IEpisodeRepository : IBaseRepository<Episode>
    {
        Task<Episode> GetFirstOrDefaultAsync(Expression<Func<Episode, bool>> predicate);
        Task<IEnumerable<Episode>> SearchAsync(string searchText);
        Task<IEnumerable<Episode>> SortByCreateDateAsync(string order);
        Task<IEnumerable<Episode>> GetAllPaginatedAsync(int page, int take);
        Task<IEnumerable<Episode>> GetBySeriesIdAsync(int seriesId);
    }
}
