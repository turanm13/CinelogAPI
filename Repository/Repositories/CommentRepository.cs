using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class CommentRepository : BaseRepository<Comment>, ICommentRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<Comment> _comments;

        public CommentRepository(AppDbContext context) : base(context)
        {
            _context = context;
            _comments = context.Comments;
        }
        public async Task<IEnumerable<Comment>> GetAllAsync()
        {
            return await _comments.Include(m=>m.User).ToListAsync();
        }
        public async Task<Comment> GetByIdAsync(int id)
        {
            return await _comments.Include(m=>m.User).FirstOrDefaultAsync(m=>m.Id == id);
        }

        public async Task<IEnumerable<Comment>> GetByMovieIdAsync(int movieId)
        {
            return await _comments.Include(m => m.User).Where(m=>m.MovieId == movieId).ToListAsync();
        }

        public async Task<IEnumerable<Comment>> GetByUserIdAsync(string userId)
        {
            return await _comments.Include(m => m.User).Where(m => m.UserId == userId).ToListAsync();
        }
    }
}
