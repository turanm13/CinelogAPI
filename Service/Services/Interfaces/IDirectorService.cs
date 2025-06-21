using Service.DTOs.Actor;
using Service.DTOs.Director;
using Service.Helpers.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Interfaces
{
    public interface IDirectorService
    {
        Task<IEnumerable<DirectorDto>> GetAllAsync();
        Task CreateAsync(DirectorCreateDto director, string webRootPath);
        Task DeleteAsync(int id);
        Task<DirectorDto> GetByIdAsync(int id);
        Task<IEnumerable<DirectorDto>> GetDirectorsByMovieAsync(int movieId);
        Task<IEnumerable<DirectorDto>> GetDirectorsBySeriesAsync(int seriesId);

        Task UpdateAsync(int id, DirectorUpdateDto director, string? webRootPath);
        Task<IEnumerable<DirectorDto>> SearchAsync(string searchText);
        Task<IEnumerable<DirectorDto>> SortByCreateDateAsync(string order);
        Task<PaginateResponse<DirectorDto>> GetAllPaginatedAsync(int page, int take = 3);
    }
}
