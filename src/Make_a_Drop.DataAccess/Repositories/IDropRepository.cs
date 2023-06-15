using Make_a_Drop.Core.Entities;
using System.Linq.Expressions;

namespace Make_a_Drop.DataAccess.Repositories;


public interface IDropRepository : IBaseRepository<Drop> {

    Task<List<Drop>> FindAllAsync();

    Task<Drop> FindByIdAsync(Expression<Func<Drop, bool>> predicate);

    Task<List<Drop>> GetExpired();

    Task<Drop> GetFirstAsync(Expression<Func<Drop, bool>> predicate);

    Task<Drop> GetByNameAsync(Expression<Func<Drop, bool>> predicate);

    IQueryable<Drop> OrderByNameAsc(Expression<Func<Drop, bool>> predicate);

    IQueryable<Drop> OrderByNameDesc(Expression<Func<Drop, bool>> predicate);

    IQueryable<Drop> OrderBySizeAsc(Expression<Func<Drop, bool>> predicate);

    IQueryable<Drop> OrderBySizeDesc(Expression<Func<Drop, bool>> predicate);

    IQueryable<Drop> SearchByName(Expression<Func<Drop, bool>> predicate, string name);

    IQueryable<Drop> PaginatedList(Expression<Func<Drop, bool>> predicate, int page, int pageSize);

}
    

