using Service.DTOs.Actor;
using Service.Helpers.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Interfaces
{
    public interface IActorService
    {
        Task<IEnumerable<ActorDto>> GetAllAsync();
        Task CreateAsync(ActorCreateDto actor, string webRootPath);
        Task DeleteAsync(int id);
        Task<ActorDto> GetByIdAsync(int id);
        Task<IEnumerable<ActorDto>> GetActorsByMovieAsync(int movieId);
        Task<IEnumerable<ActorDto>> GetActorsBySeriesAsync(int seriesId);

        Task UpdateAsync(int id, ActorUpdateDto actor, string? webRootPath);
        Task<IEnumerable<ActorDto>> SearchAsync(string searchText);
        Task<IEnumerable<ActorDto>> SortByCreateDateAsync(string order);
        Task<PaginateResponse<ActorDto>> GetAllPaginatedAsync(int page, int take = 3);
    }
}
