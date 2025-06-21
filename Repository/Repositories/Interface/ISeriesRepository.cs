using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.Interface
{
    public interface ISeriesRepository : IBaseRepository<Series>
    {
        public Task<IEnumerable<Series>> SearchAsync(string searchText);
        public Task<IEnumerable<Series>> SortByCreatedDateAsync(string order);
        Task<IEnumerable<Series>> GetAllPaginatedAsync(int page, int take);
        Task<IEnumerable<Series>> GetByGenreIdAsync(int genreId);
        Task<IEnumerable<Series>> GetByActorIdAsync(int actorId);
        Task<IEnumerable<Series>> GetByDirectorIdAsync(int directorId);
        Task<IEnumerable<Series>> FilterByReleaseYearAsync(int year);

    }
}
