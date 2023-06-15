using Make_a_Drop.Core.Entities;
using Make_a_Drop.DataAccess.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Make_a_Drop.DataAccess.Repositories.Impl
{
    public class CommentRepository : BaseRepository<Comment>, ICommentRepository
    {
        public CommentRepository(DatabaseContext context) : base(context) { }

        public async Task<Comment> GetFirstAsync(Expression<Func<Comment, bool>> predicate)
        {
            return await DbSet.Where(predicate).Include(c => c.Drop).FirstOrDefaultAsync();
        }
        public async Task<List<Comment>> GetAllByDropIdAsync(Expression<Func<Comment, bool>> predicate)
        {
            return await DbSet.Where(predicate).Include(c => c.User).ToListAsync();
        }
        public IQueryable<Comment> OrderByDateAsc(Expression<Func<Comment, bool>> predicate)
        {
            return DbSet.Where(predicate).OrderBy(d => d.CreatedOn).Include(c => c.User);
        }

        public IQueryable<Comment> OrderByDateDesc(Expression<Func<Comment, bool>> predicate)
        {
            return DbSet.Where(predicate).OrderByDescending(d => d.CreatedOn).Include(c => c.User);
        }

        public IQueryable<Comment> PaginatedList(Expression<Func<Comment, bool>> predicate, int page, int pageSize)
        {
            return DbSet.Where(predicate).Include(c => c.User).Skip((page - 1) * pageSize).Take(pageSize);
        }
    }
}
