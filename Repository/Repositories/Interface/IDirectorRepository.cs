using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.Interface
{
    public interface IDirectorRepository : IBaseRepository<Director>
    {
        public Task<IEnumerable<Director>> SearchAsync(string searchText);
        public Task<IEnumerable<Director>> SortByCreatedDateAsync(string order);
        public Task<IEnumerable<Director>> GetAllPaginatedAsync(int page, int take);
        Task<IEnumerable<Director>> GetDirectorsByMovieAsync(int movieId);
        Task<IEnumerable<Director>> GetDirectorsBySeriesAsync(int seriesId);
    }
}
