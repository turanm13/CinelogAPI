using Service.DTOs.Favorite;
using Service.DTOs.Watchlist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Interfaces
{
    public interface IWatchlistService
    {
        Task AddAsync(WatchlistCreateDto request, string userId);
        Task<IEnumerable<WatchlistDto>> GetAllByUserAsync(string userId);
        Task<WatchlistDto> GetByIdAsync(int id, string userId);
        Task DeleteAsync(int id, string userId);
        Task<IEnumerable<WatchlistDto>> GetMoviesByUserAsync(string userId);
        Task<IEnumerable<WatchlistDto>> GetSeriesByUserAsync(string userId);
        Task<bool> IsInWatchlistAsync(string userId, int? movieId, int? seriesId);
    }
}
