using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.Interface
{
    public interface IWatchlistRepository : IBaseRepository<Watchlist>
    {
        Task<IEnumerable<Watchlist>> GetAllByUserAsync(string userId);
        Task<IEnumerable<Watchlist>> GetMoviesByUserAsync(string userId);
        Task<IEnumerable<Watchlist>> GetSeriesByUserAsync(string userId);
        Task<bool> IsInWatchlistAsync(string userId, int? movieId, int? seriesId);
    }
}
