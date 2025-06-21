using Service.DTOs.Favorite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Interfaces
{
    public interface IFavoriteService
    {
        Task AddAsync(FavoriteCreateDto request, string userId);
        Task<IEnumerable<FavoriteDto>> GetAllByUserAsync(string userId);
        Task<FavoriteDto> GetByIdAsync(int id, string userId);
        Task DeleteAsync(int id, string userId);
        Task<IEnumerable<FavoriteDto>> GetMoviesByUserAsync(string userId);
        Task<IEnumerable<FavoriteDto>> GetSeriesByUserAsync(string userId);
        Task<bool> IsFavoritedAsync(string userId, int? movieId, int? seriesId);
    }
}
