using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.Interface
{
    public interface ICommentRepository : IBaseRepository<Comment>
    {
        Task<IEnumerable<Comment>> GetByMovieIdAsync(int movieId);
        Task<IEnumerable<Comment>> GetByUserIdAsync(string userId);
    }
}
