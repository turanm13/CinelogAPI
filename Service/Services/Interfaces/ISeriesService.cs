using Service.DTOs.Movie;
using Service.DTOs.Series;
using Service.Helpers.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Interfaces
{
    public interface ISeriesService
    {
        Task<IEnumerable<SeriesDto>> GetAllAsync();
        Task CreateAsync(SeriesCreateDto series, string webRootPath);
        Task DeleteAsync(int id);
        Task<SeriesDto> GetByIdAsync(int id);
        Task UpdateAsync(int id, SeriesUpdateDto series, string? webRootPath);
        Task<IEnumerable<SeriesDto>> SearchAsync(string searchText);
        Task<IEnumerable<SeriesDto>> SortByCreateDateAsync(string order);
        Task<PaginateResponse<SeriesDto>> GetAllPaginatedAsync(int page, int take = 3);
        Task<IEnumerable<SeriesDto>> GetByGenreIdAsync(int genreId);
        Task<IEnumerable<SeriesDto>> GetByActorIdAsync(int actorId);
        Task<IEnumerable<SeriesDto>> GetByDirectorIdAsync(int directorId);
        Task<IEnumerable<SeriesDto>> FilterByReleaseYearAsync(int year);
    }
}
