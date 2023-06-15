using Make_a_Drop.Core.Entities;
using System.Linq.Expressions;

namespace Make_a_Drop.DataAccess.Repositories;

public interface ICollaborationRepository : IBaseRepository<Collaboration> 
{
    Task<Collaboration> FindByIdAsync(Expression<Func<Collaboration, bool>> predicate);
    Task<List<Collaboration>> FindAllByUserIdAsync(Expression<Func<Collaboration, bool>> predicate);
    Task<List<Collaboration>> FindAllAsync();

    Task<Collaboration> GetFirstAsync(Expression<Func<Collaboration, bool>> predicate);
    
    IQueryable<Collaboration> OrderByNameAsc(Expression<Func<Collaboration, bool>> predicate);

    IQueryable<Collaboration> OrderByNameDesc(Expression<Func<Collaboration, bool>> predicate);

    IQueryable<Collaboration> SearchByName(Expression<Func<Collaboration, bool>> predicate, string name);

    IQueryable<Collaboration> PaginatedList(Expression<Func<Collaboration, bool>> predicate, int page, int pageSize);
}
