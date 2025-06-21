using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.Interface
{
    public interface IRatingRepository : IBaseRepository<Rating>
    {
        Task<IEnumerable<Rating>> GetAllByUserAsync(string userId);
        Task<IEnumerable<Rating>> GetMovieRatingsByUserAsync(string userId);
        Task<IEnumerable<Rating>> GetSeriesRatingsByUserAsync(string userId);
        Task<bool> IsRatedAsync(string userId, int? movieId, int? seriesId);
    }
}
