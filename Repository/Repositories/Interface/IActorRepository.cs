using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.Interface
{
    public interface IActorRepository : IBaseRepository<Actor>
    {
        Task<IEnumerable<Actor>> SearchAsync(string searchText);
        Task<IEnumerable<Actor>> SortByCreatedDateAsync(string order);
        Task<IEnumerable<Actor>> GetAllPaginatedAsync(int page, int take);
        Task<IEnumerable<Actor>> GetActorsByMovieAsync(int movieId);
        Task<IEnumerable<Actor>> GetActorsBySeriesAsync(int seriesId);
    }
}
