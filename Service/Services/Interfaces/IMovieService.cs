using Service.DTOs.Actor;
using Service.DTOs.Movie;
using Service.Helpers.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Interfaces
{
    public interface IMovieService
    {
        Task<IEnumerable<MovieDto>> GetAllAsync();
        Task CreateAsync(MovieCreateDto movie, string webRootPath);
        Task DeleteAsync(int id);
        Task<MovieDto> GetByIdAsync(int id);
        Task UpdateAsync(int id, MovieUpdateDto movie, string? webRootPath);
        Task<IEnumerable<MovieDto>> SearchAsync(string searchText);
        Task<IEnumerable<MovieDto>> SortByCreateDateAsync(string order);
        Task<PaginateResponse<MovieDto>> GetAllPaginatedAsync(int page, int take = 20);
        Task<IEnumerable<MovieDto>> GetByGenreIdAsync(int genreId);
        Task<IEnumerable<MovieDto>> GetByActorIdAsync(int actorId);
        Task<IEnumerable<MovieDto>> GetByDirectorIdAsync(int directorId);
        Task<IEnumerable<MovieDto>> FilterByReleaseYearAsync(int year);


    }
}
