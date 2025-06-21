using Service.DTOs.Director;
using Service.DTOs.Genre;
using Service.Helpers.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Interfaces
{
    public interface IGenreService
    {
        Task<IEnumerable<GenreDto>> GetAllAsync();
        Task<GenreDto> GetByIdAsync(int id);
        Task CreateAsync(GenreCreateDto genre);
        Task DeleteAsync(int id);
        Task UpdateAsync(int id, GenreUpdateDto genre);
        Task<IEnumerable<GenreDto>> SearchAsync(string searchText);
        Task<PaginateResponse<GenreDto>> GetAllPaginatedAsync(int page, int take = 10);
        Task<bool> ExistsByNameAsync(string name); 
    }
}
