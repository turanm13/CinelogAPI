using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.Interface
{
    public interface IFavoriteRepository : IBaseRepository<Favorite>
    {
        Task<IEnumerable<Favorite>> GetAllByUserAsync(string userId);
        Task<IEnumerable<Favorite>> GetMoviesByUserAsync(string userId);
        Task<IEnumerable<Favorite>> GetSeriesByUserAsync(string userId);
        Task<bool> IsFavoritedAsync(string userId, int? movieId, int? seriesId);

    }
}
