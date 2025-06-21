using Service.DTOs.Rating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Interfaces
{
    public interface IRatingService
    {
        Task AddAsync(RatingCreateDto request, string userId);
        Task<RatingDto> GetByIdAsync(int id, string userId);
        Task UpdateAsync(int id, RatingUpdateDto request, string userId);
        Task DeleteAsync(int id, string userId);
        Task<IEnumerable<RatingDto>> GetAllByUserAsync(string userId);
        Task<IEnumerable<RatingDto>> GetMovieRatingsByUserAsync(string userId);
        Task<IEnumerable<RatingDto>> GetSeriesRatingsByUserAsync(string userId);

        Task<bool> IsRatedAsync(string userId, int? movieId, int? seriesId);

        //// Movie və ya Series üçün orta reytinq (ağırlıq ortası və ya sadə orta)
        //Task<double> GetAverageRatingAsync(int? movieId, int? seriesId);
    }
}
