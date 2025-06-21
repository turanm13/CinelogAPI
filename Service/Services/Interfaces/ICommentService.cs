using Service.DTOs.Comment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Interfaces
{
    public interface ICommentService
    {
        Task<IEnumerable<CommentDto>> GetAllAsync();
        Task<CommentDto> GetByIdAsync(int id);
        Task CreateAsync(CommentCreateDto request, string userName);
        Task UpdateAsync(int id, CommentUpdateDto request, string userId);
        Task DeleteAsync(int id, string userId);
        Task<IEnumerable<CommentDto>> GetByMovieIdAsync(int movieId);

        //// Seriala aid bütün şərhlər
        //Task<IEnumerable<CommentDto>> GetBySeriesIdAsync(int seriesId);

        Task<IEnumerable<CommentDto>> GetByUserIdAsync(string userId);




    }
}
