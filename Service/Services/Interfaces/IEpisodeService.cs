using Service.DTOs.Episode;
using Service.DTOs.Series;
using Service.Helpers.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Interfaces
{
    public interface IEpisodeService
    {
        Task<IEnumerable<EpisodeDto>> GetAllAsync();
        Task CreateAsync(EpisodeCreateDto episode);
        Task DeleteAsync(int id);
        Task<EpisodeDto> GetByIdAsync(int id);
        Task UpdateAsync(int id, EpisodeUpdateDto episode);
        Task<IEnumerable<EpisodeDto>> SearchAsync(string searchText);
        Task<IEnumerable<EpisodeDto>> SortByCreateDateAsync(string order);
        Task<PaginateResponse<EpisodeDto>> GetAllPaginatedAsync(int page, int take = 3);
        Task<IEnumerable<EpisodeDto>> GetBySeriesIdAsync(int seriesId);
    }
}
