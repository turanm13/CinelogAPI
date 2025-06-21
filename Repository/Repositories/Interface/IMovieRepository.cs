using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.Interface
{
    public interface IMovieRepository : IBaseRepository<Movie>
    {
        public Task<IEnumerable<Movie>> SearchAsync(string searchText);
        public Task<IEnumerable<Movie>> SortByCreatedDateAsync(string order);
        Task<IEnumerable<Movie>> GetAllPaginatedAsync(int page, int take);
        Task<IEnumerable<Movie>> GetByGenreIdAsync(int genreId);
        Task<IEnumerable<Movie>> GetByActorIdAsync(int actorId);
        Task<IEnumerable<Movie>> GetByDirectorIdAsync(int directorId);
        Task<IEnumerable<Movie>> FilterByReleaseYearAsync(int year);
    }
}
