using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.Interface
{
    public interface IGenreRepository : IBaseRepository<Genre>
    {
        public Task<IEnumerable<Genre>> SearchAsync(string searchText);
        public Task<IEnumerable<Genre>> GetAllPaginatedAsync(int page, int take);
        public Task<bool> ExistsByNameAsync(string name);


    }
}
