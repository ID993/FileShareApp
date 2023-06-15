using System.Linq.Expressions;
using Make_a_Drop.Core.Common;

namespace Make_a_Drop.DataAccess.Repositories;

public interface IBaseRepository<TEntity> where TEntity : BaseEntity
{
    Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate);

    Task<TEntity> AddAsync(TEntity entity);

    Task<TEntity> UpdateAsync(TEntity entity);

    Task<TEntity> DeleteAsync(TEntity entity);
}
