using Make_a_Drop.Core.Entities;
using System.Linq.Expressions;

namespace Make_a_Drop.DataAccess.Repositories
{
    public interface ICommentRepository : IBaseRepository<Comment> 
    {
        Task<List<Comment>> GetAllByDropIdAsync(Expression<Func<Comment, bool>> predicate);

        Task<Comment> GetFirstAsync(Expression<Func<Comment, bool>> predicate);
        
        IQueryable<Comment> OrderByDateAsc(Expression<Func<Comment, bool>> predicate);

        IQueryable<Comment> OrderByDateDesc(Expression<Func<Comment, bool>> predicate);

        IQueryable<Comment> PaginatedList(Expression<Func<Comment, bool>> predicate, int page, int pageSize);
    }
}
